import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MedicineCardsComponent } from '../medicine-cards/medicine-cards.component';
import { CartComponent } from '../cart/cart.component';
import { OrderService } from '../../../shared/services/order.service';
import { Medicine } from '../../../shared/models/medicine.model';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-store',
  standalone: true,
  imports: [CommonModule, MedicineCardsComponent, CartComponent],
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.scss']
})
export class StoreComponent implements OnInit {
  medicines: Medicine[] = [];
  cartItems: Medicine[] = [];
  showCart = false;
  isLoading = true;

  showOrderModal = false;
  orderDetails: any = null;
  isProcessing = false;

  constructor(
    private orderService: OrderService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadMedicines();
  }

  private async loadMedicines() {
    this.isLoading = true;
    try {
      const data = await lastValueFrom(this.orderService.getMedicines());
      this.medicines = [...(data || [])];
    } catch (err) {
      console.error('Error loading medicines:', err);
    } finally {
      this.isLoading = false;
      this.cdr.detectChanges();
    }
  }

  addToCart(medicine: Medicine) {
    const exists = this.cartItems.some(item => item.id === medicine.id);
    if (!exists) {
      this.cartItems.push(medicine);
    }
    this.showCart = true;
    this.cdr.detectChanges();
  }

  removeFromCart(medicineId: number) {
    this.cartItems = this.cartItems.filter(item => item.id !== medicineId);
    this.cdr.detectChanges();
  }

  async checkout() {
    if (this.cartItems.length === 0) {
      alert('Корзина пуста');
      return;
    }


    const items = this.cartItems.map(item => ({ medicineId: item.id! }));

    this.isProcessing = true;
    this.cdr.detectChanges();

    try {
      const response = await lastValueFrom(this.orderService.createOrder({ items }));
      const order = await lastValueFrom(this.orderService.getOrder(response.orderId));

      this.orderDetails = order;
      this.showOrderModal = true;
      this.cdr.detectChanges();

    } catch (err) {
      console.error('Checkout error:', err);
      alert('Ошибка при оформлении заказа');
    } finally {
      this.isProcessing = false;
      this.cdr.detectChanges();
    }
  }

  async payOrder() {
    if (!this.orderDetails) return;

    this.isProcessing = true;
    this.cdr.detectChanges();

    try {
      await lastValueFrom(this.orderService.updateOrderStatus(this.orderDetails.id, 1));
      this.orderDetails.status = 1;
      this.cdr.detectChanges();

      alert('Заказ успешно оплачен!');
      this.cartItems = [];
      this.showCart = false;
      this.cdr.detectChanges();

    } catch (err) {
      console.error('Payment error:', err);
      alert('Ошибка при оплате');
    } finally {
      this.isProcessing = false;
      this.cdr.detectChanges();
    }
  }

  closeOrderModal() {
    this.showOrderModal = false;
    this.orderDetails = null;
    this.cdr.detectChanges();
  }

  get totalPrice(): number {
    return this.cartItems.reduce((sum, item) => sum + item.price, 0);
  }
}
