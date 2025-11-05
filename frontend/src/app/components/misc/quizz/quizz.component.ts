import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { QuizzRequestService, Quizz, Question, Choice } from '../../../services/request/quizz-request.service';
import { UserRequestService } from 'src/app/services/request/user-request.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-quizz',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './quizz.component.html',
  styleUrls: ['./quizz.component.scss']
})
export class QuizzComponent implements OnInit {
  public quiz: Quizz | null = null;

  // 🔹 États du quiz
  public isPlaying = false;
  public isFinished = false;
  public score = 0;
  public totalPoints = 0;

  constructor(
    private quizzRequestService: QuizzRequestService,
    private route: ActivatedRoute,
    private userRequestService: UserRequestService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    console.log('ID du quiz :', id);

    this.quizzRequestService.getQuizz(id).subscribe({
      next: (quiz) => {
        console.log('Données reçues :', quiz);
        this.quiz = Array.isArray(quiz) ? quiz[0] : quiz;

        // Calcul du total de points possibles
        if (this.quiz) {
          this.totalPoints = this.quiz.questions.reduce(
            (sum, q) => sum + (q.point || 0),
            0
          );
          console.log('Total des points possibles :', this.totalPoints);
        }
      },
      error: (err) => {
        console.error('Erreur lors du chargement du quiz :', err);
      }
    });
  }

  // Lancer le quiz
  public startQuiz(): void {
    this.isPlaying = true;
    this.isFinished = false;
    this.score = 0;
  }

  // Sélectionner une réponse
  public selectChoice(question: any, choice: any): void {
    // Si question de type ONE → une seule réponse possible
    if (question.type === 'ONE') {
      question.choices.forEach((c: any) => (c.selected = false));
      choice.selected = true;
    } 
    // Si question de type MULTI → bascule le choix
    else if (question.type === 'MULTI') {
      choice.selected = !choice.selected;
    }
  }

  // Soumettre le quiz
  public submitQuiz(): void {
    if (!this.quiz) return;

    let total = 0;

    this.quiz.questions.forEach((q: Question) => {
      if (q.type === 'MULTI') {
          let good = true;
          q.choices.forEach((c: Choice) => {
            if(c.isCorrect){
              if(!c.selected) good = false;
            }
          });
          if(good) total += q.point;
      }else{
        q.choices.forEach((c: Choice) => {
          if (c.selected && c.isCorrect) {
            total += q.point;
            return;
          }
        });
      }
    });

    this.score = total;
    this.isFinished = true;
    this.isPlaying = false;
    if(this.authService.isLoggedIn()){
      this.userRequestService.addTry(this.score, this.quiz.id).subscribe({
        next: (response) => {
          console.log('Essai ajouté avec succès :', response);
        },
        error: (err) => {
          console.error('Erreur lors de l\'ajout de l\'essai :', err);
        }
      });
    }

  }

  // Rejouer
  public retryQuiz(): void {
    if (!this.quiz) return;

    this.isPlaying = true;
    this.isFinished = false;
    this.score = 0;

    this.quiz.questions.forEach((q: Question) =>
      q.choices.forEach((c: Choice) => (c.selected = false))
    );
  }
}
