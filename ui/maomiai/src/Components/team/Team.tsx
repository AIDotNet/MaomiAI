import { useEffect, useState } from "react";
import { useParams, useNavigate, Outlet } from "react-router";
import { Spin, Layout, Menu, message } from "antd";
import { GetApiClient } from "../ServiceClient";
import { MaomiAITeamSharedQueriesResponsesQueryTeamSimpleCommandResponse } from "../../ApiClient/models";

const { Sider, Content } = Layout;

export default function Team() {
  const { teamId } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [team, setTeam] =
    useState<MaomiAITeamSharedQueriesResponsesQueryTeamSimpleCommandResponse | null>(
      null
    );
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    const fetchTeamInfo = async () => {
      if (!teamId || teamId == "undefined") {
        messageApi.error("请先选择团队");
        navigate("/app/teamlist");
        return;
      }

      setLoading(true);
      try {
        const client = GetApiClient();
        const response = await client.api.team.byId(teamId).teamitem.get();
        if (response) {
          setTeam(response);
        }
      } catch (error) {
        console.error("Failed to fetch team info:", error);
        const typedError = error as {
          detail?: string;
        };
        if (typedError.detail) {
          messageApi.error(typedError.detail);
        } else {
          messageApi.error("获取团队信息失败");
        }
        navigate("/app/teamlist");
      } finally {
        setLoading(false);
      }
    };

    fetchTeamInfo();
  }, [teamId, navigate, messageApi]);

  const handleMenuClick = (key: string) => {
    navigate(`/app/team/${teamId}/${key}`);
  };

  if (loading) {
    return <Spin size="large" />;
  }

  if (!team) {
    return null;
  }

  return (
    <>
      {contextHolder}
      <Layout style={{ minHeight: "100vh" }}>
        <p>团队</p>
        <Content style={{ padding: "24px" }}>
          <Outlet />
        </Content>
      </Layout>
    </>
  );
}
