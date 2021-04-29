import React from "react";
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
} from "./Styles";
import { useForm } from "react-hook-form";

type TFormData = {
  title: string;
  content: string;
};

const AskPage = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<TFormData>({ mode: "onBlur" });
  return (
    <Page title="Ask A Question">
      <form>
        <FieldSet>
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
              <FieldError>Please Provide A Title</FieldError>
            )}
            {errors.content && errors.content.type === "minLength" && (
              <FieldError>Title must be at least 50 characters long</FieldError>
            )}
          </FieldContainer>
          <FormButtonContainer>
            <PrimaryButton type="submit">Submit Your Question</PrimaryButton>
          </FormButtonContainer>
        </FieldSet>
      </form>
    </Page>
  );
};

export default AskPage;
