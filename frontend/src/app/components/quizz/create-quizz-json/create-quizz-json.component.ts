import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { QuizzPost, QuestionPost, ChoicePost } from 'src/app/models/quizzpost';
import { QuizzRequestService } from 'src/app/services/request/quizz-request.service';
import { QuizzService } from 'src/app/services/utils/quizz.service';

@Component({
  selector: 'app-create-quizz-json',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule, FormsModule, RouterModule],
  templateUrl: './create-quizz-json.component.html',
  styleUrl: './create-quizz-json.component.scss'
})

export class CreateQuizzJsonComponent {


  constructor(private readonly quizzRequestService: QuizzRequestService, private readonly quizzService: QuizzService) { }

  rawJson: string = '';
  parsedQuizz: QuizzPost | null = null;
  error: string | null = null;

  public showPreview: boolean = false;

  isModalOpen = false;
  modalTitle = "";
  modalMessage = "";
  modalConfirmFn: (() => void) | null = null;
  quizzId: number | null = null;

  public convertJson() {
    try {
      this.parsedQuizz = JSON.parse(this.rawJson);
      this.showPreview = true;
      this.error = null;
    } catch (e) {
      this.error = 'Invalid JSON format';
      this.parsedQuizz = null;
    }
  }

  

  public openModal(title: string, message: string, confirmFn?: () => void) {
    this.modalTitle = title;
    this.modalMessage = message;
    this.modalConfirmFn = confirmFn ?? null;
    this.isModalOpen = true;
  }

  public closeModal() {
    this.isModalOpen = false;
  }

  public confirmModal() {
    if (this.modalConfirmFn) this.modalConfirmFn();
    this.closeModal();
  }


  public saveQuizz() {

    if (!this.parsedQuizz) {
      this.openModal("Erreur", "Aucun quiz valide à enregistrer.");
      return;
    }

    let validationResult = this.quizzService.validateQuizz(this.parsedQuizz);
    if (typeof validationResult === 'string'){
      this.openModal("Erreur", validationResult);
      return;
    }else{
      this.parsedQuizz = validationResult;
    }


    this.quizzRequestService.postQuizz(this.parsedQuizz).subscribe({
      next: (response) => {
        console.log(response);
        this.quizzId = response.id;
        this.openModal("Succès", "Le quiz a été enregistré avec succès.");
      },
      error: (err) => {
        this.openModal("Erreur", "Une erreur est survenue lors de l'enregistrement du quiz : " + err.error.message);
      }
    });

  }
}