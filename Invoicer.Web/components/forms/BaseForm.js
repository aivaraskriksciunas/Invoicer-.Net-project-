'use client'

import { useForm, SubmitHandler } from "react-hook-form"
import { createContext, useState } from 'react'
import { Button } from '@chakra-ui/react'
import { useRouter } from 'next/navigation'
import backend from '@/backend'

export const AjaxFormContext = createContext()

export default function BaseForm( {
    action,
    method,
    onSuccess,
    redirectOnSuccess,
    onError,
    children,
    urlParams,
} ) {

    const {
        register,
        control,
        handleSubmit,
        formState: { errors },
        setValue,
        watch,
    } = useForm()

    const router = useRouter();

    const [isLoading, setIsLoading] = useState( false )

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

            <AjaxFormContext.Provider value={{ register, setValue, watch, control, errors }}>
                {children( isLoading )}
            </AjaxFormContext.Provider>

        </form>
    )

}