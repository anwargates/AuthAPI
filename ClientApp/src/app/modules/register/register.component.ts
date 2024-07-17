import {
    Component,
    OnInit,
    Renderer2,
    OnDestroy,
    HostBinding
} from '@angular/core';
import {
    UntypedFormGroup,
    UntypedFormControl,
    Validators,
    AbstractControl
} from '@angular/forms';
import {AppService} from '@services/app.service';
import {ToastrService} from 'ngx-toastr';
import {Location} from '@angular/common';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
    @HostBinding('class') class = 'register-box';

    public registerForm: UntypedFormGroup;
    public isAuthLoading = false;
    public isGoogleLoading = false;
    public isFacebookLoading = false;

    constructor(
        private renderer: Renderer2,
        private toastr: ToastrService,
        private appService: AppService,
        public location: Location
    ) {}

    ngOnInit() {
        this.renderer.addClass(
            document.querySelector('app-root'),
            'register-page'
        );
        this.registerForm = new UntypedFormGroup(
            {
                identitas: new UntypedFormControl(null, [
                    Validators.required,
                    Validators.minLength(12)
                ]),
                phoneNumber: new UntypedFormControl(null, Validators.required),
                username: new UntypedFormControl(null, Validators.required),
                email: new UntypedFormControl(null, [
                    Validators.required,
                    Validators.email
                ]),
                password: new UntypedFormControl(null, [
                    Validators.required,
                    Validators.minLength(6)
                ]),
                retypePassword: new UntypedFormControl(null, [
                    Validators.required,
                    Validators.minLength(6)
                ])
            },
            {validators: this.passwordMatchValidator}
        );
    }

    passwordMatchValidator(
        control: AbstractControl
    ): {[key: string]: boolean} | null {
        const password = control.get('password');
        const confirmPassword = control.get('retypePassword');

        // console.log(password);
        // console.log(password);

        if (!password || !confirmPassword) {
            return null;
        }

        if (password.value !== confirmPassword.value) {
            return {passwordMismatch: true};
        }

        return null;
    }

    async registerByAuth() {
        if (this.registerForm.valid) {
            this.isAuthLoading = true;
            await this.appService.registerWithEmail(
                this.registerForm.value.username,
                this.registerForm.value.email,
                this.registerForm.value.password,
                this.registerForm.value.identitas,
                this.registerForm.value.phoneNumber
            );
            this.isAuthLoading = false;
        } else {
            console.log(this.registerForm);
            this.toastr.error('Form is not valid');
        }
    }

    async registerByGoogle() {
        this.isGoogleLoading = true;
        await this.appService.signInByGoogle();
        this.isGoogleLoading = false;
    }

    ngOnDestroy() {
        this.renderer.removeClass(
            document.querySelector('app-root'),
            'register-page'
        );
    }
}
