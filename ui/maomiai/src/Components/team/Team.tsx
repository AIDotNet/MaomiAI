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
  Flex,
  Select,
} from "antd";
import { GetApiClient } from "../ServiceClient";
import { QueryTeamSimpleCommandResponse } from "../../apiClient/models";
import { MaomiClient } from "../../apiClient/maomiClient";
import TeamList from "../teamlist/TeamList";
import { proxyRequestError } from "../../helper/RequestError";
import { Header } from "antd/es/layout/layout";
import Meta from "antd/es/card/Meta";
import "./Team.css";

const { Content } = Layout;
const { Title } = Typography;
const { Search } = Input;
const { Option } = Select;

export default function Team() {
  const { teamId } = useParams();
  const navigate = useNavigate();
  const [messageApi, contextHolder] = message.useMessage();

  const [loadingTeams, setLoadingTeams] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [teams, setTeams] = useState<QueryTeamSimpleCommandResponse[]>([]);
  const [selectedTeamId, setSelectedTeamId] = useState<string>();

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
      const response = await client.api.team.byTeamId(teamId).teamitem.get();
      if (response) {
        setCurrentTeam(response);
      }
    } catch (error) {
      messageApi.error("获取团队列表失败");
      proxyRequestError(error, messageApi);
    } finally {
      setLoadingTeams(false);
    }
  };

  // Fetch teams for selection
  const fetchTeamsForSelection = async () => {
    try {
      setLoadingTeams(true);
      const client = GetApiClient();
      const response = await client.api.team.joined_list.post({
        pageNo: 1,
        pageSize: 100,
      });
      if (response) {
        setTeams(response.items || []);
      }
    } catch (error) {
      messageApi.error("获取团队列表失败");
      proxyRequestError(error, messageApi);
    } finally {
      setLoadingTeams(false);
    }
  };

  // Handle team selection
  const handleTeamSelect = (value: string) => {
    setSelectedTeamId(value);
  };

  // Handle modal confirm
  const handleModalConfirm = () => {
    if (selectedTeamId) {
      navigate(`/app/team/${selectedTeamId}/dashboard`);
    } else {
      messageApi.warning("请选择一个团队");
    }
  };

  // Handle team loading and search
  useEffect(() => {
    if (!teamId || teamId === "undefined") {
      setIsModalVisible(true);
      fetchTeamsForSelection();
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
        <Modal
          title="选择团队"
          open={isModalVisible}
          onOk={handleModalConfirm}
          onCancel={() => setIsModalVisible(false)}
          okText="确认"
          cancelText="取消"
        >
          <Select
            style={{ width: "100%" }}
            placeholder="请选择团队"
            onChange={handleTeamSelect}
            loading={loadingTeams}
          >
            {teams.map((team) => (
              <Option key={team.id} value={team.id}>
                <Space>
                  <Avatar size="small" src={team.avatarUrl} />
                  <span>{team.name}</span>
                  {team.isRoot && <Tag color="gold">所有者</Tag>}
                  {team.isAdmin && <Tag color="blue">管理员</Tag>}
                  {team.isPublic && <Tag color="green">公开</Tag>}
                </Space>
              </Option>
            ))}
          </Select>
        </Modal>
        <TeamList />
      </>
    );
  }

  return (
    <>
      {contextHolder}
      <Layout style={{ minHeight: "100vh", padding: "0 0px 0px" }}>
        <Header className="header">
          <Flex gap="middle" justify="flex-start" align="center" vertical={false}>
            <Avatar size={40} src={currentTeam.avatarUrl} />
            <Title level={4} style={{ margin: 0 }}>
              {currentTeam.name}
            </Title>
            <div style={{ display: "flex", gap: "8px" }}>
              {currentTeam.isRoot && <Tag color="gold">所有者</Tag>}
              {currentTeam.isAdmin && <Tag color="blue">管理员</Tag>}
              {currentTeam.isPublic && <Tag color="green">公开</Tag>}
            </div>
          </Flex>
        </Header>
        <Content style={{ padding: "12px 24px", backgroundColor: "#fff" }}>
          <Outlet />
        </Content>
      </Layout>
    </>
  );
}
