import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter, withHashLocation } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app/app.component';
import { LoginComponent } from './app/components/login/login.component';
import { RegisterComponent } from './app/components/register/register.component';
import { Routes } from '@angular/router'; 
import { MainPageComponent } from './app/components/main-page/main-page.component';
import { AuthGuard } from './app/services/auth-guard.service';
import { OfferDetailsComponent } from './app/components/offer-details/offer-details.component';
import { BookingsComponent } from './app/components/bookings/bookings.component';
import { authInterceptor } from './app/interceptors/auth.interceptor';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'main', component: MainPageComponent, canActivate: [AuthGuard] },
  { path: 'offer/:id', component: OfferDetailsComponent, canActivate: [AuthGuard] },
  { path: 'bookings', component: BookingsComponent },
  { path: 'dashboard', redirectTo: '/main', pathMatch: 'full' }
];

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes, withHashLocation()),
    provideHttpClient(
      withInterceptors([authInterceptor])
    ),
    importProvidersFrom(ReactiveFormsModule)
  ]
}).catch(err => console.error(err));
