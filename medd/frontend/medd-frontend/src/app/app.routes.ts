import { Routes } from '@angular/router';
import { AdminComponent } from './admin/components/admin/admin.component';
import { StoreComponent } from './store/components/store/store.component';

export const routes: Routes = [
  { path: 'admin', component: AdminComponent },
  { path: 'store', component: StoreComponent },
  { path: '', redirectTo: '/store', pathMatch: 'full' },
  { path: '**', redirectTo: '/store' }
];
