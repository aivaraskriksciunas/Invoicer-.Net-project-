'use client'

import AjaxForm from "@/components/forms/AjaxForm";
import TextField from "@/components/forms/TextField";
import ContentBox from "@/components/app-ui/ContentBox";
import { useRouter } from 'next/navigation';

export default function ClientCreate() {

    const router = useRouter()

    return (
        <ContentBox>
            <AjaxForm
                action="/Api/Client"
                method="post"
                redirectOnSuccess="/clients"
            >
                <TextField name="Name">Name:</TextField>
                <TextField name="PhoneNumber">Phone:</TextField>
                <TextField name="AddressLine1">Address Line 1:</TextField>
                <TextField name="AddressLine2">Address Line 2:</TextField>
            </AjaxForm>
        </ContentBox>
    )
}