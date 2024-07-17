export interface GenericResponse<T> {
    statusCode: number;
    description: string;
    data: T;
    timestamp: string;
}