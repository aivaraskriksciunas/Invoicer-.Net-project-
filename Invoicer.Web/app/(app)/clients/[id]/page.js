"use client"

import ApiDataLoader from "@/components/wrappers/ApiDataLoader"
import { useState } from "react"
import Link from 'next/link'
import { useRouter, useParams } from "next/navigation"
import { Button, Heading } from '@chakra-ui/react'
import ContentBox from '@/components/app-ui/ContentBox'
import BillableRecordList from './BillableRecordList'
import CreateBillableRecord from './CreateBillableRecord'

export default function ClientDetail() {

    const params = useParams()
    const [client, setClient] = useState( null );

    return (
        <ApiDataLoader
            url={`/Api/Client/${params.id}`}
            onLoad={data => setClient( data )}>

            <ContentBox>
                <Heading>{client?.name}</Heading>

                <Link href={`/clients/${params.id}/edit`}>
                    <Button>Edit</Button>
                </Link>
            </ContentBox>

            <ContentBox>
                <Heading size="sm">Records</Heading>
                <CreateBillableRecord clientId={params.id}></CreateBillableRecord>
                <BillableRecordList clientId={params.id} />
            </ContentBox>

        </ApiDataLoader>
    )
}