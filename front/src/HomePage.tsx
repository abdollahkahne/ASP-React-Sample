/** @jsxImportSource @emotion/react */
import { css } from "@emotion/react";
import React from "react";
import { QuestionList } from "./QuestionList";
import { getUnansweredQuestions } from "./Data/QuestionsData";
import { Page } from "./Page";
import { PageTitle } from "./PageTitle";
import { PrimaryButton } from "./Styles";
import { useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import {
  IAppState,
  gettingUnAnsweredQuestionsAction,
  gotUnAnsweredQuestionsAction,
} from "./store";

export const HomePage = () => {
  const dispatch = useDispatch();
  const questions = useSelector(
    (store: IAppState) => store.questions.unAnswered,
  );
  const questionsLoading = useSelector(
    (store: IAppState) => store.questions.loading,
  );
  // const [questions, setQuestions] = React.useState<IQuestionData[]>([]);
  // const [questionsLoading, setQuestionsLoading] = React.useState(true);

  React.useEffect(() => {
    const doGetUnansweredQuestions = async () => {
      dispatch(gettingUnAnsweredQuestionsAction());
      const unansweredQuestions = await getUnansweredQuestions();
      dispatch(gotUnAnsweredQuestionsAction(unansweredQuestions));
      // setQuestions(unansweredQuestions);
      // setQuestionsLoading(false);
    };
    doGetUnansweredQuestions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  const navigate = useNavigate();
  const handleAskQuestionClick = () => {
    navigate("ask");
  };

  return (
    <Page>
      <div
        css={css`
          display: flex;
          align-items: center;
          justify-content: space-between;
        `}
      >
        <PageTitle>Unanswered Questions</PageTitle>
        <PrimaryButton onClick={handleAskQuestionClick}>
          Ask a question
        </PrimaryButton>
      </div>
      {questionsLoading ? (
        <div>Loading...</div>
      ) : (
        <QuestionList data={questions} />
      )}
    </Page>
  );
};
