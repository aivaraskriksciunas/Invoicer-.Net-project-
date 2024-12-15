import { Bleed, Flex, Heading } from '@chakra-ui/react'

export function ContentBoxHeading( {
    actions = null,
    children
} ) {

    return (
        <Bleed mb="4" borderBottomWidth="1px" inline="4" pb="4" px="4">
            <Flex justify="space-between" alignItems="center">
                <Heading>
                    {children}
                </Heading>
                { actions ? actions : null }
            </Flex>
        </Bleed>
    )
}