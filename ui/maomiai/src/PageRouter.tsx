import { createBrowserRouter, RouterProvider } from "react-router";

import Home from "./Home";
import App from "./App";
import { DashboardPageRouter } from "./components/dashboard/PageRouter";
import { TeamPageRouter } from "./components/team/PageRouter";
import { UserPageRouter } from "./components/user/PageRouter";
import { LoginPageRouter } from "./components/login/PageRouter";
import { RegisterPageRouter } from "./components/register/PageRouter";
import { TeamListPageRouter } from "./components/teamlist/PageRouter";
import { NotePageRouter } from "./components/note/PageRouter";

// 在此集合所有页面的路由，每个子模块的路由从模块下的 PageRouter 导出

export const PageRouterProvider = createBrowserRouter([
  {
    path: "/",
    Component: Home,
  },
  {
    path: "/app/*",
    Component: App,
    children: [
      DashboardPageRouter,
      TeamListPageRouter,
      TeamPageRouter,
      NotePageRouter,
      UserPageRouter,
    ],
  },
  LoginPageRouter,
  RegisterPageRouter,
]);
