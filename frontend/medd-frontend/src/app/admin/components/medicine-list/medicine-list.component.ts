import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { lastValueFrom } from 'rxjs';
import { CatalogService } from '../../../shared/services/catalog.service';
import { Medicine } from '../../../shared/models/medicine.model';

@Component({
  selector: 'app-medicine-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './medicine-list.component.html',
  styleUrls: ['./medicine-list.component.scss']
})
export class MedicineListComponent implements OnInit {
  medicines: Medicine[] = [];
  isLoading = false;

  showForm = false;
  isEditMode = false;
  editId: number | undefined = 0;
  formData: Medicine = {
    name: '',
    description: '',
    manufacturer: '',
    price: 0,
    isRequiredPrescription: false
  };

  constructor(
    private catalogService: CatalogService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadMedicines();
  }

  private async loadMedicines() {
    this.isLoading = true;
    try {
      const data = await lastValueFrom(this.catalogService.getMedicines());
      this.medicines = data || [];
    } catch (err) {
      console.error('Error:', err);
    } finally {
      this.isLoading = false;
      this.cdr.detectChanges();
    }
  }

  openAddForm() {
    this.showForm = true;
    this.isEditMode = false;
    this.editId = 0;
    this.formData = {
      name: '',
      description: '',
      manufacturer: '',
      price: 0,
      isRequiredPrescription: false
    };
    this.cdr.detectChanges();
  }

  openEditForm(medicine: Medicine) {
    this.showForm = true;
    this.isEditMode = true;
    this.editId = medicine.id;
    this.formData = { ...medicine };
    this.cdr.detectChanges();
  }

  closeForm() {
    this.showForm = false;
    this.cdr.detectChanges();
  }

  async saveMedicine() {
    if (!this.formData.name || !this.formData.manufacturer || this.formData.price <= 0) {
      alert('Заполните обязательные поля');
      return;
    }

    try {
      if (this.isEditMode && this.editId) {
        await lastValueFrom(this.catalogService.updateMedicine(this.editId, this.formData));
      } else {
        await lastValueFrom(this.catalogService.createMedicine(this.formData));
      }
      await this.loadMedicines();
      this.closeForm();
    } catch (err: any) {
      console.error('Save error:', err);
      const errorMessage = err.error?.error || err.message || 'Ошибка при сохранении';
      alert(errorMessage);
    }
  }

  async deleteMedicine(id: number) {
    if (confirm('Удалить лекарство?')) {
      try {
        await lastValueFrom(this.catalogService.deleteMedicine(id));
        await this.loadMedicines();
      } catch (err) {
        console.error('Delete error:', err);
      }
    }
  }
}
