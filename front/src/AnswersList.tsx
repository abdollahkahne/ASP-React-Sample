/** @jsxImportSource @emotion/react */
import { css } from "@emotion/react";
import React from "react";
import { IAnswerData } from "./Data/QuestionsData";
import { Answer } from "./Answer";
import { gray5 } from "./Styles";

interface IProps {
  data: IAnswerData[];
}

export const AnswersList = ({ data }: IProps) => (
  <ul
    css={css`
      list-style: none;
      margin: 10px 0 0 0;
      padding: 0;
    `}
  >
    {data.map((answer) => (
      <li
        css={css`
          border-top: 1px solid ${gray5};
        `}
        key={answer.answerId}
      >
        <Answer data={answer} />
      </li>
    ))}
  </ul>
);
