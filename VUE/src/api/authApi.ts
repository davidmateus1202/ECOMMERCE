import { httpClient } from "../service/httpClient";

export interface AuthResponse {
    id: number;
    username: string;
    passwordHash: string;
    accessToken: string;
    acceToken?: string;
    expiresIn: number;
    role: string;
    createdAt: string;
    updatedAt: string;
}

export const getAuthTokenFromResponse = (response: AuthResponse) => {
    return response.accessToken || response.acceToken || '';
}

export const login = async (
    username : string,
    password : string
) => {
    const response = await httpClient.post<AuthResponse>(`auth/login`,{ username, password });
    return response.data;
}

export const forgotPassword = async (
    email: string
) => {
    const response = await httpClient.post(`Auth/forgot-password`, { email });
    return response;
}

export const resetPassword = async (
    token: string,
    newPassword: string
) => {
    const response = await httpClient.post(`auth/reset-password`, { token, newPassword });
    return response;
}