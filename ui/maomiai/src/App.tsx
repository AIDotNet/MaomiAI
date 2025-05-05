import { useEffect } from "react";
import { Route, Routes, useNavigate, Link } from "react-router";
import { Layout, Menu, message } from "antd";
import { Header } from "@lobehub/ui";
import "./App.css";
import { CheckToken, RefreshServerInfo } from "./InitPage";
import { GetApiClient } from "./Components/ServiceClient";

import AiModel from "./Components/aimodel/AiModel";
import Application from "./Components/application/Application";
import Wiki from "./Components/wiki/Wiki";
import Note from "./Components/note/Note";
import Plugin from "./Components/plugin/Plugin";
import Team from "./Components/team/Team";
import User from "./Components/user/User";
import Dashboard from "./Components/dashboard/Dashboard";

const { Sider, Content, Footer } = Layout;

function App() {
  const [messageApi, contextHolder] = message.useMessage();
  const navigate = useNavigate();

  useEffect(() => {
    let client = GetApiClient();
    const fetchData = async () => {
      await RefreshServerInfo(client);
      var isVerify = await CheckToken();
      if (!isVerify) {
        messageApi.success("身份已失效，正在重定向到登录页面");
        setTimeout(() => {
          navigate("/login");
        }, 1000);
      }
    };
    fetchData();

    // 每分钟刷新一次 token
    const refreshToken = setInterval(async () => {
      await CheckToken();
    }, 1000 * 60);

    return () => {
      clearInterval(refreshToken);
    };
  }, []);

  const horizontalMenuItems = [
    { key: "1", label: "Home" },
    { key: "2", label: "About" },
    { key: "3", label: "Contact" },
  ];

  const inlineMenuItems = [
    { key: "1", label: <Link to="/app/index">首页</Link> },
    { key: "2", label: <Link to="/app/team">团队</Link> },
    { key: "3", label: <Link to="/app/aimodel">模型</Link> },
    { key: "4", label: <Link to="/app/application">应用</Link> },
    { key: "5", label: <Link to="/app/wiki">知识库</Link> },
    { key: "6", label: <Link to="/app/note">笔记系统</Link> },
    { key: "7", label: <Link to="/app/plugin">插件</Link> },
    { key: "8", label: <Link to="/app/user">个人中心</Link> },
  ];

  return (
    <Layout className="layout">
      <Header className="header">
        <div className="logo">My App</div>
        <Menu
          theme="dark"
          mode="horizontal"
          defaultSelectedKeys={["1"]}
          className="menu-horizontal"
          items={horizontalMenuItems}
        />
      </Header>
      <Layout>
        <Sider width={200} className="sider" collapsible>
          <Menu
            mode="inline"
            defaultSelectedKeys={["1"]}
            className="menu-inline"
            items={inlineMenuItems}
          />
        </Sider>
        <Layout style={{ padding: "0 24px 24px" }}>
          <Content className="content">
            <Routes>
              <Route element={<Dashboard />} />
              <Route path="aimodel" element={<AiModel />} />
              <Route path="application" element={<Application />} />
              <Route path="document" element={<Wiki />} />
              <Route path="note" element={<Note />} />
              <Route path="plugin" element={<Plugin />} />
              <Route path="user" element={<User />} />
              <Route path="team" element={<Team />} />
            </Routes>
          </Content>
          <Footer className="footer">
            MaomiAI ©2025
          </Footer>
        </Layout>
      </Layout>
    </Layout>
  );
}

export default App;
