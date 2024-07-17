// dashboard.component.ts
import {Component, OnInit} from '@angular/core';
import {
    faBookmark,
    faEnvelope,
    faChartSimple,
    faBox,
    faUserPlus,
    faChartPie
} from '@fortawesome/free-solid-svg-icons';
import {ProductService} from '@services/product.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    faBookmark = faBookmark;
    faEnvelope = faEnvelope;
    faChartSimple = faChartSimple;
    faBox = faBox;
    faUserPlus = faUserPlus;
    faChartPie = faChartPie;

    totalData = 0;
    isLoading = true;

    constructor(private productService: ProductService) {}

    ngOnInit(): void {
        this.productService.countAllProducts().subscribe({
            next: (data) => {
                this.totalData = data.totalData;
                this.isLoading = false; // Set loading flag to false after data is loaded
            },
            error: (error) => {
                console.error('Error fetching product count', error);
                this.isLoading = false; // Ensure loading flag is reset even if there is an error
            }
        });
    }
}
