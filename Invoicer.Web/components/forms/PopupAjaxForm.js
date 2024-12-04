'use client'

import BaseForm from './BaseForm';
import {
    DialogActionTrigger,
    DialogBody,
    DialogCloseTrigger,
    DialogContent,
    DialogFooter,
    DialogHeader,
    DialogRoot,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import { Button } from '@/components/ui/button'

export default function PopupAjaxForm( {
    trigger,
    triggerButtonText,
    onSuccess,
    children,
    ...props
} ) {

    const displayTriggerButton = () => {
        if ( trigger ) {
            return trigger 
        }

        triggerButtonText = triggerButtonText ?? 'Open form'
        return (
            <Button variant="outline">{triggerButtonText}</Button>
        )
    }

    const onFormSuccess = (data) => {
        if ( onSuccess ) {
            onSuccess( data )
        }
    }

    return (
        <DialogRoot>
            <DialogTrigger asChild>
                {displayTriggerButton()}
            </DialogTrigger>
            <DialogContent>
                <BaseForm
                    {...props}
                    onSuccess={onFormSuccess}
                >
                    {isLoading => (
                        <>
                            <DialogHeader>
                                <DialogTitle>New billable record</DialogTitle>
                            </DialogHeader>
                            <DialogBody>
                                {children}
                            </DialogBody>
                            <DialogFooter>
                                <DialogActionTrigger asChild>
                                    <Button variant="outline">Cancel</Button>
                                </DialogActionTrigger>
                                <Button loading={isLoading} type="submit">
                                    Save
                                </Button>
                            </DialogFooter>
                            <DialogCloseTrigger />
                        </>
                    )}
                </BaseForm>
            </DialogContent>
        </DialogRoot>
    )

}