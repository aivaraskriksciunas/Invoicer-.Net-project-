'use client'

import { AjaxFormContext } from './BaseForm'
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