"use client"

import { useEffect, useState } from "react";
import backend from "../../backend";

export default function ApiDataLoader( {
    url,
    onLoad,
    onError,
    children,
} ) {

    const [isLoading, setIsLoading] = useState( true )

    async function fetchData() {
        setIsLoading( true )

        try {
            let result = await backend.get( url )
            onLoad( result.data )
        }
        catch ( e ) {
            if ( onError ) {
                onError( e.response )
            }
            else {
                console.log( e )
            }
        }
        finally {
            setIsLoading( false )
        }
    }

    useEffect( () => {
        fetchData()
    }, [] )

    if ( isLoading ) {
        return (
            <div>Loading...</div>
        )
    }

    return <>{ children }</>

}