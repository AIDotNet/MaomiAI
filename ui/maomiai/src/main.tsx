import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { Provider } from 'react-redux';
import { RouterProvider } from "react-router";
import { ThemeProvider } from '@lobehub/ui'

import './index.css'
import store from './stateshare/store.tsx';
import { PageRouterProvider } from './PageRouter.tsx';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <ThemeProvider>
        <RouterProvider router={PageRouterProvider} />
      </ThemeProvider>
    </Provider>
  </StrictMode>
)