'use client'
import { Provider as ChakraProvider } from '@/components/ui/provider'
import { Provider as ReduxProvider } from 'react-redux'
import reduxStore from '@/stores/store'

export default function Providers( { children } ) {
    return (
        <ChakraProvider enableSystem={false} defaultTheme="light">
            <ReduxProvider store={reduxStore}>
                {children}
            </ReduxProvider>
        </ChakraProvider>
    )
}