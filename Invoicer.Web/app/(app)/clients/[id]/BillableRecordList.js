'use client'

import { Table } from '@chakra-ui/react'
import ApiDataLoader from '@/components/wrappers/ApiDataLoader'
import { useState } from 'react'

export default function BillableRecordList({ clientId }) {

    const [records, setRecords] = useState( [] )

    const displayTable = ( records ) => {
        if ( !records ) {
            return (
                <div>No records found.</div>
            )
        }

        return (
            <Table.Root>
                <Table.Header>
                    <Table.Row>
                        <Table.ColumnHeader>#</Table.ColumnHeader>
                        <Table.ColumnHeader>Name</Table.ColumnHeader>
                        <Table.ColumnHeader>Date</Table.ColumnHeader>
                    </Table.Row>
                </Table.Header>
                <Table.Body>
                    {records.map( (record, idx) => (
                        <Table.Row key={record.name}>
                            <Table.Cell>{idx + 1}</Table.Cell>
                            <Table.Cell>{record.name}</Table.Cell>
                            <Table.Cell>{record.startTime}</Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>
            </Table.Root>
        )
    }

    return (
        <ApiDataLoader
            url={`/Api/Client/${clientId}/BillableRecord`}
            onLoad={ data => setRecords( data ) }>
            {displayTable( records )}
        </ApiDataLoader>
    )

}