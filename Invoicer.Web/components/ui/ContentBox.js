import { Box } from '@chakra-ui/react'

export default function ContentBox( {
    children
} ) {

    return (
        <Box bg="white" p="4" rounded="sm" borderWidth="1px" borderColor="gray.100">
            {children }
        </Box>
    )
}