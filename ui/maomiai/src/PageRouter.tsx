import { createBrowserRouter, RouterProvider } from "react-router";

import Home from "./Home";
import App from "./App";
import { DashboardPageRouter } from "./Components/dashboard/PageRouter";
import { TeamPageRouter } from "./Components/team/PageRouter";
import { UserPageRouter } from "./Components/user/PageRouter";
import { LoginPageRouter } from "./Components/login/PageRouter";
import { RegisterPageRouter } from "./Components/register/PageRouter";
import { TeamListPageRouter } from "./Components/teamlist/PageRouter";
import { NotePageRouter } from "./Components/note/PageRouter";

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
