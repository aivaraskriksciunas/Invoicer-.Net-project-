import UserProviderWrapper from "../../components/wrappers/UserProviderWrapper";
import { Container, Box, Flex } from '@chakra-ui/react'
import styles from './layout.module.css'
import Sidebar from '@/components/app-ui/Sidebar'

export default function AppLayout( { children } ) {
    return (
        <UserProviderWrapper>
            <Box background="blue.800" color="white" px="3" py="3">
                Navbar    
            </Box>

            <Flex minH="100vh">
                <Sidebar/>

                <Box p="5" background="gray.50" width="100%">
                    { children }
                </Box>
            </Flex>
        </UserProviderWrapper>
    );
}