'use client'

import AjaxForm from "@/components/forms/AjaxForm"
import TextField from '@/components/forms/TextField'
import { useRouter } from 'next/navigation'

export default function RegisterPage() {

    const router = useRouter()

    const onRegister = ( data ) => {
        router.push( "/dashboard" )
    }

    const onError = ( statusCode, response ) => {
        console.log( "Invalid credentials." )
    }

    return (
        <>
            Create an account
            <AjaxForm action="/register" method='post'
                onSuccess={onRegister}
                onError={onError}>
                <TextField
                    name="email"
                    type="email"
                    rules={{ required: true, minLength: 3 }}
                >
                    Email
                </TextField>
                <TextField
                    name="firstName"
                    rules={{ required: true }}
                >
                    First name
                </TextField>
                <TextField
                    name="lastName"
                >
                    Last name
                </TextField>
                <TextField
                    name="password"
                    type="password"
                    rules={{ required: true, minLength: 5 }}>
                    Password
                </TextField>
                <TextField
                    name="passwordConfirm"
                    type="password"
                    rules={{ required: true, minLength: 5 }}>
                    Confirm password
                </TextField>
            </AjaxForm>
        </>
    )
}