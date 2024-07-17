import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';
import {Product} from '@/models/product';
import {GenericResponse} from '@/models/generic-response';
import {Count} from '@/models/count';
import {environment} from 'environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ProductService {
    private apiUrl = environment.API_PRODUCTS;
    private token = localStorage.getItem('token');
    private headers = new HttpHeaders().set(
        'Authorization',
        `Bearer ${this.token}`
    );

    constructor(private http: HttpClient) {}

    getAllProducts(): Observable<Product[]> {
        return this.http
            .get<
                GenericResponse<Product[]>
            >(this.apiUrl, {headers: this.headers})
            .pipe(map((response) => response.data));
    }

    getProductById(id: number): Observable<Product> {
        return this.http
            .get<
                GenericResponse<Product>
            >(`${this.apiUrl}/${id}`, {headers: this.headers})
            .pipe(map((response) => response.data));
    }

    countAllProducts(): Observable<Count> {
        return this.http
            .get<
                GenericResponse<Count>
            >(`${this.apiUrl}/count`, {headers: this.headers})
            .pipe(map((response) => response.data));
    }

    addProduct(productData: any): Observable<GenericResponse<Product>> {
        return this.http.post<GenericResponse<Product>>(
            `${this.apiUrl}`,
            productData,
            {headers: this.headers}
        );
    }

    deleteProduct(productId: number): Observable<GenericResponse<any>> {
        return this.http
            .delete<GenericResponse<any>>(`${this.apiUrl}/${productId}`)
            .pipe(map((response) => response));
    }

    editProduct(productData: any): Observable<GenericResponse<Product>> {
        return this.http.put<GenericResponse<Product>>(
            `${this.apiUrl}`,
            productData,
            {headers: this.headers}
        );
    }

    browseProducts(params: HttpParams): Observable<Product[]> {
        return this.http
            .get<
                GenericResponse<Product[]>
            >(`${this.apiUrl}/browse`, {headers: this.headers, params: params})
            .pipe(map((response) => response.data));
    }
}
