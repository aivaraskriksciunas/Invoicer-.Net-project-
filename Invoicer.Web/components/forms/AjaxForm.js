'use client'

import { useForm, SubmitHandler } from "react-hook-form"
import { createContext, useState } from 'react'
import { Button } from '@/components/ui/button'
import { useRouter } from 'next/navigation'
import backend from '@/backend'
import BaseForm from "./BaseForm"

export default function AjaxForm({
    children,
    ...props
}) {
    return (
        <BaseForm {...props}>
            {( isLoading ) => (
                <>
                    {children}

                    <div>
                        <Button loading={isLoading} type="submit">
                            Submit
                        </Button>
                    </div>
                </>
            )}
        </BaseForm>
    )

}