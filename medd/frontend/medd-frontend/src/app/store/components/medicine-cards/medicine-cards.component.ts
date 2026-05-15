import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Medicine } from '../../../shared/models/medicine.model';

@Component({
  selector: 'app-medicine-cards',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './medicine-cards.component.html',
  styleUrls: ['./medicine-cards.component.scss']
})
export class MedicineCardsComponent {
  @Input() medicines: Medicine[] = [];
  @Output() addToCart = new EventEmitter<Medicine>();
}
