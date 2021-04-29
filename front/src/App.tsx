/** @jsxImportSource @emotion/react */
import { css } from "@emotion/react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { SearchPage } from "./SearchPage";
import { SignInPage } from "./SignIn";
import { NotFoundPage } from "./NotFoundPage";
import { Header } from "./Header";
import { HomePage } from "./HomePage";
import { QuestionPage } from "./QuestionPage";
import React from "react";
import { fontFamily, fontSize, gray2 } from "./Styles";

const AskPage = React.lazy(() => import("./AskPage"));

function App() {
  return (
    <BrowserRouter>
      <div
        css={css`
          font-family: ${fontFamily};
          font-size: ${fontSize};
          color: ${gray2};
        `}
      >
        <Header />
        <Routes>
          <Route path="" element={<HomePage />} />
          <Route path="search" element={<SearchPage />} />
          <Route
            path="ask"
            element={
              <React.Suspense
                fallback={
                  <div
                    css={css`
                      margin-top: 100px;
                      text-align: center;
                    `}
                  >
                    Loading
                  </div>
                }
              >
                <AskPage />
              </React.Suspense>
            }
          />
          <Route path="signin" element={<SignInPage />} />
          <Route path="questions/:questionId" element={<QuestionPage />} />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;