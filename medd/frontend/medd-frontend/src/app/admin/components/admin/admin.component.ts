import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MedicineListComponent } from '../medicine-list/medicine-list.component';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, MedicineListComponent],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent {}
