import { Injectable } from '@angular/core';
import { AbstractRequestService } from './abstract-request.service';
import { Observable } from 'rxjs/internal/Observable';
import { Choice, Question, Quizz } from 'src/app/models/quizz';
import { ChoicePost, QuizzPost, QuestionPost } from 'src/app/models/quizzpost';

@Injectable({
  providedIn: 'root'
})

export class QuizzRequestService extends AbstractRequestService {

  private url = this.baseUrl + '/quizz';

  public getQuizz(id: number): Observable<Quizz> {
    return this.requestNoAuth<Quizz>('GET', this.url + '?id=' + id + '' );
  }

  public getAllQuizzes(): Observable<Quizz[]> {
    return this.requestNoAuth<Quizz[]>('GET', this.url + '/summary');
  }

  public postQuizz(quizz: QuizzPost): Observable<any> {
    return this.request<any>('POST', this.url, quizz);
  }

}
