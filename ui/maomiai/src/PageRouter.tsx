import {
    createBrowserRouter,
    RouterProvider,
} from "react-router";

import Home from './Home'
import App from "./App";
import { UserPageRouter } from "./Components/user/PageRouter";
import { LoginPageRouter } from "./Components/login/PageRouter";
import { RegisterPageRouter } from "./Components/register/PageRouter";

// 在此集合所有页面的路由，每个子模块的路由从模块下的 PageRouter 导出


export const PageRouterProvider = createBrowserRouter([
    {
        path: "/",
        Component: Home,
    },
    {
        path: "/login",
        Component: Home,
    },
    {
        path: "/register",
        Component: Home,
    },
    {
        path: "/app/*",
        Component: App,
        children: [
            UserPageRouter
        ],
    }
]);