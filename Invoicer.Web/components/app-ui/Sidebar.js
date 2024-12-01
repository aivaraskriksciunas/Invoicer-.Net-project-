"use client"

import {
    Box,
    Flex,
    NavItem,
    Text,
} from '@chakra-ui/react'

import { CloseButton } from '@/components/ui/close-button'
import {
    DrawerRoot,
    DrawerContent,
} from '@/components/ui/drawer'

import SidebarContent from './SidebarContent'


export default function Sidebar() {

    return (
        <>
            <SidebarContent display={{ base: 'none', md: 'block' }} />
            <DrawerRoot
                placement="left"
                returnFocusOnClose={false}
                size="full">
                <DrawerContent>
                    <SidebarContent />
                </DrawerContent>
            </DrawerRoot>
            {/* mobilenav */}
        </>
    )
}

