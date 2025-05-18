import { useEffect, useState } from "react";
import { useParams, useNavigate, Outlet } from "react-router";
import {
  Spin,
  Layout,
  message,
  Avatar,
  Typography,
  Tag,
  Modal,
  Input,
  Card,
  Space,
} from "antd";
import { GetApiClient } from "../ServiceClient";
import { QueryTeamSimpleCommandResponse } from "../../apiClient/models";
import { TeamOutlined } from "@ant-design/icons";
import { MaomiClient } from "../../apiClient/maomiClient";
import TeamList from "../teamlist/TeamList";

const { Content } = Layout;
const { Title } = Typography;
const { Search } = Input;

export default function Team() {
  const { teamId } = useParams();
  const navigate = useNavigate();
  const [messageApi, contextHolder] = message.useMessage();

  const [loadingTeams, setLoadingTeams] = useState(false);

  // Current team state
  const [currentTeam, setCurrentTeam] =
    useState<QueryTeamSimpleCommandResponse>({});

  // Fetch teams for the selection modal
  const fetchTeams = async (teamId: string, client?: MaomiClient) => {
    if (!client) {
      client = GetApiClient();
    }

    try {
      setLoadingTeams(true);
      const response = await client.api.team.byId(teamId).teamitem.get();
      if (response) {
        setCurrentTeam(response);
      }
    } catch (error) {
      console.error("Failed to fetch teams:", error);
      messageApi.error("获取团队列表失败");
    } finally {
      setLoadingTeams(false);
    }
  };

  // Handle team loading and search
  useEffect(() => {
    if (!teamId || teamId === "undefined") {
      messageApi.error("请先选择团队");
    } else {
      const client = GetApiClient();
      const fetchData = async () => {
        await fetchTeams(teamId, client);
      };
      fetchData();
    }
  }, [teamId]);

  // If no valid team is selected, show TeamList
  if (!teamId || teamId === "undefined") {
    return (
      <>
        {contextHolder}
        <TeamList />
      </>
    );
  }

  return (
    <>
      {contextHolder}
      <Layout style={{ minHeight: "100vh" }}>
        <div
          style={{
            padding: "16px",
            display: "flex",
            alignItems: "center",
            gap: "12px",
          }}
        >
          <Avatar size={40} src={currentTeam.avatarUrl} />
          <Title level={4} style={{ margin: 0 }}>
            {currentTeam.name}
          </Title>
          <div style={{ display: "flex", gap: "8px" }}>
            {currentTeam.isRoot && <Tag color="gold">所有者</Tag>}
            {currentTeam.isAdmin && <Tag color="blue">管理员</Tag>}
            {currentTeam.isPublic && <Tag color="green">公开</Tag>}
          </div>
        </div>
        <Content style={{ padding: "24px" }}>
          <Outlet />
        </Content>
      </Layout>
    </>
  );
}
