'use client'
import { Provider as ChakraProvider } from '@/components/ui/provider'
import { Theme } from "@chakra-ui/react"
import { Provider as ReduxProvider } from 'react-redux'
import reduxStore from '@/stores/store'

export default function Providers( { children } ) {
    return (
        <ChakraProvider>
            <Theme appearance="light">
                <ReduxProvider store={reduxStore}>
                    {children}
                </ReduxProvider>
            </Theme>
        </ChakraProvider>
    )
}