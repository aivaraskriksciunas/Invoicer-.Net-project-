'use client'

import { useForm, SubmitHandler } from "react-hook-form"
import { createContext, useState } from 'react'
import { Button } from '@chakra-ui/react'
import backend from '@/backend'

export const AjaxFormContext = createContext()

export default function AjaxForm({
    action,
    method,
    onSuccess,
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