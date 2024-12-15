'use client'

import { AjaxFormContext } from './BaseForm'
import { useContext, useEffect, useState } from 'react';
import {
    Input
} from '@chakra-ui/react'
import AsyncSelect from 'react-select/async'
import {
    Field
} from '@/components/ui/field'
import axios from '@/backend'
import backend from '@/backend';
import { debounce, filter } from 'lodash';

export function AsyncSelectField( {
    name, 
    type,
    rules,
    value,
    url,
    optionLabelKey = 'id',
    optionValueKey = 'label',
    placeholder,
    children,
}) {

    const { register, setValue, watch, errors } = useContext( AjaxFormContext )

    register(
        name,
        {
            ...rules,
            value,
        }
    )

    const formValue = watch( name )
    const [options, setOptions] = useState([])
    const [selectedValue, setSelectedValue] = useState( null )
    useEffect(() => {
        setSelectedValue( options.find( v => getOptionValue( v ) == formValue ) )
    }, [formValue])

    const getOptionValue = option => {
        return option[optionValueKey] ?? null 
    } 

    const getOptionLabel = option => {
        return option[optionLabelKey] ?? null
    }

    const filterResults = ( results, inputValue ) => {
        if ( !Array.isArray( results ) ) return [];

        return results.filter( 
            i => getOptionLabel( i ) != null && getOptionValue( i ) != null 
        ).filter(
            i => String( inputValue ).trim() == '' 
                || String( getOptionLabel( i ) ).toLowerCase().startsWith( inputValue.toLowerCase() )
        )
    };

    const loadOptions = (
        inputValue,
        callback
    ) => {
        backend.get( url, {
            params: {
                search: inputValue
            }
        } )
            .then( res => {
                let filtered = filterResults( res.data, inputValue ) 
                setOptions( filtered )
                callback( filtered ) 
            })
            .catch( err => console.log( err ) )
    }

    return (
        <Field label={children} required={rules?.required} className="form-control">
            <AsyncSelect
                value={selectedValue}
                loadOptions={debounce( loadOptions, 500 )}
                onChange={v => setValue( name, getOptionValue( v ) )}
                getOptionValue={getOptionValue}
                getOptionLabel={getOptionLabel}
                cacheOptions
                defaultOptions
                name={name}
                className='form-control-full-width'
            >
            </AsyncSelect>
        </Field>
    )
}