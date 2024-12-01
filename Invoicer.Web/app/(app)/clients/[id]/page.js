"use client"

import ApiDataLoader from "@/components/wrappers/ApiDataLoader"
import { useState } from "react"
import Link from 'next/link'
import { useRouter, useParams } from "next/navigation"
import { Button } from '@chakra-ui/react'

export default function ClientDetail() {

    const params = useParams()
    const [client, setClient] = useState( null );

    return (
        <ApiDataLoader
            url={`/Api/Client/${params.id}`}
            onLoad={data => setClient( data )}>

            <div>You are viewing client "{client?.name}"</div>

            <Link href={`/clients/${params.id}/edit`}>
                <Button>Edit</Button>
            </Link>
        </ApiDataLoader>
    )
}