"use client"

import {
    Drawer,
    DrawerContent,
    Box,
    Flex,
    NavItem,
    Text,
    CloseButton,
} from '@chakra-ui/react'

import SidebarContent from './SidebarContent'


export default function Sidebar() {

    return (
        <>
            <SidebarContent display={{ base: 'none', md: 'block' }} />
            <Drawer
                placement="left"
                returnFocusOnClose={false}
                size="full">
                <DrawerContent>
                    <SidebarContent />
                </DrawerContent>
            </Drawer>
            {/* mobilenav */}
        </>
    )
}

