import { Injectable } from '@angular/core';
import { AbstractRequestService } from './abstract-request.service';
import { Observable } from 'rxjs/internal/Observable';

export interface Choice {
  id: number;
  name: string;
  isCorrect: boolean;
  selected?: boolean;
}

export interface Question {
  id: number;
  name: string;
  description: string;
  point: number;
  type: string;
  choices: Choice[];
}

export interface Quizz {
  id: number;
  name: string;
  description: string;
  author: string;
  questionCount: number;
  questions: Question[];
}


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

}
