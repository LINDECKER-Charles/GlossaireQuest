import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { combineLatest } from 'rxjs/internal/observable/combineLatest';
import { startWith } from 'rxjs/internal/operators/startWith';
import { Try } from 'src/app/models/tries';
import { User } from 'src/app/models/user';
import { UserRequestService } from 'src/app/services/request/user-request.service';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss'
})
export class HistoryComponent implements OnInit {

  public loading: boolean = true;
  public error: any = null;

  public tries: Try[] = [];
  public amountOfTries: FormControl<number> = new FormControl<number>(10, { nonNullable: true });
  public page: FormControl<number> = new FormControl<number>(1, { nonNullable: true });

  public emailUser: string | null= null;

  constructor(private router: Router, private userRequest: UserRequestService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.loadTries(this.amountOfTries.value);
    this.emailUser = this.route.snapshot.paramMap.get('email');

    console.log("Initial amount: ", this.amountOfTries.value);

    combineLatest([
      this.amountOfTries.valueChanges.pipe(startWith(this.amountOfTries.value)),
      this.page.valueChanges.pipe(startWith(this.page.value))
    ]).subscribe(([amount, page]) => {
      const scope = amount * (page - 1);
      this.loadTries(amount, scope);
    });

  }

  private loadTries(amount: number = 10, scope: number = 0): void {
    this.userRequest.getTries(amount, scope, this.emailUser || undefined).subscribe({
      next: (data) => {
        this.tries = data.tries || [];
        this.loading = false;

      },
      error: (err) => {
        this.error = "Erreur lors du chargement des tentatives.";
        console.error(err);
      }
    });
  }
}
