"use client"

import ApiDataLoader from "@/components/wrappers/ApiDataLoader"
import { useState } from "react"
import { Button, Table } from '@chakra-ui/react'
import Link from 'next/link'
import { ContentBox } from "@/components/app-ui/ContentBox"
import { ContentBoxHeading } from "@/components/app-ui/ContentBoxHeading"

export default function ClientsIndex() {

    const [clients, setClients] = useState( [] )

    const getCreateLink = () => {
        return (
            <Link href="/clients/create">
                <Button>Create</Button>
            </Link>
        )
    }

    return (
        <ContentBox>
            <ContentBoxHeading
                actions={getCreateLink()}>
                Clients
            </ContentBoxHeading>
            <ApiDataLoader url="/Api/Client" onLoad={data => setClients( data )}>
                <Table.Root>
                    <Table.Header>
                        <Table.Row>
                            <Table.ColumnHeader>Client</Table.ColumnHeader>
                        </Table.Row>
                    </Table.Header>
                    <Table.Body>
                        {clients.map( client => (
                            <Table.Row key={client.id}>
                                <Table.Cell>
                                    <Link href={`/clients/${client.id}`}>
                                        {client.name}
                                    </Link>
                                </Table.Cell>
                            </Table.Row>
                        ) )}
                    </Table.Body>
                </Table.Root>
            </ApiDataLoader>
        </ContentBox>
       
    )
}