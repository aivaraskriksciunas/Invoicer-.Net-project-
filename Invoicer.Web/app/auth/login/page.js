'use client'

import AjaxForm from "@/components/forms/AjaxForm"
import TextField from '@/components/forms/TextField'
import { useRouter } from 'next/navigation'

export default function Page() {

    const router = useRouter()

    const onLogin = ( data ) => {
        router.push( "/dashboard" )
    }

    const onError = ( statusCode, response ) => {
        console.log( "Invalid credentials." )
    }

    return (
        <>
            Login form
            <AjaxForm action="/login" method='post'
                onSuccess={onLogin}
                onError={onError}
                urlParams={{ useCookies: true }}>
                <TextField
                    name="email">Email</TextField>
                <TextField
                    name="password"
                    type="password"
                    rules={{ minLength: 5 }}>
                    Password 
                </TextField>
            </AjaxForm>
        </>
    )
}