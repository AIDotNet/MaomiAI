/*
 * @Author: whuanle 1586052146@qq.com
 * @Date: 2025-04-28 14:24:13
 * @LastEditors: whuanle 1586052146@qq.com
 * @LastEditTime: 2025-05-04 15:07:32
 * @FilePath: \maomiai\src\PageRouter.tsx
 * @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 */
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
    LoginPageRouter,
    RegisterPageRouter,
    UserPageRouter,
    {
        path: "/app/*",
        Component: App,
        children: [
            UserPageRouter
        ],
    }
]);