'use client'
import { useState } from 'react'
import { Button } from '@chakra-ui/react'
import PopupAjaxForm from '@/components/forms/PopupAjaxForm'
import TextField from '@/components/forms/TextField'

export default function CreateBillableRecord( { clientId } ) {

    return (
        <PopupAjaxForm
            action={`/Api/Client/${clientId}/BillableRecord`}
            method="post"
            trigger={<Button variant="outline">New record</Button>}>
            <TextField
                name="name"
                rules={{ required: true, minLenght: 3 }}>
                Title
            </TextField>
            <TextField
                name="startTime"
                rules={{ required: true }}>
                Start date
            </TextField>
            <TextField
                name="endTime">
                End date
            </TextField>
        </PopupAjaxForm>
    )

}