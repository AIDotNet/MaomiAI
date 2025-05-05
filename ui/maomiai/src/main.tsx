/*
 * @Author: whuanle 1586052146@qq.com
 * @Date: 2025-04-28 10:29:10
 * @LastEditors: whuanle 1586052146@qq.com
 * @LastEditTime: 2025-05-04 10:48:24
 * @FilePath: \maomiai\src\main.tsx
 * @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 */
import { createRoot } from 'react-dom/client'
import { Provider } from 'react-redux';
import { RouterProvider } from "react-router";
import { ThemeProvider } from '@lobehub/ui'

import './index.css'
import store from './stateshare/store.tsx';
import { PageRouterProvider } from './PageRouter.tsx';
import { message } from 'antd';

message.config({
  maxCount: 8
});

createRoot(document.getElementById('root')!).render(
  <Provider store={store}>
  <ThemeProvider>
    <RouterProvider router={PageRouterProvider} />
  </ThemeProvider>
</Provider>
)