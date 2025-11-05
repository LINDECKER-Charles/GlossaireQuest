import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { QuizzRequestService, Quizz } from '../../../services/request/quizz-request.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  constructor(public quizzRequestService: QuizzRequestService) {}

  public quizzList: Quizz[] = [];
  ngOnInit(): void {
    this.quizzRequestService.getAllQuizzes().subscribe(quizzes => {
      console.log(quizzes);
      this.quizzList = quizzes;
    });
  }
}
