'use client'

import AjaxForm from "@/components/forms/AjaxForm";
import TextField from "@/components/forms/TextField";
import ContentBox from "@/components/app-ui/ContentBox";
import ApiDataLoader from '@/components/wrappers/ApiDataLoader'
import { useRouter, useParams } from 'next/navigation';
import { useState } from 'react'

export default function ClientEdit() {

    const router = useRouter()
    const params = useParams()
    const [client, setClient] = useState( null )

    return (
        <ApiDataLoader
            url={`/Api/Client/${params.id}`}
            onLoad={data => setClient( data )}>
            <ContentBox>
                <AjaxForm
                    action={`/Api/Client/${params.id}`}
                    method="put"
                    redirectOnSuccess="/clients"
                >
                    <TextField
                        name="Name"
                        rules={{ required: true }}
                        value={client?.name}>
                        Name:
                    </TextField>
                    <TextField
                        name="PhoneNumber"
                        value={client?.phoneNumber}>
                        Phone:
                    </TextField>
                    <TextField
                        name="AddressLine1"
                        value={client?.addressLine1}>
                        Address Line 1:
                    </TextField>
                    <TextField
                        name="AddressLine2"
                        value={client?.addressLine2}>
                        Address Line 2:
                    </TextField>
                </AjaxForm>
            </ContentBox>
        </ApiDataLoader>
    )
}