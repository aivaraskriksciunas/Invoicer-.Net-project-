import styles from './layout.css'
import {
    Box
} from '@chakra-ui/react'

export default function AuthLayout( { children } ) {
    return (
        <Box bgColor="teal.700" p="20px" minHeight="100vh">
            <Box bgColor="bg" p="20px">
                { children }
            </Box>
        </Box>
    )
}