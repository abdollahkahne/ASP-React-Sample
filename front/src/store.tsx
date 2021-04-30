import { IQuestionData } from "./Data/QuestionsData";
import { Store, createStore, combineReducers } from "redux";

// Do things in the following steps
// 1- Define IAppState and its different properties
// 2- Defin IFeatureState for every define feature in step 1 with its initial value
// 3- Define Different Action Types using const or Enum
// 4- Define Action Creators which are functions that receive as manay arguments that you need and return an Action with at least one property name type
// 5- Define Reducers for Each Feature which receive two arguments including current state and action (type and payload) and return new state
// Reducer should be Pure Function
// 6- Define Root Reducer using combining different reducers which defined for Each Feature.
// It is a function which receive and Object with keys equal to Features Defined in IAppState and Values equal to corresponding Reducer
// 7- Define a Store Creator Function which create Store using create Store. create store accpet two arguments including root reducer and IAppState Initial Value (Undefined!)
// and Return an Store for IAppState Features.
export interface IAppState {
  questions: IQuestionsState;
}

interface IQuestionsState {
  readonly loading: boolean;
  readonly unAnswered: IQuestionData[];
  readonly searched: IQuestionData[];
  readonly viewing: IQuestionData | null;
}

const initialQuestionsState: IQuestionsState = {
  loading: false,
  unAnswered: [],
  searched: [],
  viewing: null,
};

// Here the type of const variable if not defined explicity is const value!
export const GETTINGUNANSEREDQUESTIONS = "GettingUnansweredQuestions";

// const assertion do two thing:
// 1- make object property readonly/immutable object (not modified after first creation)
// 2- type narrowing (instead of string we have type of GettingUnAnsweredQuestions)
export const gettingUnAnsweredQuestionsAction = () =>
  ({
    type: GETTINGUNANSEREDQUESTIONS,
  } as const);

export const GOTUNANSWEREDQUESTIONS = "GotUansweredQuestions";

export const gotUnAnsweredQuestionsAction = (questions: IQuestionData[]) =>
  ({
    type: GOTUNANSWEREDQUESTIONS,
    questions,
  } as const);

export const GETTINGQUESTION = "GettingQuestion";
export const gettingQuestionAction = () =>
  ({
    type: GETTINGQUESTION,
  } as const);

export const GOTQUESTION = "GotQuestion";
export const gotQuestionAction = (question: IQuestionData) =>
  ({
    type: GOTQUESTION,
    question,
  } as const);

export const SEARCHINGQUESTIONS = "SearchingQuestions";

export const searchingQuestionsAction = () =>
  ({
    type: SEARCHINGQUESTIONS,
  } as const);

export const SEARCHEDQUESTIONS = "SearchedQuestions";
export const searchedQuestionsAction = (question: IQuestionData[]) =>
  ({
    type: SEARCHEDQUESTIONS,
    question,
  } as const);

type TQuestionActions =
  | ReturnType<typeof gettingUnAnsweredQuestionsAction>
  | ReturnType<typeof gotUnAnsweredQuestionsAction>
  | ReturnType<typeof gettingQuestionAction>
  | ReturnType<typeof gotQuestionAction>
  | ReturnType<typeof searchingQuestionsAction>
  | ReturnType<typeof searchedQuestionsAction>;

const questionsReducer = (
  state: IQuestionsState = initialQuestionsState,
  action: TQuestionActions,
): IQuestionsState => {
  switch (action.type) {
    case GETTINGUNANSEREDQUESTIONS: {
      return {
        ...state,
        loading: true,
        unAnswered: [],
      };
    }
    case GOTUNANSWEREDQUESTIONS: {
      return {
        ...state,
        loading: false,
        unAnswered: action.questions,
      };
    }
    case GETTINGQUESTION: {
      return {
        ...state,
        viewing: null,
        loading: true,
      };
    }
    case GOTQUESTION: {
      return {
        ...state,
        viewing: action.question,
        loading: false,
      };
    }
    case SEARCHINGQUESTIONS: {
      return {
        ...state,
        loading: true,
        searched: [],
      };
    }
    case SEARCHEDQUESTIONS: {
      return {
        ...state,
        loading: false,
        searched: action.question,
      };
    }
    // only in case of undefined actions return inserted state!
    default: {
      return state;
    }
  }
};

const rootReducer = combineReducers<IAppState>({
  questions: questionsReducer,
});

export function configureStore(): Store<IAppState> {
  const store = createStore(rootReducer, undefined);
  return store;
}
