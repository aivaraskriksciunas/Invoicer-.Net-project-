"use client"

import ApiDataLoader from "@/components/wrappers/ApiDataLoader"
import { useState } from "react"

export default function ClientsIndex() {

    const [clients, setClients] = useState([])

    return (
        <ApiDataLoader url="/Api/Client" onLoad={data => setClients( data )}>
            {clients.map( client => (
                <div>{client.name}</div>
            )) }
        </ApiDataLoader>
    )
}