import { useState, useEffect } from "react";
import { Button, Input, Table, Modal, message, Space, Avatar, Popconfirm } from "antd";
import { PlusOutlined, UserOutlined, DeleteOutlined, CrownOutlined } from "@ant-design/icons";
import { useParams } from "react-router";
import { GetApiClient } from "../../ServiceClient";
import type { MaomiAITeamSharedQueriesResponsesTeamMemberResponse } from "../../../ApiClient/models";

export default function TeamMember() {
  const { teamId } = useParams();
  const [members, setMembers] = useState<
    MaomiAITeamSharedQueriesResponsesTeamMemberResponse[]
  >([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [isInviteModalVisible, setIsInviteModalVisible] = useState(false);
  const [inviteUsername, setInviteUsername] = useState("");
  const [messageApi, contextHolder] = message.useMessage();

  const fetchMembers = async (page: number, size: number) => {
    if (!teamId) {
      messageApi.error("团队ID不存在");
      return;
    }
    setLoading(true);
    try {
      const client = GetApiClient();

      const response = await client.api.team.byId(teamId).memberlist.post({
        pageNo: page,
        pageSize: size,
      });
      if (response) {
        setMembers(response.items || []);
        setTotal(response.total || 0);
      }
    } catch (error) {
      message.error("获取团队成员列表失败");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchMembers(currentPage, pageSize);
  }, [teamId, currentPage, pageSize]);

  const handleInvite = async () => {
    if (!inviteUsername.trim()) {
      messageApi.warning("请输入用户名");
      return;
    }

    try {
      const client = GetApiClient();
      await client.api.team.member.invite.post({
        teamId: teamId,
        userName: inviteUsername,
      });
      messageApi.success("邀请成功");
      setIsInviteModalVisible(false);
      setInviteUsername("");
      fetchMembers(currentPage, pageSize);
    } catch (error) {
      console.error("upload file error:", error);
      const typedError = error as {
        detail?: string;
      };
      if (typedError.detail) {
        messageApi.error(typedError.detail);
      } else {
        messageApi.error("邀请失败");
      }

      return;
    }
  };

  const handleRemoveMember = async (userId: string | null | undefined) => {
    if (!teamId) {
      messageApi.error("团队ID不存在");
      return;
    }

    if (!userId) {
      messageApi.error("用户ID不存在");
      return;
    }

    try {
      const client = GetApiClient();
      await client.api.team.member.remove.post({
        teamId: teamId,
        userId: userId as string,
      });
      messageApi.success("移除成员成功");
      fetchMembers(currentPage, pageSize);
    } catch (error) {
      console.error("remove member error:", error);
      const typedError = error as {
        detail?: string;
      };
      if (typedError.detail) {
        messageApi.error(typedError.detail);
      } else {
        messageApi.error("移除成员失败");
      }
    }
  };

  const handleSetAdmin = async (userId: string | null | undefined, isAdmin: boolean) => {
    if (!teamId) {
      messageApi.error("团队ID不存在");
      return;
    }

    if (!userId) {
      messageApi.error("用户ID不存在");
      return;
    }

    try {
      const client = GetApiClient();
      await client.api.team.setadmin.post({
        teamId: teamId,
        userId: userId,
        isAdmin: isAdmin,
      });
      messageApi.success(isAdmin ? "已设置为管理员" : "已取消管理员权限");
      fetchMembers(currentPage, pageSize);
    } catch (error) {
      console.error("set admin error:", error);
      const typedError = error as {
        detail?: string;
      };
      if (typedError.detail) {
        messageApi.error(typedError.detail);
      } else {
        messageApi.error("设置管理员失败");
      }
    }
  };

  const columns = [
    {
      title: "用户",
      dataIndex: "userName",
      key: "userName",
      render: (
        text: string,
        record: MaomiAITeamSharedQueriesResponsesTeamMemberResponse
      ) => (
        <Space>
          <Avatar src={record.userAvatarPath} icon={<UserOutlined />} />
          <span>{text}</span>
        </Space>
      ),
    },
    {
      title: "昵称",
      dataIndex: "nickName",
      key: "nickName",
    },
    {
      title: "角色",
      key: "role",
      render: (record: MaomiAITeamSharedQueriesResponsesTeamMemberResponse) => (
        <span>
          {record.isOwner ? "所有者" : record.isAdmin ? "管理员" : "成员"}
        </span>
      ),
    },
    {
      title: "操作",
      key: "action",
      render: (record: MaomiAITeamSharedQueriesResponsesTeamMemberResponse) => (
        <Space>
          {!record.isOwner && (
            <>
              <Popconfirm
                title="确定要移除该成员吗？"
                onConfirm={() => handleRemoveMember(record.userId)}
                okText="确定"
                cancelText="取消"
              >
                <Button 
                  type="text" 
                  danger 
                  icon={<DeleteOutlined />}
                >
                  移除
                </Button>
              </Popconfirm>
              <Popconfirm
                title={`确定要${record.isAdmin ? '取消' : '设置'}该成员为管理员吗？`}
                onConfirm={() => handleSetAdmin(record.userId, !record.isAdmin)}
                okText="确定"
                cancelText="取消"
              >
                <Button 
                  type="text" 
                  icon={<CrownOutlined />}
                >
                  {record.isAdmin ? "取消管理员" : "设为管理员"}
                </Button>
              </Popconfirm>
            </>
          )}
        </Space>
      ),
    },
  ];

  return (
    <>
      {contextHolder}
      <div className="p-4">
        <div className="mb-4 flex justify-between items-center">
          <h2 className="text-xl font-bold">团队成员</h2>
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => setIsInviteModalVisible(true)}
          >
            邀请成员
          </Button>
        </div>

        <Table
          columns={columns}
          dataSource={members}
          rowKey="userId"
          loading={loading}
          pagination={{
            current: currentPage,
            pageSize: pageSize,
            total: total,
            onChange: (page, size) => {
              setCurrentPage(page);
              setPageSize(size);
            },
          }}
        />

        <Modal
          title="邀请成员"
          open={isInviteModalVisible}
          onOk={handleInvite}
          onCancel={() => {
            setIsInviteModalVisible(false);
            setInviteUsername("");
          }}
        >
          <Input
            placeholder="请输入用户名"
            value={inviteUsername}
            onChange={(e) => setInviteUsername(e.target.value)}
          />
        </Modal>
      </div>
    </>
  );
}
