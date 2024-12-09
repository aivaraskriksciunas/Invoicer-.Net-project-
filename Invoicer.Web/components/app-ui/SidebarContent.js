"use client"

import {
    Drawer,
    DrawerContent,
    Box,
    Flex,
    Text,
} from '@chakra-ui/react'
import { CloseButton } from '@/components/ui/close-button'

import Link from 'next/link'

export default function SidebarContent() {
    return (
        <Box
            bg='white'
            borderRight="1px"
            borderRightColor='gray.200'
            w={{ base: 'full', md: 60 }}
            p="3"
            h="full">
            <CloseButton display={{ base: 'flex', md: 'none' }} />

            <Link href="/clients">
                <Box _hover={{ border: "1px" }} p="2">
                    Clients
                </Box>
            </Link>

            <Link href="/units">
                <Box _hover={{ border: "1px" }} p="2">
                    Billable units
                </Box>
            </Link>
        </Box>
    )
}