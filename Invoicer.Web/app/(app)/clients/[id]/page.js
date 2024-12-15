"use client"

import ApiDataLoader from "@/components/wrappers/ApiDataLoader"
import { useState } from "react"
import Link from 'next/link'
import { useRouter, useParams } from "next/navigation"
import { Button, Heading } from '@chakra-ui/react'
import ContentBox from '@/components/app-ui/ContentBox'
import BillableRecordList from './BillableRecordList'
import CreateBillableRecord from './CreateBillableRecord'
import { ContentBoxHeading } from "../../../../components/app-ui/ContentBoxHeading"

export default function ClientDetail() {

    const params = useParams()
    const [client, setClient] = useState( null );

    const getEditClientLink = () => (
        <Link href={`/clients/${params.id}/edit`}>
            <Button>Edit</Button>
        </Link>
    )

    const getAddRecordLink = () => (
        <CreateBillableRecord clientId={params.id}></CreateBillableRecord>
    )

    return (
        <ApiDataLoader
            url={`/Api/Client/${params.id}`}
            onLoad={data => setClient( data )}>

            <ContentBox>
                <ContentBoxHeading actions={getEditClientLink()}>
                    {client?.name}
                </ContentBoxHeading>          
            </ContentBox>

            <ContentBox>
                <ContentBoxHeading actions={getAddRecordLink()}>
                    Records
                </ContentBoxHeading>
                <BillableRecordList clientId={params.id} />
            </ContentBox>

        </ApiDataLoader>
    )
}