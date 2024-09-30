import AjaxForm from "@/components/forms/AjaxForm"
import TextField from '@/components/forms/TextField'

export default function Page() {
    return (
        <>
            Login form
            <AjaxForm action="/login" method='post'>
                <TextField name="email">Email</TextField>
                <TextField
                    name="password"
                    type="password"
                    rules={{ minLength: 5 } }
                >Password</TextField>
            </AjaxForm>
        </>
    )
}