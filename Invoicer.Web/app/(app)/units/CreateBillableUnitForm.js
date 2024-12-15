'use client'
import { useState } from 'react'
import { Button } from '@chakra-ui/react'
import PopupAjaxForm from '@/components/forms/PopupAjaxForm'
import TextField from '@/components/forms/TextField'
import DateField from '@/components/forms/DateField'
import { SwitchField } from '@/components/forms/SwitchField'

export default function CreateBillableRecord( { clientId, onSuccess = () => { } } ) {

    return (
        <PopupAjaxForm
            action={`/Api/BillableUnit/`}
            method="post"
            onSuccess={onSuccess}
            trigger={<Button variant="outline">Add unit</Button>}>
            <TextField
                name="fullName"
                rules={{ required: true, minLenght: 3 }}
                placeholder="e.g. meters, hours, units">
                Unit full name
            </TextField>
            <TextField
                name="shortName"
                rules={{ required: true, minLenght: 3 }}
                placeholder="e.g. m, h, u">
                Short name
            </TextField>
            <SwitchField
                name="wholeValuesOnly"
            >
                Allow whole values only
            </SwitchField>
        </PopupAjaxForm>
    )
}