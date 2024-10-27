'use client'

import { useForm, SubmitHandler } from "react-hook-form"
import { createContext, useState } from 'react'
import { Button } from '@chakra-ui/react'
import { useRouter } from 'next/router'
import backend from '@/backend'

export const AjaxFormContext = createContext()

export default function AjaxForm({
    action,
    method,
    onSuccess,
    redirectOnSuccess,
    onError,
    children,
    urlParams,
}) {

    const {
        register,
        control,
        handleSubmit,
        formState: { errors },
    } = useForm()

    const router = useRouter();

    const [ isLoading, setIsLoading ] = useState( false )

    const onSubmit = async ( data ) => {

        urlParams = urlParams || {}

        try {
            setIsLoading( true );

            let result = await backend.request( {
                url: action,
                method: method || 'get',
                data,
                params: urlParams,
            } )

            if ( typeof onSuccess === 'function' ) {
                onSuccess( result )
            }
            else if ( redirectOnSuccess ) {
                router.push( redirectOnSuccess )
            }
        }
        catch ( e ) {
            if ( onError ) {
                onError( e.statusCode, e.response )
            }
            else {
                console.log( e )
            }
        }

        setIsLoading( false )
    }

    return (
        <form onSubmit={handleSubmit( onSubmit )}>

            <AjaxFormContext.Provider value={{ register, control, errors }}>
                { children }
            </AjaxFormContext.Provider>

            <div>
                <Button isLoading={isLoading} type="submit">
                    Submit
                </Button>
            </div>
        </form>
    )

}