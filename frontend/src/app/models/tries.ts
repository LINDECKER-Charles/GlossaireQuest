export interface Try {
  tryId: number;
  quizId: number;
  quizName: string;
  result: number;
  date: string;
}

export interface Tries {
  message: string;
  tries: Try[];
}