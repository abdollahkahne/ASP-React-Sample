/** @jsxImportSource @emotion/react */
import { css } from "@emotion/react";
import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { Page } from "./Page";
import { searchQuestions, IQuestionData } from "./Data/QuestionsData";
import { QuestionList } from "./QuestionList";

export const SearchPage = () => {
  const [searchParams] = useSearchParams();
  const criteria = searchParams.get("criteria") || "";
  const [questions, setQuestions] = useState<IQuestionData[]>([]);
  useEffect(() => {
    async function doSearch(search: string) {
      const results = await searchQuestions(search);
      setQuestions(results);
    }
    doSearch(criteria);
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
