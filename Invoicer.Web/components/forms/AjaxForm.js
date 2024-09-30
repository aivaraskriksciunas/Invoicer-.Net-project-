'use client'

import { useForm, SubmitHandler } from "react-hook-form"
import { createContext } from 'react'
import { Button } from '@chakra-ui/react'
import backend from '@/backend'

export const AjaxFormContext = createContext()

export default function AjaxForm({
    action,
    method,
    onSuccess,
    children,
}) {

    const {
        register,
        control,
        handleSubmit,
        formState: { errors },
    } = useForm()

    const onSubmit = async ( data ) => {

        try {
            let result = backend.request( {
                url: action,
                method: method || 'get',
            } )

            if ( typeof onSuccess === 'function' ) {
                onSuccess( result )
            }
        }
        catch ( e ) {
            console.log( e )
        }
    }

    return (
        <form onSubmit={handleSubmit( onSubmit )}>

            <AjaxFormContext.Provider value={{ register, control, errors }}>
                { children }
            </AjaxFormContext.Provider>

            <div>
                <Button type="submit">
                    Submit
                </Button>
            </div>
        </form>
    )

}