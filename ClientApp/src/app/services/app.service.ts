import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {sleep} from '@/utils/helpers';

import {IdTokenResult, createUserWithEmailAndPassword} from '@firebase/auth';
import {
    User,
    onAuthStateChanged,
    signInWithEmailAndPassword,
    signInWithPopup
} from 'firebase/auth';
import {GoogleAuthProvider} from 'firebase/auth';
import {firebaseAuth} from '@/firebase';
import {HttpClient} from '@angular/common/http';
import {GenericResponse} from '@/models/generic-response';
import {
    LoginReq,
    LoginRes,
    RegisterReq,
    RegisterRes,
    UserData
} from '@/models/auth-model';
import {environment} from 'environments/environment';

const provider = new GoogleAuthProvider();

const fallbackUserData: User = {
    uid: '15c89b06-5ec2-437d-81ca-fc70f5df331e',
    displayName: 'Guest User',
    email: 'guest@example.com',
    photoURL: 'https://example.com/avatar.png',
    emailVerified: true,
    isAnonymous: false,
    metadata: undefined,
    providerData: [],
    refreshToken: '',
    tenantId: '',
    delete: function (): Promise<void> {
        throw new Error('Function not implemented.');
    },
    getIdToken: function (forceRefresh?: boolean): Promise<string> {
        throw new Error('Function not implemented.');
    },
    getIdTokenResult: function (
        forceRefresh?: boolean
    ): Promise<IdTokenResult> {
        throw new Error('Function not implemented.');
    },
    reload: function (): Promise<void> {
        throw new Error('Function not implemented.');
    },
    toJSON: function (): object {
        throw new Error('Function not implemented.');
    },
    phoneNumber: '0808',
    providerId: ''
};

@Injectable({
    providedIn: 'root'
})
export class AppService {
    public user?: User | null = null;
    public userData?: UserData | null = null;
    private apiUrl = environment.API_AUTH;

    constructor(
        private router: Router,
        private toastr: ToastrService,
        private http: HttpClient
    ) {
        // onAuthStateChanged(
        //     firebaseAuth,
        //     (user) => {
        //         if (user) {
        //             this.user = user;
        //         } else {
        //             this.user = fallbackUserData;
        //         }
        //     },
        //     (e) => {
        //         this.user = fallbackUserData;
        //     }
        // );
    }

    async registerWithEmail(
        username: string,
        email: string,
        password: string,
        identitas: string,
        phoneNumber: string
    ) {
        try {
            // const result = await createUserWithEmailAndPassword(
            //     firebaseAuth,
            //     email,
            //     password
            // );
            console.log('Registering user.... ');
            const registerReq: RegisterReq = {
                username,
                email,
                password,
                identitas,
                phoneNumber
            };
            return await this.http
                .post<
                    GenericResponse<RegisterRes>
                >(`${this.apiUrl}/register`, registerReq)
                .subscribe(
                    (response) => {
                        console.log('Register successful', response);
                        this.userData = {token: response.data?.token};
                        localStorage.setItem('token', this.userData?.token);
                        this.toastr.success('Register success');
                        console.log('userData :', response);
                        this.router.navigate(['/admin']);
                    },
                    (error) => {
                        console.error(
                            'Error Registering user',
                            error?.error?.description
                        );
                        this.toastr.error(
                            'Error Registering user',
                            error?.error?.description
                        );
                    }
                );
        } catch (error) {
            this.toastr.error(error.message);
        }
    }

    async loginWithEmail(email: string, password: string) {
        try {
            // const result = await signInWithEmailAndPassword(
            //     firebaseAuth,
            //     email,
            //     password
            // );
            console.log('Logging in.... ');
            const loginReq: LoginReq = {email, password};
            return await this.http
                .post<
                    GenericResponse<LoginRes>
                >(`${this.apiUrl}/login`, loginReq)
                .subscribe(
                    (response) => {
                        console.log('Login successful', response);
                        this.toastr.success('Login success');
                        this.userData = response.data;
                        localStorage.setItem('token', this.userData.token);
                        console.log('userData :', response);
                        this.router.navigate(['/admin']);
                    },
                    (error) => {
                        console.error(
                            'Error logging in',
                            error?.error?.description
                        );
                        this.toastr.error(
                            'Error logging in',
                            error?.error?.description
                        );
                    }
                );
        } catch (error) {
            this.toastr.error(error.message);
        }
    }

    async signInByGoogle() {
        try {
            const result = await signInWithPopup(firebaseAuth, provider);
            this.user = result.user;
            this.router.navigate(['/']);

            return result;
        } catch (error) {
            this.toastr.error(error.message);
        }
    }

    async getProfile() {
        try {
            await sleep(500);
            // todo: refresh token=

            this.userData = {token: localStorage.getItem('token')};
            if (this.userData.token) {
                // this.user = user;
            } else {
                this.logout();
            }
        } catch (error) {
            this.logout();
            this.toastr.error(error.message);
        }
    }

    async logout() {
        // await firebaseAuth.signOut();
        localStorage.removeItem('token');
        this.userData = null;
        this.router.navigate(['/login']);
    }
}
