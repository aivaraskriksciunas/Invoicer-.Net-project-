'use client'

import { AjaxFormContext } from './AjaxForm'
import { useContext } from 'react';
import {
    FormControl,
    FormLabel,
    Input
} from '@chakra-ui/react'

export default function TextField( {
    name, 
    type,
    rules,
    children,
}) {

    const { register, errors } = useContext( AjaxFormContext )

    return (
        <FormControl>
            <FormLabel>{children}</FormLabel>
            <Input
                type={type || 'text'}
                {...register( name, rules || {} )}>
            </Input>
        </FormControl>
    )
}