import { useEffect, useState } from "react";
import { Route, Routes, useNavigate, Link, useLocation } from "react-router";
import {
  Layout,
  Menu,
  message,
  Image,
  Modal,
  Card,
  Avatar,
  Space,
  Tag,
  Spin,
} from "antd";
import { Header } from "@lobehub/ui";
import {
  TeamOutlined,
  HomeOutlined,
  AppstoreOutlined,
  DashboardOutlined,
  RobotOutlined,
  AppstoreAddOutlined,
  BookOutlined,
  ApiOutlined,
  SettingOutlined,
  FileTextOutlined,
  UserOutlined,
} from "@ant-design/icons";
import "./App.css";
import { CheckToken, RefreshServerInfo } from "./InitPage";
import { GetApiClient } from "./components/ServiceClient";

import TeamList from "./components/teamlist/TeamList";
import User from "./components/user/User";
import Dashboard from "./components/dashboard/Dashboard";
import Team from "./components/team/Team";
import Note from "./components/note/Note";
import TeamDashboard from "./components/team/TeamDashboard";
import AiModel from "./components/team/aimodel/AiModel";
import Application from "./components/team/application/Application";
import Wiki from "./components/team/wiki/Wiki";
import Plugin from "./components/team/plugin/Plugin";
import Setting from "./components/team/setting/Setting";
import TeamMember from "./components/team/setting/TeamMember";
import WikiList from "./components/team/wiki/WikiList";
import WikiDocument from "./components/team/wiki/WikiDocument";
import WikiSetting from "./components/team/wiki/WikiSetting";
import WikiEmbedding from "./components/team/wiki/WikiEmbedding";
import TeamSetting from "./components/team/setting/TeamSetting";
import TeamAdmin from "./components/team/setting/TeamAdmin";

const { Sider, Content, Footer } = Layout;

function App() {
  const [messageApi, contextHolder] = message.useMessage();
  const navigate = useNavigate();
  const location = useLocation();

  // 获取当前选中的菜单项
  const getSelectedKeys = () => {
    const path = location.pathname;
    if (path.includes("/team/")) {
      const teamId = path.split("/")[3];
      const subPath = path.split("/")[4];
      if (subPath) {
        return [`3-${getSubMenuKey(subPath)}`];
      }
      return ["3"];
    }
    if (path.includes("/teamlist")) return ["2"];
    if (path.includes("/note")) return ["4"];
    if (path.includes("/user")) return ["8"];
    return ["1"];
  };

  // 获取子菜单的 key
  const getSubMenuKey = (path: string) => {
    switch (path) {
      case "dashboard":
        return "0";
      case "aimodel":
        return "1";
      case "application":
        return "2";
      case "wikilist":
        return "3";
      case "wiki":
        return "3";
      case "plugin":
        return "4";
      case "setting":
        return "5";
      default:
        return "1";
    }
  };

  // 获取当前展开的子菜单
  const getOpenKeys = () => {
    const path = location.pathname;
    if (path.includes("/team/")) {
      return ["3"];
    }
    return [];
  };

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
    {
      key: "1",
      icon: <HomeOutlined />,
      label: <Link to="/app/index">首页</Link>,
    },
    {
      key: "2",
      icon: <AppstoreOutlined />,
      label: <Link to="/app/teamlist">团队列表</Link>,
    },
    {
      key: "3",
      icon: <TeamOutlined />,
      label: "团队",
      children: [
        {
          key: "3-0",
          icon: <DashboardOutlined />,
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/dashboard`}>
              仪表盘
            </Link>
          ),
        },
        {
          key: "3-1",
          icon: <RobotOutlined />,
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/aimodel`}>
              模型
            </Link>
          ),
        },
        {
          key: "3-2",
          icon: <AppstoreAddOutlined />,
          label: (
            <Link
              to={`/app/team/${location.pathname.split("/")[3]}/application`}
            >
              应用
            </Link>
          ),
        },
        {
          key: "3-3",
          icon: <BookOutlined />,
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/wikilist`}>
              知识库
            </Link>
          ),
        },
        {
          key: "3-4",
          icon: <ApiOutlined />,
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/plugin`}>
              插件
            </Link>
          ),
        },
        {
          key: "3-5",
          icon: <SettingOutlined />,
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/setting`}>
              设置
            </Link>
          ),
        },
      ],
    },
    {
      key: "4",
      icon: <FileTextOutlined />,
      label: <Link to="/app/note">笔记系统</Link>,
    },
    {
      key: "8",
      icon: <UserOutlined />,
      label: <Link to="/app/user">个人中心</Link>,
    },
  ];

  return (
    <>
      {contextHolder}
      <Layout className="layout">
        <Header className="header" logo={<Image src="./logo.png" width={60} height={60} />}>
        </Header>
        <Layout>
          <Sider width={200} className="sider" collapsible>
            <Menu
              mode="inline"
              selectedKeys={getSelectedKeys()}
              defaultOpenKeys={getOpenKeys()}
              className="menu-inline"
              items={inlineMenuItems}
            />
          </Sider>
          <Layout style={{ padding: "0 0px 0px" }}>
            <Content className="content">
              <Routes>
                <Route index element={<Dashboard />} />
                <Route path="index" element={<Dashboard />} />
                <Route path="user" element={<User />} />
                <Route path="teamlist" element={<TeamList />} />
                <Route path="team/:teamId/*" element={<Team />}>
                  <Route path="dashboard" element={<TeamDashboard />} />
                  <Route path="aimodel" element={<AiModel />} />
                  <Route path="application" element={<Application />} />
                  <Route path="wiki/:wikiId/*" element={<Wiki />}>
                    <Route path="setting" element={<WikiSetting />} />
                    <Route path="document" element={<WikiDocument />} />
                    <Route path="embedding" element={<WikiEmbedding />} />
                    <Route path="*" element={<WikiSetting />} />
                  </Route>
                  <Route path="wikilist" element={<WikiList />} />
                  <Route path="plugin" element={<Plugin />} />
                  <Route path="setting" element={<Setting />}>
                    <Route path="admin" element={<TeamAdmin />} />
                    <Route path="team" element={<TeamSetting />} />
                    <Route path="member" element={<TeamMember />} />
                    <Route path="*" element={<TeamSetting />} />
                  </Route>
                </Route>
                <Route path="note" element={<Note />} />
              </Routes>
            </Content>
            <Footer className="footer">MaomiAI ©2025</Footer>
          </Layout>
        </Layout>
      </Layout>
    </>
  );
}

export default App;
