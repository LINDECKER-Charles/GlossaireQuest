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