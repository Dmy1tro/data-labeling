import { Injectable } from "@angular/core";
import { FormGroup } from "@angular/forms";

@Injectable()
export class FormValidationService {
    hasErrors(form: FormGroup, controlName: string): boolean {
        const control = form.get(`${controlName}`)
        return control.invalid && (control.touched || control.dirty)
    }

    getErrorMessage(form: FormGroup, controlName: string, field = 'This field'): string {
        if (!this.hasErrors(form, controlName)) return null

        const formControl = form.get(controlName)

        if (formControl.hasError('required')) {
            return `${field} is required`;
        }

        if (formControl.hasError('minlength')) {
            return `Min. number of characters for this field is equal to ${formControl.getError('minlength').requiredLength}`
        }

        if (formControl.hasError('maxlength')) {
            return `Max. number of characters for this field is equal to ${formControl.getError('maxlength').requiredLength}`;
        }

        if (formControl.hasError('invalidEmail')) {
            return `Please enter correct email address.`;
        }

        if (formControl.hasError('invalidDate')) {
            return `Please choose date greater than today.`
        }

        if (formControl.hasError('invalidBankCardNumber')) {
            return `Required number of characters for this field is equal to 16.`
        }

        if (formControl.hasError('invalidWithdrawableAmount')) {
            return `Please enter withdrawable amount that less than or equal your balance`
        }

        if (formControl.hasError('zeroWithdrawableAmount')) {
            return `Please enter withdrawable amount that bigger than zero`
        }
    }
}