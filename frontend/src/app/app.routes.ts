import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { HomeComponent } from './components/misc/home/home.component';
import { NoAuthGuard } from './guards/no-auth.guard';
import { AboutComponent } from './components/misc/about/about.component';
import { ProfilComponent } from './components/user/profil/profil.component';
import { authGuard } from './guards/auth.guard';
import { QuizzComponent } from './components/misc/quizz/quizz.component';
import { CreateQuizzComponent } from './components/misc/create-quizz/create-quizz.component';
import { isAdminGuard } from './guards/is-admin.guard';
import { VerifyEmailComponent } from './components/auth/verify-email/verify-email.component';
import { ConfirmEmailComponent } from './components/auth/confirm-email/confirm-email.component';
import { ResetPasswordComponent } from './components/auth/reset-password/reset-password.component';
import { ForgotPasswordComponent } from './components/auth/forgot-password/forgot-password.component';
import { DeleteAccountComponent } from './components/user/action/delete-account/delete-account.component';
import { EditNameComponent } from './components/user/action/edit-name/edit-name.component';
import { EditEmailComponent } from './components/user/action/edit-email/edit-email.component';
import { EditPasswordComponent } from './components/user/action/edit-password/edit-password.component';


export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },

  { path: 'login', component: LoginComponent, canActivate: [NoAuthGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [NoAuthGuard] },

  { path: 'check-email', component: ConfirmEmailComponent },
  { path: 'send-reset-password', component: ForgotPasswordComponent, canActivate: [NoAuthGuard] },
  { path: 'reset', component: ResetPasswordComponent },

  { path: 'verify', component: VerifyEmailComponent },

  { path: 'home', component: HomeComponent },
  { path: 'quizz/:id', component: QuizzComponent },
  { path: 'quizz-create', component: CreateQuizzComponent },
  
  { path: 'profil', component: ProfilComponent, canActivate: [authGuard] },
  { path: 'profil/password-change', component: EditPasswordComponent, canActivate: [authGuard] },
  { path: 'profil/email-change', component: EditEmailComponent, canActivate: [authGuard] },
  { path: 'profil/name-change', component: EditNameComponent, canActivate: [authGuard] },
  { path: 'profil/delete-account', component: DeleteAccountComponent, canActivate: [authGuard] },
  
  { path: 'about', component: AboutComponent }
];
