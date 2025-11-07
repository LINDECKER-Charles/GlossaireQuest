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


export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },

  { path: 'login', component: LoginComponent, canActivate: [NoAuthGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [NoAuthGuard] },

  { path: 'home', component: HomeComponent },
  { path: 'quizz/:id', component: QuizzComponent },
  { path: 'quizz-create', component: CreateQuizzComponent },
  
  { path: 'profil', component: ProfilComponent, canActivate: [authGuard] },

  { path: 'about', component: AboutComponent }
];
