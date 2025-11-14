export interface ChoicePost {
  name: string;
  isCorrect: boolean;
}

export interface QuestionPost {
  name: string;
  description: string;
  point: number;
  type: string;
  choices: ChoicePost[];
}

export interface QuizzPost {
  name: string;
  description: string;
  questions: QuestionPost[];
}