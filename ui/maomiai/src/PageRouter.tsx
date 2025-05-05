import { createBrowserRouter, RouterProvider } from "react-router";

import Home from "./Home";
import App from "./App";
import { AiModelPageRouter } from "./Components/aimodel/PageRouter";
import { ApplicationPageRouter } from "./Components/application/PageRouter";
import { WikiPageRouter } from "./Components/wiki/PageRouter";
import { NotePageRouter } from "./Components/note/PageRouter";
import { PluginPageRouter } from "./Components/plugin/PageRouter";
import { TeamPageRouter } from "./Components/team/PageRouter";
import { UserPageRouter } from "./Components/user/PageRouter";
import { LoginPageRouter } from "./Components/login/PageRouter";
import { RegisterPageRouter } from "./Components/register/PageRouter";
import { DashboardPageRouter } from "./Components/dashboard/PageRouter";

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
      AiModelPageRouter,
      ApplicationPageRouter,
      WikiPageRouter,
      NotePageRouter,
      PluginPageRouter,
      TeamPageRouter,
      UserPageRouter,
    ],
  },
  LoginPageRouter,
  RegisterPageRouter,
]);
