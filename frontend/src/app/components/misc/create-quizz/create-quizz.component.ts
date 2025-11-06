import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { QuizzRequestService, QuizzPost } from 'src/app/services/request/quizz-request.service';


@Component({
  selector: 'app-create-quizz',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-quizz.component.html',
  styleUrls: ['./create-quizz.component.scss']
})

export class CreateQuizzComponent {

  quizzForm: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder, private quizzService: QuizzRequestService) {
    this.quizzForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      questions: this.fb.array([])
    });
  }

  get questions(): FormArray {
    return this.quizzForm.get('questions') as FormArray;
  }

  getChoices(questionIndex: number): FormArray {
    return this.questions.at(questionIndex).get('choices') as FormArray;
  }

  addQuestion(): void {
    const question = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      point: [1, [Validators.required, Validators.min(1)]],
      type: ['ONE', Validators.required],
      choices: this.fb.array([])  // 👈 doit être bien ici
    });
    this.questions.push(question);
  }

  removeQuestion(index: number): void {
    this.questions.removeAt(index);
  }

  addChoice(questionIndex: number): void {
    const choice = this.fb.group({
      name: ['', Validators.required],
      isCorrect: [false]
    });
    this.getChoices(questionIndex).push(choice);
  }

  removeChoice(questionIndex: number, choiceIndex: number): void {
    this.getChoices(questionIndex).removeAt(choiceIndex);
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.quizzForm.invalid) return;

    const quizz: QuizzPost = this.quizzForm.value;
    this.quizzService.postQuizz(quizz).subscribe({
      next: (res) => {
        console.log('Quiz créé avec succès :', res);
        alert('Quiz créé avec succès !');
        this.quizzForm.reset();
        this.questions.clear();
        this.submitted = false;
      },
      error: (err) => console.error('Erreur lors de la création du quiz :', err)
    });
  }
}
