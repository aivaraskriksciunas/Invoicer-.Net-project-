'use client'

import { AjaxFormContext } from './AjaxForm'
import { useContext } from 'react';
import {
    Input
} from '@chakra-ui/react'
import {
    Field
} from '@/components/ui/field'

export default function TextField( {
    name, 
    type,
    rules,
    value,
    children,
}) {

    const { register, errors } = useContext( AjaxFormContext )
    console.log(value)
    return (
        <Field label={children} required={rules?.required}>
            <Input
                type={type || 'text'}
                {...register( name, {
                    ...rules, 
                    value,
                } )}>
            </Input>
        </Field>
    )
}