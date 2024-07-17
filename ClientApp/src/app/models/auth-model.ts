export interface LoginRes {
    // id: number;
    // username: string;
    // email: string;
    // password: string;
    // createdOn: string;
    token: string;
}

export interface LoginReq {
    email: string;
    password: string;
}

export interface RegisterReq {
    username: string;
    email: string;
    password: string;
    identitas: string;
    phoneNumber: string;
}

export interface RegisterRes {
    id: number;
    username: string;
    email: string;
    password: string;
    identitas: string;
    phoneNumber: string;
    createdOn: string;
    lastLoggedIn: string;
    token: string;
}

export interface UserData {
    token: string;
    // id: number;
    // username: string;
    // email: string;
    // password: string;
    // createdOn: string;
}
