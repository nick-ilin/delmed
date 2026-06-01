import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Medicine } from '../../../shared/models/medicine.model';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent {
  @Input() items: Medicine[] = [];  // ← просто массив
  @Input() isProcessing = false;
  @Output() removeFromCart = new EventEmitter<number>();
  @Output() checkout = new EventEmitter<void>();

  get totalPrice(): number {
    return this.items.reduce((sum, item) => sum + item.price, 0);
  }

  remove(itemId: number) {
    this.removeFromCart.emit(itemId);
  }

  onCheckout() {
    if (!this.isProcessing && this.items.length > 0) {
      this.checkout.emit();
    }
  }
}
