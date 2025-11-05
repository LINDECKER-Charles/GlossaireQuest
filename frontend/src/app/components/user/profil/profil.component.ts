import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { UserRequestService, User, Try } from 'src/app/services/request/user-request.service';

@Component({
  selector: 'app-profil',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './profil.component.html',
  styleUrl: './profil.component.scss'
})
export class ProfilComponent implements OnInit {
  public loading: boolean = true;
  public error: any = null;
  public user: User | null = null;
  public tries: Try[] = [];
  public triesSummary = { total: 0, best: 0, average: 0 };

  constructor(
    private userRequest: UserRequestService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUser();
    this.loadTries();
  }

  // Charger les informations de l’utilisateur
  private loadUser(): void {
    this.userRequest.getUser().subscribe({
      next: (response) => {
        this.user = response;
        this.loading = false;
      },
      error: (err) => {
        this.error = "Erreur lors du chargement du profil utilisateur.";
        this.loading = false;
        console.error(err);
      }
    });
  }

  // Charger les tentatives et calculer le résumé
  private loadTries(): void {
    this.userRequest.getTries().subscribe({
      next: (data) => {
        this.tries = data.tries || [];
        this.computeSummary();
      },
      error: (err) => {
        this.error = "Erreur lors du chargement des tentatives.";
        console.error(err);
      }
    });
  }

  // Calcul du résumé global
  private computeSummary(): void {
    if (!this.tries.length) {
      this.triesSummary = { total: 0, best: 0, average: 0 };
      return;
    }

    const results = this.tries.map(t => t.result);
    const total = results.length;
    const best = Math.max(...results);
    const average = results.reduce((a, b) => a + b, 0) / total;

    this.triesSummary = { total, best, average };
  }

  // Déconnexion
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  // Changements de profil
  changePassword(): void {
    this.router.navigate(['/profil/password-change']);
  }

  changeEmail(): void {
    this.router.navigate(['/profil/email-change']);
  }

  changeName(): void {
    this.router.navigate(['/profil/name-change']);
  }
}
