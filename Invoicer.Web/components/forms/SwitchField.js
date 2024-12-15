'use client'

import { AjaxFormContext } from '@/components/forms/BaseForm'
import { useContext } from 'react'
import { Field } from '@/components/ui/field'
import { Switch } from "@/components/ui/switch"

export function SwitchField( {
    name,
    rules,
    value = false,
    children,
    showTimeInput = false,
} ) {

    const { register, setValue, watch, errors } = useContext( AjaxFormContext )

    const { onBlur, name: fieldName } = register(
        name,
        {
            ...rules,
            value,
            setValueAs: v => !!v,
        }
    )

    const isChecked = watch( name )

    return (
        <Field label={children} required={rules?.required} className="form-control">
            <Switch
                name={fieldName}
                checked={isChecked}
                onCheckedChange={({ checked }) => setValue( name, checked ) }
                inputProps={{ onBlur }}
            >
                {children}
            </Switch>
        </Field>
    )
}