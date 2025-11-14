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

  public isLoggedIn = false;
  public isAdmin = false;

  private sub = new Subscription();

  constructor(
    private auth: AuthService,
  ) {}

  ngOnInit(): void {

    this.sub.add(
      this.auth.loggedIn$.subscribe((status: boolean) => {
        this.isLoggedIn = status;
      })
    );

    this.sub.add(
      this.auth.isAdmin$.subscribe((status: boolean) => {
        this.isAdmin = status;
      })
    );

  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  logout(): void {
    // Déconnecte et l’état se mettra à jour automatiquement
    this.auth.logout();
  }


}
