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
import useAppStore from "../../stateshare/store";
import "./Team.css";

const { Content } = Layout;
const { Title } = Typography;
const { Search } = Input;
const { Option } = Select;

export default function Team() {
  const { teamId } = useParams();
  const navigate = useNavigate();
  const [messageApi, contextHolder] = message.useMessage();

  // Get current team from global state
  const { currentTeam: globalCurrentTeam, setCurrentTeam: setCurrentTeamAction } = useAppStore();

  const [loadingTeams, setLoadingTeams] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [teams, setTeams] = useState<QueryTeamSimpleCommandResponse[]>([]);
  const [selectedTeamId, setSelectedTeamId] = useState<string>();

  // Current team state
  const [currentTeam, setCurrentTeam] =
    useState<QueryTeamSimpleCommandResponse>({});

  // Check if team fetch resulted in 404 error
  const [teamNotFound, setTeamNotFound] = useState(false);

  // Fetch teams for the selection modal
  const fetchTeams = async (teamId: string, client?: MaomiClient) => {
    if (!client) {
      client = GetApiClient();
    }

    try {
      setLoadingTeams(true);
      setTeamNotFound(false);
      const response = await client.api.team.byTeamId(teamId).teamitem.get();
      if (response) {
        setCurrentTeam(response);
        setCurrentTeamAction(response);
      }
    } catch (error: any) {
      // Check if it's a 404 error
      if (error?.status === 404 || error?.response?.status === 404) {
        setTeamNotFound(true);
        messageApi.error("团队不存在或您没有访问权限，请重新选择团队");
        // Clear the invalid team from global state
        setCurrentTeamAction(null);
        setIsModalVisible(true);
        fetchTeamsForSelection();
      } else {
        messageApi.error("获取团队信息失败");
        proxyRequestError(error, messageApi);
      }
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
      setIsModalVisible(false);
    } else {
      messageApi.warning("请选择一个团队");
    }
  };

  // Handle team loading and search
  useEffect(() => {
    // Case 1: No teamId in URL or teamId is "undefined"
    if (!teamId || teamId === "undefined") {
      // If we have a global team, redirect to it
      if (globalCurrentTeam && globalCurrentTeam.id) {
        navigate(`/app/team/${globalCurrentTeam.id}/dashboard`);
        return;
      }
      // Otherwise, show team selection modal
      setIsModalVisible(true);
      fetchTeamsForSelection();
      return;
    }

    // Case 2: We have a teamId in URL
    // If global team exists and matches URL teamId, use it directly
    if (globalCurrentTeam && globalCurrentTeam.id === teamId) {
      setCurrentTeam(globalCurrentTeam);
      return;
    }

    // Case 3: No global team or global team doesn't match URL teamId
    // Fetch team info from API
    const client = GetApiClient();
    const fetchData = async () => {
      await fetchTeams(teamId, client);
    };
    fetchData();
  }, [teamId]); // Remove globalCurrentTeam from dependencies to avoid infinite loop

  // Handle global team state changes
  useEffect(() => {
    if (globalCurrentTeam && globalCurrentTeam.id && teamId && teamId === globalCurrentTeam.id) {
      setCurrentTeam(globalCurrentTeam);
      setIsModalVisible(false);
      setTeamNotFound(false);
    }
  }, [globalCurrentTeam, teamId]);

  // If team was not found (404), or no valid team is selected, show selection modal
  if (teamNotFound || !teamId || teamId === "undefined") {
    return (
      <>
        {contextHolder}
        <Modal
          title="选择团队"
          open={isModalVisible}
          onOk={handleModalConfirm}
          onCancel={() => {
            setIsModalVisible(false);
            // If user cancels and there's no valid team, go back to team list
            if (!globalCurrentTeam || !globalCurrentTeam.id) {
              navigate('/app/teamlist');
            }
          }}
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
        <Outlet />
      </Layout>
    </>
  );
}
