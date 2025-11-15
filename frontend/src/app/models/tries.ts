export interface Try {
  tryId: number;
  quizId: number;
  quizName: string;
  result: number;
  date: string;
  maxPoint: number;
}

export interface Tries {
  message: string;
  tries: Try[];
}