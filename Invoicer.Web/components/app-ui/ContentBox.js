import { Box } from '@chakra-ui/react'

export function ContentBox( {
    children
} ) {

    return (
        <Box bg="bg" p="4" mb="8" rounded="sm" borderWidth="1px">
            {children }
        </Box>
    )
}

export default ContentBox