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
export class NavBarComponent implements OnInit, OnDestroy {
  isLoggedIn = false;
  isAdmin = false;
  private sub = new Subscription();

  constructor(
    private auth: AuthService,
    private router: Router,
    private userService: UserRequestService
  ) {}

  ngOnInit(): void {
    // Ecoute réactive des changements de connexion
    this.sub.add(
      this.auth.isLoggedIn$.subscribe((loggedIn) => {
        this.isLoggedIn = loggedIn;

        // Si connecté → vérifie le rôle
        if (loggedIn) {
          this.userService.isAdmin().subscribe((isAdmin) => {
            this.isAdmin = isAdmin;
            console.log('isAdmin dans NavBar:', isAdmin);
          });
        } else {
          this.isAdmin = false;
        }
      })
    );

    // Surveille les changements de route (optionnel mais utile)
    this.sub.add(
      this.router.events
        .pipe(filter((event) => event instanceof NavigationEnd))
        .subscribe(() => {
          this.isLoggedIn = this.auth.isLoggedIn();
        })
    );
  }

  logout(): void {
    // Déconnecte et l’état se mettra à jour automatiquement
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
