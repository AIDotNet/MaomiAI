import { useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { CheckToken, InitServerInfo } from './InitPage'
import { useNavigate } from 'react-router'
import { message } from 'antd'

function App() {
  const [count, setCount] = useState(0)
  const [messageApi, contextHolder] = message.useMessage();
  const navigate = useNavigate();
  
  useEffect(() => {
    (async () => {
      await InitServerInfo();
      var isVerify = await CheckToken();
      if (!isVerify) {
        messageApi.error("登录失效，正在重定向到登录页面");
        setTimeout(() => {
          navigate("/login");
        }, 1000);
      }
    })();

    // 每分钟刷新一次 token
    const refreshToken = setInterval(async () => {
      await CheckToken();
    }, 1000 * 60);


    return () => {
      clearInterval(refreshToken);
    };
  }, []); 


  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
