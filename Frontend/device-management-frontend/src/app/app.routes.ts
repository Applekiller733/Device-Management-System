import { Routes } from '@angular/router';
import { DeviceListComponent } from './pages/device-list/device-list.component';
import { DeviceDetailComponent } from './pages/device-detail/device-detail.component';
import { DeviceFormComponent } from './pages/device-form/device-form.component';
import { authGuard } from './guards/auth.guard';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: '', component: DeviceListComponent, canActivate: [authGuard] },
    { path: 'device-detail/:id', component: DeviceDetailComponent, canActivate: [authGuard] }, 
    { path: 'device-form', component: DeviceFormComponent, canActivate: [authGuard] },      
    { path: 'device-form/:id', component: DeviceFormComponent, canActivate: [authGuard] },
    { path: '**', redirectTo: '' },
];
