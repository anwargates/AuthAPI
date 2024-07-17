import {Product} from '@/models/product';
import {Component, ViewChild} from '@angular/core';
import {DataTableComponent} from '@components/data-table/data-table.component';
import {ModalFormProductsComponent} from '@components/modal-form-products/modal-form-products.component';
import {ProductService} from '@services/product.service';
import {Modal} from 'bootstrap';

@Component({
    selector: 'app-blank',
    templateUrl: './blank.component.html',
    styleUrls: ['./blank.component.scss']
})
export class BlankComponent {
    modalInstance: Modal | null = null;
    @ViewChild(DataTableComponent) dataTableComponent!: DataTableComponent;
    @ViewChild(ModalFormProductsComponent)
    modalForm!: ModalFormProductsComponent;

    constructor(private productService: ProductService) {}

    openModal() {
        const modalElement = document.getElementById('productModal');
        if (modalElement) {
            this.modalInstance = new Modal(modalElement);
            this.modalInstance.show();
        }
    }

    openModalEdit(product: Product) {
        console.log('modal opening');
        const modalElement = document.getElementById('productModal');
        if (modalElement) {
            this.modalInstance = new Modal(modalElement);
            this.modalForm.productForm.setValue(product);
            this.modalInstance.show();
        }
    }

    doRefreshTable() {
        this.dataTableComponent.refreshTable();
    }
}
