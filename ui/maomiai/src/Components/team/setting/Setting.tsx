import { useState, useEffect } from "react";
import { Layout, Menu, message } from "antd";
import { UserOutlined, TeamOutlined, SettingOutlined } from "@ant-design/icons";
import {
  Routes,
  Route,
  useNavigate,
  useLocation,
  Link,
  Outlet,
} from "react-router";
import TeamAdmin from "./TeamAdmin";
import TeamSetting from "./TeamSetting";
import { GetApiClient } from "../../ServiceClient";
import { useParams } from "react-router";

const { Sider, Content } = Layout;

export default function Setting() {
  const [teamInfo, setTeamInfo] = useState<any>(null);
  const [messageApi, contextHolder] = message.useMessage();
  const apiClient = GetApiClient();
  const { teamId } = useParams();
  const location = useLocation();
  const navigate = useNavigate();

  // Fetch team info
  const fetchTeamInfo = async () => {
    if (!teamId) {
      messageApi.error("团队ID不存在");
      return;
    }

    try {
      const response = await apiClient.api.team.byId(teamId).teamdetail.get();
      if (response) {
        setTeamInfo(response);
      }
    } catch (error) {
      console.error("Failed to fetch team info:", error);
      messageApi.error("获取团队信息失败");
    }
  };

  useEffect(() => {
    fetchTeamInfo();
  }, [teamId]);

  const handleTeamInfoUpdate = () => {
    fetchTeamInfo();
  };

  // Get the current active menu item based on the route
  const getSelectedKey = () => {
    const path = location.pathname;
    if (path.includes("/member")) return "member";
    if (path.includes("/admin")) return "admin";
    if (path.includes("/settings")) return "settings";
    return "team"; // default
  };

  const menuItems = [
    {
      key: "member",
      icon: <TeamOutlined />,
      label: <Link to={`/app/team/${teamId}/setting/member`}>成员</Link>,
    },
    {
      key: "admin",
      icon: <UserOutlined />,
      label: <Link to={`/app/team/${teamId}/setting/admin`}>管理员</Link>,
    },
    {
      key: "settings",
      icon: <SettingOutlined />,
      label: <Link to={`/app/team/${teamId}/setting/settings`}>团队设置</Link>,
    },
  ];

  return (
    <>
      {contextHolder}
      <Layout style={{ minHeight: "100vh" }}>
        <Sider width={200} theme="light">
          <Menu
            mode="inline"
            selectedKeys={[getSelectedKey()]}
            style={{ height: "100%" }}
            items={menuItems}
          />
        </Sider>
        <Content style={{ padding: "24px" }}>
          <Outlet />
        </Content>
      </Layout>
    </>
  );
}
