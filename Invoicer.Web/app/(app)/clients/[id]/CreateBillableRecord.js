'use client'
import { useState } from 'react'
import { Button, Flex } from '@chakra-ui/react'
import PopupAjaxForm from '@/components/forms/PopupAjaxForm'
import TextField from '@/components/forms/TextField'
import DateField from '@/components/forms/DateField'
import { AsyncSelectField } from '@/components/forms/AsyncSelectField'

export default function CreateBillableRecord( { clientId, onSuccess = () => { } } ) {

    return (
        <PopupAjaxForm
            action={`/Api/Client/${clientId}/BillableRecord`}
            method="post"
            onSuccess={onSuccess}
            trigger={<Button variant="outline">New record</Button>}>
            <TextField
                name="name"
                rules={{ required: true, minLenght: 3 }}>
                Title
            </TextField>
            <DateField
                name="startTime"
                rules={{ required: true }}
                showTimeInput={true }
            >
                Start date
            </DateField>
            <DateField
                name="endTime"
                showTimeInput
            >
                End date
            </DateField>

            <Flex gap="2">
                <TextField
                    name="amount"
                    rules={{ required: true }}
                >
                    Amount
                </TextField>

                <AsyncSelectField
                    name="billableUnitId"
                    url="/Api/BillableUnit"
                    optionLabelKey='fullName'
                    optionValueKey='id'
                    >
                    Unit
                </AsyncSelectField>
            </Flex>
        </PopupAjaxForm>
    )

}