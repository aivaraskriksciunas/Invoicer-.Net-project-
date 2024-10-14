'use client'
import { ChakraProvider } from '@chakra-ui/react'
import { Provider as ReduxProvider } from 'react-redux'
import reduxStore from '@/stores/store'

export default function Providers( { children } ) {
    return (
        <ChakraProvider>
            <ReduxProvider store={reduxStore}>
                {children}
            </ReduxProvider>
        </ChakraProvider>
    )
}