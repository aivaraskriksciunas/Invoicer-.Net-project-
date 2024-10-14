'use client'

import { useSelector, useDispatch } from 'react-redux'
import { setUser, logout, selectUser } from '@/stores/userSlice'
import { useEffect } from 'react'
import { useRouter } from 'next/navigation'
import backend from '@/backend'

export default function UserProviderWrapper( { children } ) {

    const user = useSelector( selectUser )
    const dispatch = useDispatch()
    const router = useRouter()


    useEffect( () => {

        async function fetchUserData() {
            try {
                let res = await backend.get( '/api/user' )
                dispatch( setUser( res.data ) )
            }
            catch ( err ) {
                dispatch( logout() )
                router.push( "/auth/login" )
            }
        }

        fetchUserData()
        
    }, [] )


    if ( user == null ) {
        return <p>Loading...</p>
    }

    return (
        <>
            <h1>User logged in</h1>
            { children }
        </>
    )
}