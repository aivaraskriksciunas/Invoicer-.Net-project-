"use client"

import ApiDataLoader from "@/components/wrappers/ApiDataLoader"
import { useState } from "react"
import { Table } from '@chakra-ui/react'
import Link from 'next/link'
import CreateBillableUnitForm from './CreateBillableUnitForm'

export default function BillableUnitsIndex() {

    const [units, setUnits] = useState( [] )
    const [key, setKey] = useState(0)

    return (
        <ApiDataLoader url="/Api/BillableUnit" onLoad={data => setUnits( data )} key={key}>
            <CreateBillableUnitForm onSuccess={() => setKey(key + 1)} />
            <Table.Root>
                <Table.Header>
                    <Table.Row>
                        <Table.ColumnHeader>Unit</Table.ColumnHeader>
                        <Table.ColumnHeader>Whole values only</Table.ColumnHeader>
                    </Table.Row>
                </Table.Header>
                <Table.Body>
                    {units.map( unit => (
                        <Table.Row key={unit.id}>
                            <Table.Cell>
                                <Link href={`/units/${unit.id}`}>
                                    {unit.fullName} ({unit.shortName})
                                </Link>
                            </Table.Cell>
                            <Table.Cell>
                                {unit.wholeValuesOnly ? 'Yes' : 'No' }
                            </Table.Cell>
                        </Table.Row>
                    ) )}
                </Table.Body>
            </Table.Root>
        </ApiDataLoader>
    )
}