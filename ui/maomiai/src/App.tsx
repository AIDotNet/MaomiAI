import { useEffect, useState } from "react";
import { Route, Routes, useNavigate, Link, useLocation } from "react-router";
import {
  Layout,
  Menu,
  message,
  Modal,
  Card,
  Avatar,
  Space,
  Tag,
  Spin,
} from "antd";
import { Header } from "@lobehub/ui";
import { TeamOutlined } from "@ant-design/icons";
import "./App.css";
import { CheckToken, RefreshServerInfo } from "./InitPage";
import { GetApiClient } from "./Components/ServiceClient";
import { MaomiAITeamSharedQueriesResponsesQueryTeamSimpleCommandResponse } from "./ApiClient/models";

import TeamList from "./Components/teamlist/TeamList";
import User from "./Components/user/User";
import Dashboard from "./Components/dashboard/Dashboard";
import Team from "./Components/team/Team";
import Note from "./Components/note/Note";
import TeamDashboard from "./Components/team/TeamDashboard";
import AiModel from "./Components/team/aimodel/AiModel";
import Application from "./Components/team/application/Application";
import Wiki from "./Components/team/wiki/Wiki";
import Plugin from "./Components/team/plugin/Plugin";
import Setting from "./Components/team/setting/Setting";
import TeamAdmin from "./Components/team/setting/TeamAdmin";
import TeamSetting from "./Components/team/setting/TeamSetting";
import TeamMember from "./Components/team/setting/TeamMember";

const { Sider, Content, Footer } = Layout;

function App() {
  const [messageApi, contextHolder] = message.useMessage();
  const navigate = useNavigate();
  const location = useLocation();
  const [isTeamSelectModalVisible, setIsTeamSelectModalVisible] =
    useState(false);
  const [teams, setTeams] = useState<
    MaomiAITeamSharedQueriesResponsesQueryTeamSimpleCommandResponse[]
  >([]);
  const [loadingTeams, setLoadingTeams] = useState(false);
  const [selectedTeamPath, setSelectedTeamPath] = useState("");

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
      case "aimodel":
        return "1";
      case "application":
        return "2";
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
    { key: "1", label: <Link to="/app/index">首页</Link> },
    { key: "2", label: <Link to="/app/teamlist">团队列表</Link> },
    {
      key: "3",
      label: "团队",
      children: [
        {
          key: "3-0",
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/dashboard`}>
              仪表盘
            </Link>
          ),
        },
        {
          key: "3-1",
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/aimodel`}>
              模型
            </Link>
          ),
        },
        {
          key: "3-2",
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
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/wiki`}>
              知识库
            </Link>
          ),
        },
        {
          key: "3-4",
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/plugin`}>
              插件
            </Link>
          ),
        },
        {
          key: "3-5",
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/setting`}>
              设置
            </Link>
          ),
        },
      ],
    },
    { key: "4", label: <Link to="/app/note">笔记系统</Link> },
    { key: "8", label: <Link to="/app/user">个人中心</Link> },
  ];

  return (
    <Layout className="layout">
      {contextHolder}
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
                <Route path="wiki" element={<Wiki />} />
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
  );
}

export default App;
