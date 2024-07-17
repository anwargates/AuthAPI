import {Product, ProductAddDto} from '@/models/product';
import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ProductService} from '@services/product.service';
import {Modal} from 'bootstrap';
import {ToastrService} from 'ngx-toastr';

@Component({
    selector: 'app-modal-form-products',
    templateUrl: './modal-form-products.component.html',
    styleUrls: ['./modal-form-products.component.scss']
})
export class ModalFormProductsComponent {
    productForm: FormGroup;
    // modalInstance: Modal | null = null;
    @Input() modalInstance: Modal | null = null;
    @Output() refreshTable = new EventEmitter<string>();

    constructor(
        private fb: FormBuilder,
        private productService: ProductService,
        private toastr: ToastrService
    ) {
        this.productForm = this.fb.group({
            id: null,
            productName: ['', Validators.required],
            units: ['', Validators.required],
            price: [0, Validators.required],
            createdOn: null
        });
    }

    closeModal() {
        console.log(this.modalInstance);
        this.doRefreshTable();
        if (this.modalInstance) {
            this.modalInstance.hide();
        }
    }

    onSubmit() {
        if (this.productForm.valid) {
            const productData: Product = this.productForm.value;

            if (productData?.id == null) {
                const {id, ...dtoWithoutId} = productData;
                console.log(dtoWithoutId);
                this.productService.addProduct(dtoWithoutId).subscribe(
                    (response) => {
                        console.log('Product added successfully:', response);
                        // Optionally reset the form after successful submission
                        this.productForm.reset();
                        this.toastr.success('Success Adding Data');
                        this.closeModal();
                    },
                    (error) => {
                        console.error('Error adding product:', error);
                        this.toastr.error('Error adding product:', error);
                    }
                );
            } else {
                this.productService.editProduct(productData).subscribe(
                    (response) => {
                        console.log('Product edited successfully:', response);
                        // Optionally reset the form after successful submission
                        this.productForm.reset();
                        this.toastr.success('Success Edit Data');
                        this.closeModal();
                    },
                    (error) => {
                        console.error('Error editing product:', error);
                        this.toastr.error('Error editing product:', error);
                    }
                );
            }
        } else {
            console.error('Form is invalid');
            this.toastr.error('Form is invalid');
        }
    }

    doRefreshTable(): void {
        this.refreshTable.emit();
    }
}
