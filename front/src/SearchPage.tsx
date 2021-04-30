/** @jsxImportSource @emotion/react */
import { css } from "@emotion/react";
import React, { useEffect } from "react";
import { useSearchParams } from "react-router-dom";
import { Page } from "./Page";
import { searchQuestions } from "./Data/QuestionsData";
import { QuestionList } from "./QuestionList";
import { useDispatch, useSelector } from "react-redux";
import {
  searchedQuestionsAction,
  searchingQuestionsAction,
  IAppState,
} from "./store";

export const SearchPage = () => {
  const dispatch = useDispatch();
  const [searchParams] = useSearchParams();
  const criteria = searchParams.get("criteria") || "";
  const questions = useSelector((store: IAppState) => store.questions.searched);
  useEffect(() => {
    async function doSearch(search: string) {
      dispatch(searchingQuestionsAction());
      const results = await searchQuestions(search);
      dispatch(searchedQuestionsAction(results));
    }
    doSearch(criteria);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [criteria]);
  return (
    <Page title={`Search Results`}>
      {criteria && (
        <p
          css={css`
            font-size: 16px;
            font-style: italic;
            margin-top: 0px;
          `}
        >
          for {criteria}
        </p>
      )}
      <QuestionList data={questions} />
    </Page>
  );
};
