import { useEffect, useState } from "react";
import { Route, Routes, useNavigate, Link, useLocation } from "react-router";
import { Layout, Menu, message, Modal, Card, Avatar, Space, Tag, Spin } from "antd";
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
import AiModel from "./Components/team/aimodel/AiModel";
import Application from "./Components/team/application/Application";
import Wiki from "./Components/team/wiki/Wiki";
import Plugin from "./Components/team/plugin/Plugin";

const { Sider, Content, Footer } = Layout;

function App() {
  const [messageApi, contextHolder] = message.useMessage();
  const navigate = useNavigate();
  const location = useLocation();
  const [isTeamSelectModalVisible, setIsTeamSelectModalVisible] = useState(false);
  const [teams, setTeams] = useState<MaomiAITeamSharedQueriesResponsesQueryTeamSimpleCommandResponse[]>([]);
  const [loadingTeams, setLoadingTeams] = useState(false);
  const [selectedTeamPath, setSelectedTeamPath] = useState("");

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

  const fetchJoinedTeams = async () => {
    setLoadingTeams(true);
    try {
      const client = GetApiClient();
      const response = await client.api.team.joined_list.post({
        pageNo: 1,
        pageSize: 100
      });
      if (response) {
        setTeams(response.items || []);
      }
    } catch (error) {
      console.error("Failed to fetch teams:", error);
      messageApi.error("获取团队列表失败");
    } finally {
      setLoadingTeams(false);
    }
  };

  const handleTeamMenuClick = (path: string) => {
    const currentPath = location.pathname;
    const teamIdMatch = currentPath.match(/\/team\/([^\/]+)/);
    
    if (!teamIdMatch) {
      messageApi.error("请先选择团队");
      navigate("/app/teamlist");
      return;
    }

    const teamId = teamIdMatch[1];
    navigate(`/app/team/${teamId}${path}`);
  };

  const handleTeamSelect = (teamId: string | null | undefined) => {
    if (!teamId) return;
    setIsTeamSelectModalVisible(false);
    navigate(`/app/team/${teamId}${selectedTeamPath}`);
  };

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
        { key: "3-1", label: <Link to={`/app/team/${location.pathname.split('/')[3]}/aimodel`}>模型</Link> },
        { key: "3-2", label: <Link to={`/app/team/${location.pathname.split('/')[3]}/application`}>应用</Link> },
        { key: "3-3", label: <Link to={`/app/team/${location.pathname.split('/')[3]}/wiki`}>知识库</Link> },
        { key: "3-4", label: <Link to={`/app/team/${location.pathname.split('/')[3]}/plugin`}>插件</Link> }
      ]
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
            defaultSelectedKeys={["1"]}
            className="menu-inline"
            items={inlineMenuItems}
          />
        </Sider>
        <Layout style={{ padding: "0 24px 24px" }}>
          <Content className="content">
            <Routes>
              <Route index element={<Dashboard />} />
              <Route path="index" element={<Dashboard />} />
              <Route path="user" element={<User />} />
              <Route path="teamlist" element={<TeamList />} />
              <Route path="team/:teamId/*" element={<Team />}>
                <Route path="aimodel" element={<AiModel />} />
                <Route path="application" element={<Application />} />
                <Route path="wiki" element={<Wiki />} />
                <Route path="plugin" element={<Plugin />} />
              </Route>
              <Route path="note" element={<Note />} />
            </Routes>
          </Content>
          <Footer className="footer">
            MaomiAI ©2025
          </Footer>
        </Layout>
      </Layout>

      <Modal
        title="选择团队"
        open={isTeamSelectModalVisible}
        onCancel={() => setIsTeamSelectModalVisible(false)}
        footer={null}
        width={800}
      >
        <Spin spinning={loadingTeams}>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(250px, 1fr))', gap: '16px' }}>
            {teams.map((team) => (
              <Card
                key={team.id}
                hoverable
                onClick={() => handleTeamSelect(team.id)}
                bodyStyle={{ padding: "12px" }}
              >
                <Space direction="vertical" size="small" style={{ width: "100%" }}>
                  <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                    <Space>
                      <Avatar size={40} src={team.avatarUrl} icon={<TeamOutlined />} />
                      <h3 style={{ margin: 0, fontSize: "16px" }}>{team.name}</h3>
                    </Space>
                    <Space size="small">
                      {team.isRoot && <Tag color="gold">所有者</Tag>}
                      {team.isAdmin && <Tag color="blue">管理员</Tag>}
                      {team.isPublic && <Tag color="green">公开</Tag>}
                    </Space>
                  </div>
                  <p style={{ margin: "8px 0", color: "#666", fontSize: "13px" }}>
                    {team.description}
                  </p>
                </Space>
              </Card>
            ))}
          </div>
        </Spin>
      </Modal>
    </Layout>
  );
}

export default App;
