import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { QuizzPost } from 'src/app/models/quizzpost';
import { QuizzRequestService } from 'src/app/services/request/quizz-request.service';
import { QuizzService } from 'src/app/services/utils/quizz.service';

@Component({
  selector: 'app-edit-quizz',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, CommonModule, RouterModule, FormsModule, RouterModule],
  templateUrl: './edit-quizz.component.html',
  styleUrl: './edit-quizz.component.scss'
})
export class EditQuizzComponent implements OnInit {

  public quizzId: number | null = null;
  public quizz: QuizzPost | null = null;

    isModalOpen = false;
    modalTitle = "";
    modalMessage = "";
    modalConfirmFn: (() => void) | null = null;

  constructor(private readonly quizzRequestService: QuizzRequestService, private readonly quizzService: QuizzService, private readonly route: ActivatedRoute, private readonly router: Router) { }

  ngOnInit(): void {
    this.quizzId = Number(this.route.snapshot.paramMap.get('id'));

    this.quizzRequestService.getQuizzP(this.quizzId).subscribe({
      next: (res) => {
        this.quizz = Array.isArray(res) ? res[0] : res;
        console.log("QUIZZ =", this.quizz);
      },
      error: (err) => {
        console.error('Erreur lors du chargement du quiz :', err);
      }
    })
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

  public deleteQuizz() {
    
    if (!this.quizzId) {
      this.openModal("Erreur", "Aucun quiz valide à supprimer.");
      return;
    }
    this.quizzRequestService.deleteQuizz(this.quizzId).subscribe({
      next: (res) => {
        console.log('Quiz supprimé avec succès :', res);
        this.router.navigate(['/home']);
      },
      error: (err) => {
        console.error('Erreur lors de la suppression du quiz :', err);
        this.openModal("Erreur", "Une erreur est survenue lors de la suppression du quiz.");
      }
    });
  }

  public saveQuizz() {
    if (!this.quizz || !this.quizzId) {
      this.openModal("Erreur", "Aucun quiz valide à enregistrer.");
      return;
    }

    let validationResult = this.quizzService.validateQuizz(this.quizz);
    if (typeof validationResult === 'string'){
      this.openModal("Erreur", validationResult);
      return;
    }else{
      this.quizz = validationResult;
    }

    this.quizzRequestService.putQuizz(this.quizzId, this.quizz).subscribe({
      next: (res) => {
        console.log('Quiz mis à jour avec succès :', res);
        this.openModal("Succès", "Le quiz a été mis à jour avec succès.", res);
      },
      error: (err) => {
        console.error('Erreur lors de la mise à jour du quiz :', err);
        this.openModal("Erreur", "Une erreur est survenue lors de la mise à jour du quiz.");
      }
    });
  }
}
