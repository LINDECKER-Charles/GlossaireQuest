import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { User } from 'src/app/models/user';
import { UserRequestService } from 'src/app/services/request/user-request.service';

@Component({
  selector: 'app-all-users',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, CommonModule, RouterModule, FormsModule, RouterModule],
  templateUrl: './all-users.component.html',
  styleUrl: './all-users.component.scss'
})
export class AllUsersComponent implements OnInit {

  public users: User[] = [];

  public loading: boolean = false;
  public error: string | null = null;

  constructor(private readonly userRequestService: UserRequestService) { }

  ngOnInit(): void {
    this.userRequestService.getAllUsers().subscribe({
      next: (res) => {
        this.users = res;
      },
      error: (err) => {
        this.error = err;
      },
      complete: () => {
        this.loading = false;
      }
    })
  }
}
