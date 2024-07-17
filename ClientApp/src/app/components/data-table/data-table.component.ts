import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ProductService} from '../../services/product.service';
import {ToastrService} from 'ngx-toastr';
import {Product} from '@/models/product';
import {HttpParams} from '@angular/common/http';

@Component({
    selector: 'app-data-table',
    templateUrl: './data-table.component.html',
    styleUrls: ['./data-table.component.scss']
})
export class DataTableComponent implements OnInit {
    products: Product[] = [];
    filteredProducts: Product[] = [];
    searchTerm: string = '';
    searchProductName: string = '';
    searchUnits: string = '';
    searchPrice: string = '';
    sortField: string = '';
    sortOrder: boolean = true; // true for ascending, false for descending
    currentPage: number = 1;
    itemsPerPage: number = 10;
    totalItems: number = 0;

    editProduct: Product | null = null;

    @Output() openModalEdit = new EventEmitter<Product>();

    constructor(
        private productService: ProductService,
        private toastr: ToastrService
    ) {}

    ngOnInit(): void {
        this.refreshTable();
    }

    refreshTable(): void {
        let params = new HttpParams()
            .append('productName', this.searchProductName)
            .append('units', this.searchUnits)
            .set('price', this.searchPrice);
        this.productService.browseProducts(params).subscribe((data) => {
            this.products = data;
            this.totalItems = data.length;
            this.applyFilters();
        });
    }

    onSearchChange(): void {
        this.applyFilters();
    }

    onSearchColumn(): void {
        // const inputElement = event.target as HTMLInputElement;
        // const value = inputElement.value;
        this.browse();
    }

    onSort(field: string): void {
        if (this.sortField === field) {
            this.sortOrder = !this.sortOrder;
        } else {
            this.sortField = field;
            this.sortOrder = true;
        }
        this.applyFilters();
    }

    onPageChange(page: number): void {
        this.currentPage = page;
        this.applyFilters();
    }

    applyFilters(): void {
        let filtered = this.products;

        if (this.searchTerm) {
            filtered = filtered.filter((product) =>
                product.productName
                    .toLowerCase()
                    .includes(this.searchTerm.toLowerCase())
            );
        }

        if (this.sortField) {
            filtered.sort((a, b) => {
                const aValue = a[this.sortField];
                const bValue = b[this.sortField];
                if (aValue < bValue) return this.sortOrder ? -1 : 1;
                if (aValue > bValue) return this.sortOrder ? 1 : -1;
                return 0;
            });
        }

        this.totalItems = filtered.length;
        const startIndex = (this.currentPage - 1) * this.itemsPerPage;
        this.filteredProducts = filtered.slice(
            startIndex,
            startIndex + this.itemsPerPage
        );
    }

    browse(): void {
        let params = new HttpParams()
            .append('productName', this.searchProductName)
            .append('units', this.searchUnits)
            .set('price', this.searchPrice);
        this.productService.browseProducts(params).subscribe((data) => {
            this.filteredProducts = data;
        });
    }

    getTotalPages(): number {
        return Math.ceil(this.totalItems / this.itemsPerPage);
    }

    onEdit(product: Product) {
        this.openModalEdit.emit(product);
        // Logic to show the edit form modal
    }

    onDelete(productId: number) {
        this.productService.deleteProduct(productId).subscribe((data) => {
            console.log(data);
            if (data.statusCode == 200) {
                this.refreshTable();
                this.toastr.success(
                    'Deleted',
                    `Data ${data.data?.productName ?? ''} deleted`
                );
            } else {
                console.log('error');
                this.toastr.error('Failed to delet', data.description);
            }
        });
    }
}
