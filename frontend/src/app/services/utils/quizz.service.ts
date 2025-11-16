import { Injectable } from '@angular/core';
import { QuizzPost, QuestionPost, ChoicePost } from 'src/app/models/quizzpost';


@Injectable({
  providedIn: 'root'
})

export class QuizzService {

  constructor() { }

  public validateQuizz(quizz: QuizzPost): QuizzPost | string {

    let errorMessage: string = "";
    let i : number = 0;
    
    for (let question of quizz.questions) {

      let ammountedCorrectChoices : number = 0;

      for (let choice of question.choices) {
        if(choice.isCorrect) 
          ammountedCorrectChoices++;
      }

      if(ammountedCorrectChoices > 1)
        question.type = "MULTI";
      if(ammountedCorrectChoices < 1){
        if(!errorMessage)
          errorMessage += "<ul class='flex flex-col gap-0'>";

        errorMessage += `
          <li class="text-red-400">
            • Question ${i + 1} : aucune réponse valide.
          </li>
        `;
      }
      if(ammountedCorrectChoices === 1)
        question.type = "ONE";
      i++;
    }

    if(errorMessage){
      errorMessage += "</ul>";
      return errorMessage;
    }
     
    return quizz;
  }

  
}
