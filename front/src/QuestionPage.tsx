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
  FieldError,
} from "./Styles";
import React, { useEffect } from "react";
import { Page } from "./Page";
import { useParams } from "react-router-dom";
import { getQuestion, giveAnswer, INewAnswer } from "./Data/QuestionsData";
import { AnswersList } from "./AnswersList";
import { useForm } from "react-hook-form";
import { useDispatch, useSelector } from "react-redux";
import { IAppState, gotQuestionAction, gettingQuestionAction } from "./store";

type TFormData = {
  content: string;
};

export const QuestionPage = () => {
  const dispatch = useDispatch();
  const {
    register,
    reset,
    handleSubmit,
    formState: { errors },
  } = useForm<TFormData>({ mode: "onBlur" });
  const { questionId } = useParams();
  const question = useSelector((store: IAppState) => store.questions.viewing);
  useEffect(() => {
    dispatch(gettingQuestionAction());
    getQuestion(Number.parseInt(questionId)).then((q) => {
      if (q) {
        dispatch(gotQuestionAction(q));
      }
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [questionId]);
  const submitForm = ({ content }: TFormData) => {
    const answer: INewAnswer = { content, userName: "Ali" };
    giveAnswer(answer, Number.parseInt(questionId))
      .then((a) => reset())
      .catch((err) => err);
  };
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
              onSubmit={handleSubmit(submitForm)}
            >
              <FieldSet>
                <FieldContainer>
                  <FieldLabel htmlFor="content">Your Answer</FieldLabel>
                  <FieldTextArea
                    id="content"
                    {...register("content", {
                      required: "true",
                      minLength: 50,
                    })}
                  />
                  {errors.content && errors.content.type === "required" && (
                    <FieldError>This Field is Required</FieldError>
                  )}
                  {errors.content && errors.content.type === "minLength" && (
                    <FieldError>Min Length of Answer should be 50</FieldError>
                  )}
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
