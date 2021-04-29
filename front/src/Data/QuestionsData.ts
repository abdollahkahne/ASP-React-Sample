export interface IQuestionData {
  questionId: number;
  title: string;
  content: string;
  userName: string;
  created: Date;
  answers: IAnswerData[];
}
export interface IAnswerData {
  answerId: number;
  content: string;
  userName: string;
  created: Date;
}

const questions: IQuestionData[] = [
  {
    questionId: 1,
    title: "Why should I learn TypeScript?",
    content:
      "TypeScript seems to be getting popular so I wondered whether it is worth my time learning it? What benefits does it give over JavaScript?",
    userName: "Bob",
    created: new Date(),
    answers: [
      {
        answerId: 1,
        content: "To catch problems earlier speeding up your developments",
        userName: "Jane",
        created: new Date(),
      },
      {
        answerId: 2,
        content:
          "So, that you can use the JavaScript features of tomorrow, today",
        userName: "Fred",
        created: new Date(),
      },
    ],
  },
  {
    questionId: 2,
    title: "Which state management tool should I use?",
    content:
      "There seem to be a fair few state management tools around for React - React, Unstated, ... Which one should I use?",
    userName: "Bob",
    created: new Date(),
    answers: [],
  },
  {
    questionId: 3,
    title: "Server Side Rendering Using The Approach in Book vs Next.JS?",
    content:
      "Since we implemented SSR and SPA simultaneously, I am wondering what the advantage have the Next.js over the approach we implemented. Are these two approaches are comparable at all? What is the pros and cons of each?",
    userName: "Rafael",
    created: new Date(),
    answers: [],
  },
];

export const getUnansweredQuestions = async (): Promise<IQuestionData[]> => {
  await wait(500);
  return questions.filter((q) => q.answers.length === 0);
};

export const getQuestion = (
  questionId: number,
): Promise<IQuestionData | undefined> => {
  const question = questions.find((q) => q.questionId === questionId);
  return wait(500).then(() => question);
};

export const searchQuestions = (criteria: string): Promise<IQuestionData[]> => {
  const criteriaLC = criteria.toLowerCase();
  const returnedQuestions = questions.filter(
    (q) =>
      q.title.toLowerCase().includes(criteriaLC) ||
      q.content.toLowerCase().includes(criteriaLC),
  );
  return wait(500).then(() => returnedQuestions);
};

const wait = async (ms: number): Promise<void> => {
  return new Promise((resolve) => setTimeout(resolve, ms));
};
