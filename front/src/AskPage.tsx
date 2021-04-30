import React, { useState } from "react";
import { Page } from "./Page";
import {
  FieldContainer,
  FieldLabel,
  FieldInput,
  FieldTextArea,
  FormButtonContainer,
  PrimaryButton,
  FieldSet,
  FieldError,
  SubmissionSuccess,
} from "./Styles";
import { useForm } from "react-hook-form";
import { INewQuestion, askQuestion } from "./Data/QuestionsData";

type TFormData = {
  title: string;
  content: string;
};

const AskPage = () => {
  const [submited, setSubmited] = useState(false);
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<TFormData>({ mode: "onBlur" });

  const submitForm = ({ title, content }: TFormData) => {
    const q: INewQuestion = { title, content, userName: "Ali" };
    askQuestion(q).then((q) => setSubmited(true));
  };

  return (
    <Page title="Ask A Question">
      <form onSubmit={handleSubmit(submitForm)}>
        <FieldSet disabled={isSubmitting || submited}>
          <FieldContainer>
            <FieldLabel htmlFor="title">Title</FieldLabel>
            <FieldInput
              id="title"
              {...register("title", {
                required: true,
                minLength: 10,
              })}
            />
            {errors.title && errors.title.type === "required" && (
              <FieldError>Please Provide A Title</FieldError>
            )}
            {errors.title && errors.title.type === "minLength" && (
              <FieldError>Title must be at least 10 characters long</FieldError>
            )}
          </FieldContainer>
          <FieldContainer>
            <FieldLabel htmlFor="content">Content</FieldLabel>
            <FieldTextArea
              id="content"
              {...register("content", { required: true, minLength: 50 })}
            />
            {errors.content && errors.content.type === "required" && (
              <FieldError>Please Provide A Content</FieldError>
            )}
            {errors.content && errors.content.type === "minLength" && (
              <FieldError>Title must be at least 50 characters long</FieldError>
            )}
          </FieldContainer>
          <FormButtonContainer>
            <PrimaryButton type="submit">Submit Your Question</PrimaryButton>
          </FormButtonContainer>
          {submited && (
            <SubmissionSuccess>
              Your question was successfully submitted
            </SubmissionSuccess>
          )}
        </FieldSet>
      </form>
    </Page>
  );
};

export default AskPage;
