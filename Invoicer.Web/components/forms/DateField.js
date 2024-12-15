'use client'

import { AjaxFormContext } from './BaseForm'
import { useContext } from 'react';
import {
    Input
} from '@chakra-ui/react'
import {
    Field
} from '@/components/ui/field'
import DatePicker from 'react-datepicker'
import "react-datepicker/dist/react-datepicker.css";

export default function DateField( {
    name,
    rules,
    value = null,
    children,
    showTimeInput = false,
} ) {

    if ( ( new Date( value ) ).valueOf() === NaN ) {
        value = null
    }

    const { register, setValue, watch, errors } = useContext( AjaxFormContext )

    const props = register(
        name,
        {
            ...rules,
            value,
            valueAsDate: true,
        }
    )

    const selectedDate = watch( name )

    return (
        <Field label={children} required={rules?.required} className="form-control">
            <DatePicker
                customInput={<Input />}
                showTimeInput={showTimeInput}
                selected={selectedDate}
                dateFormat="yyyy-MM-dd HH:mm"
                onChange={value => setValue( name, value )}
            >
            </DatePicker>
        </Field>
    )
}