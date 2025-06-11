import { useEffect, useState } from "react";
import { Route, Routes, useNavigate, Link, useLocation } from "react-router";
import { useSelector, useDispatch } from "react-redux";
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
  Flex,
  Typography,
  Dropdown,
  Button,
} from "antd";
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
  DownOutlined,
} from "@ant-design/icons";
import "./App.css";
import { CheckToken, RefreshServerInfo } from "./InitPage";
import { GetApiClient } from "./components/ServiceClient";
import { setCurrentTeam } from "./stateshare/actions";
import { QueryTeamSimpleCommandResponse } from "./apiClient/models";

import TeamList from "./components/teamlist/TeamList";
import User from "./components/user/User";
import Dashboard from "./components/dashboard/Dashboard";
import Team from "./components/team/Team";
import Note from "./components/userapplication/note/Note";
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
import WikiEmbeddingTest from "./components/team/wiki/WikiEmbeddingTest";
import TeamPrompt from "./components/team/prompt/TeamPrompt";
import  UserChat  from "./components/userapplication/chat/Chat";

const { Sider, Content, Footer } = Layout;
const { Title } = Typography;

// 团队显示组件
function TeamDisplay() {
  const currentTeam = useSelector((state: any) => state.currentTeam);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [teams, setTeams] = useState<QueryTeamSimpleCommandResponse[]>([]);
  const [loading, setLoading] = useState(false);

  // 获取团队列表
  const fetchTeams = async () => {
    try {
      setLoading(true);
      const client = GetApiClient();
      const response = await client.api.team.joined_list.post({
        pageNo: 1,
        pageSize: 100,
      });
      if (response) {
        setTeams(response.items || []);
      }
    } catch (error) {
      console.error("获取团队列表失败:", error);
    } finally {
      setLoading(false);
    }
  };

  // 切换团队
  const handleTeamSwitch = async (teamId: string) => {
    try {
      const client = GetApiClient();
      const response = await client.api.team.byTeamId(teamId).teamitem.get();
      if (response) {
        dispatch(setCurrentTeam(response));
        navigate(`/app/team/${teamId}/dashboard`);
      }
    } catch (error) {
      console.error("切换团队失败:", error);
    }
  };

  useEffect(() => {
    if (currentTeam) {
      fetchTeams();
    }
  }, [currentTeam]);

  if (!currentTeam) {
    return null;
  }

  const menuItems = teams
    .filter((team) => team.id) // 过滤掉没有ID的团队
    .map((team) => ({
      key: team.id!,
      label: (
        <Space>
          <Avatar size="small" src={team.avatarUrl} />
          <span>{team.name}</span>
          <div style={{ display: "flex", gap: "4px" }}>
            {team.isRoot && <Tag color="gold">所有者</Tag>}
            {team.isAdmin && <Tag color="blue">管理员</Tag>}
            {team.isPublic && <Tag color="green">公开</Tag>}
          </div>
        </Space>
      ),
    }));

  const handleMenuClick = ({ key }: { key: string }) => {
    handleTeamSwitch(key);
  };

  return (
    <Dropdown
      menu={{ items: menuItems, onClick: handleMenuClick }}
      placement="bottomRight"
      trigger={["click"]}
      disabled={loading}
    >
      <Button
        type="text"
        style={{ color: "inherit", height: "auto", padding: "4px 8px" }}
      >
        <Flex gap="small" justify="flex-start" align="center" vertical={false}>
          <Avatar size={32} src={currentTeam.avatarUrl} />
          <Title level={5} style={{ margin: 0 }}>
            {currentTeam.name}
          </Title>
          <div style={{ display: "flex", gap: "4px" }}>
            {currentTeam.isRoot && <Tag color="gold">所有者</Tag>}
            {currentTeam.isAdmin && <Tag color="blue">管理员</Tag>}
            {currentTeam.isPublic && <Tag color="green">公开</Tag>}
          </div>
          <DownOutlined style={{ fontSize: "12px", marginLeft: "4px" }} />
        </Flex>
      </Button>
    </Dropdown>
  );
}

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
      case "prompt":
        return "5";
      case "setting":
        return "10";
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
          icon: <ApiOutlined />,
          label: (
            <Link to={`/app/team/${location.pathname.split("/")[3]}/prompt`}>
              提示词
            </Link>
          ),
        },
        {
          key: "3-10",
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
      label: <>个人应用</>,
      children: [
        {
          key: "4-0",
          icon: <FileTextOutlined />,
          label: <Link to="/app/userapp/note">笔记</Link>,
        },
        {
          key: "4-1",
          icon: <FileTextOutlined />,
          label: <Link to="/app/userapp/chat">AI助手</Link>,
        },
      ],
    },
    {
      key: "5",
      icon: <FileTextOutlined />,
      label: <>系统应用</>,
      children: [
        {
          key: "5-0",
          icon: <FileTextOutlined />,
          label: <Link to="/app/systemapp/note">笔记</Link>,
        },
      ],
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
        <Layout.Header
          className="header"
          style={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            backgroundColor: "white",
          }}
        >
          <Image src="/logo.png" width={60} height={60} />
          <TeamDisplay />
        </Layout.Header>
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
                  <Route path="prompt" element={<TeamPrompt />} />
                  <Route path="wiki/:wikiId/*" element={<Wiki />}>
                    <Route path="setting" element={<WikiSetting />} />
                    <Route path="document" element={<WikiDocument />} />
                    <Route path="embedding" element={<WikiEmbedding />} />
                    <Route
                      path="embeddingtest"
                      element={<WikiEmbeddingTest />}
                    />
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
                <Route path="userapp">
                  <Route path="note" element={<Note />} />
                  <Route path="chat" element={<UserChat />} />
                </Route>
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
