import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { Subscription, filter } from 'rxjs';
import { AuthService } from '../../../services/auth.service';
import { UserRequestService } from 'src/app/services/request/user-request.service';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './nav-bar.component.html',
})
export class NavBarComponent implements OnInit {
  isLoggedIn = false;
  isAdmin = false;
  private sub = new Subscription();

  constructor(
    private auth: AuthService,
    private router: Router,
    private userService: UserRequestService
  ) {}

  ngOnInit(): void {
    // Vérifie immédiatement l'état actuel
    this.isLoggedIn = !this.auth.isTokenExpired();
    console.log(this.userService.isAdmin());
    if (this.isLoggedIn && this.userService.isAdmin()) {
      this.isAdmin = true;
      console.log('isAdmin dans NavBar:', true);
    }
  }


  logout(): void {
    // Déconnecte et l’état se mettra à jour automatiquement
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
