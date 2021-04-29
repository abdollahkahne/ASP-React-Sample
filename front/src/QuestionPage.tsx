/** @jsxImportSource @emotion/react */
import { css } from "@emotion/react";
import {
  gray3,
  gray6,
  FieldSet,
  FieldContainer,
  FieldLabel,
  FieldTextArea,
  FormButtonContainer,
  PrimaryButton,
} from "./Styles";
import React, { useEffect, useState } from "react";
import { Page } from "./Page";
import { useParams } from "react-router-dom";
import { getQuestion, IQuestionData } from "./Data/QuestionsData";
import { AnswersList } from "./AnswersList";
import { useForm } from "react-hook-form";

type TFormData = {
  content: string;
};

export const QuestionPage = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<TFormData>();
  const { questionId } = useParams();
  const [question, setQuestion] = useState<IQuestionData>();
  useEffect(() => {
    getQuestion(Number.parseInt(questionId)).then((q) => {
      if (q) {
        setQuestion(q);
      }
    });
  }, [questionId]);
  return (
    <Page>
      <div
        css={css`
          box-sizing: border-box;
          padding: 15px 20px 20px 20px;
          border: 1px solid ${gray6};
          border-radius: 3px;
          background-color: white;
          box-shadow: 0 3px 5px 0 rgba(0, 0, 0, 0.16);
        `}
      >
        <div
          css={css`
            font: 19px bold;
            margin: 10px 0 5px;
          `}
        >
          {question ? question.title : ""}
        </div>
        {question ? (
          <React.Fragment>
            <p
              css={css`
                margin-top: 0px;
                background-color: white;
              `}
            >
              {question.content}
            </p>
            <div
              css={css`
                font: 12px italic;
                color: ${gray3};
              `}
            >
              {`Asked By ${
                question.userName
              } at ${question.created.toLocaleDateString()} ${question.created.toLocaleTimeString()}`}
            </div>
            <AnswersList data={question.answers} />
            <form
              css={css`
                margin-top: 20px;
              `}
            >
              <FieldSet>
                <FieldContainer>
                  <FieldLabel htmlFor="content">Your Answer</FieldLabel>
                  <FieldTextArea id="content" {...register("content")} />
                </FieldContainer>
                <FormButtonContainer>
                  <PrimaryButton type="submit">
                    Submit Your Answer
                  </PrimaryButton>
                </FormButtonContainer>
              </FieldSet>
            </form>
          </React.Fragment>
        ) : (
          ""
        )}
      </div>
    </Page>
  );
};
