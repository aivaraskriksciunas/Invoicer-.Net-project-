"use client"

import ApiDataLoader from "@/components/wrappers/ApiDataLoader"
import { useState } from "react"
import Link from 'next/link'

export default function ClientsIndex() {

    const [clients, setClients] = useState([])

    return (
        <ApiDataLoader url="/Api/Client" onLoad={data => setClients( data )}>
            {clients.map( client => (
                <Link href={`/clients/${client.id}`} key={client.id}>
                    {client.name}
                </Link>
            )) }
        </ApiDataLoader>
    )
}