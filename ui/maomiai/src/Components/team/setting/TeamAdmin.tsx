import { useState, useEffect } from "react";
import {
  Card,
  Input,
  Button,
  List,
  Avatar,
  message,
  Modal,
  Table,
  Space,
} from "antd";
import { SearchOutlined, PlusOutlined } from "@ant-design/icons";
import { TeamMemberResponse } from "../../../apiClient/models";
import { GetApiClient } from "../../ServiceClient";
import { useNavigate, useParams } from "react-router";

export default function TeamAdmin() {
  const { teamId } = useParams();
  const navigate = useNavigate();
  const [admins, setAdmins] = useState<TeamMemberResponse[]>([]);
  const [members, setMembers] = useState<TeamMemberResponse[]>([]);
  const [searchKeyword, setSearchKeyword] = useState("");
  const [isSelectModalVisible, setIsSelectModalVisible] = useState(false);
  const [selectedMember, setSelectedMember] = useState<string | null>(null);
  const apiClient = GetApiClient();
  const [messageApi, contextHolder] = message.useMessage();

  // Fetch admin list
  const fetchAdminList = async () => {
    if (!teamId) {
      messageApi.error("团队ID不存在");
      return;
    }
    try {
      const response = await apiClient.api.team
        .byTeamId(teamId)
        .adminlist.get();
      if (response) {
        setAdmins(response);
      }
    } catch (error) {
      console.error("Failed to fetch admin list:", error);
      message.error("获取管理员列表失败");
    }
  };

  // Fetch member list
  const fetchMemberList = async () => {
    if (!teamId) {
      messageApi.error("团队ID不存在");
      return;
    }
    try {
      const response = await apiClient.api.team
        .byTeamId(teamId)
        .memberlist.post({
          pageNo: 1,
          pageSize: 100,
        });
      if (response) {
        setMembers(response.items || []);
      }
    } catch (error) {
      console.error("Failed to fetch member list:", error);
      message.error("获取成员列表失败");
    }
  };

  useEffect(() => {
    fetchAdminList();
  }, [teamId]);

  // Handle admin status change
  const handleAdminStatusChange = async (
    userId: string,
    isAdmin: boolean
  ) => {
    if (!teamId) {
      messageApi.error("团队ID不存在");
      return;
    }

    try {
      await apiClient.api.team.byTeamId(teamId).setadmin.post({
        userId: userId,
        isAdmin: isAdmin,
      });
      message.success(isAdmin ? "已设置为管理员" : "已取消管理员权限");
      fetchAdminList();
    } catch (error) {
      console.error("Failed to update admin status:", error);
      message.error("更新管理员状态失败");
    }
  };

  // Handle set admin
  const handleSetAdmin = async () => {
    if (!selectedMember) {
      messageApi.warning("请选择要设置为管理员的成员");
      return;
    }
    
    if (!teamId) {
      messageApi.error("团队ID不存在");
      return;
    }

    try {
      await apiClient.api.team.byTeamId(teamId).setadmin.post({
        userId: selectedMember,
        isAdmin: true,
      });
      messageApi.success("已设置为管理员");
      setIsSelectModalVisible(false);
      setSelectedMember(null);
      fetchAdminList();
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

  const memberColumns = [
    {
      title: "用户",
      dataIndex: "userName",
      key: "userName",
      render: (text: string, record: TeamMemberResponse) => (
        <Space>
          <Avatar src={record.userAvatarPath} />
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
      title: "操作",
      key: "action",
      render: (record: TeamMemberResponse) => (
        <Button
          type="link"
          onClick={() => setSelectedMember(record.userId)}
          disabled={record.isAdmin || record.isOwner}
        >
          设为管理员
        </Button>
      ),
    },
  ];

  return (
    <>
      {contextHolder}
      <Card title="管理员管理">
        <div style={{ marginBottom: 16 }}>
          <Input
            placeholder="搜索团队成员"
            prefix={<SearchOutlined />}
            value={searchKeyword}
            onChange={(e) => setSearchKeyword(e.target.value)}
            style={{ width: 200, marginRight: 16 }}
          />
          <Button
            type="primary"
            icon={<PlusOutlined />}
            onClick={() => {
              setIsSelectModalVisible(true);
              fetchMemberList();
            }}
          >
            添加管理员
          </Button>
        </div>
        <List
          dataSource={admins}
          renderItem={(item) => (
            <List.Item
              actions={[
                <Button
                  type="link"
                  onClick={() =>
                    handleAdminStatusChange(item.userId!, !item.isAdmin)
                  }
                >
                  {item.isAdmin ? "取消管理员" : "设为管理员"}
                </Button>,
              ]}
            >
              <List.Item.Meta
                avatar={<Avatar src={item.userAvatarPath} />}
                title={item.userName}
                description={item.nickName}
              />
            </List.Item>
          )}
        />

        <Modal
          title="选择成员设置为管理员"
          open={isSelectModalVisible}
          onOk={handleSetAdmin}
          onCancel={() => {
            setIsSelectModalVisible(false);
            setSelectedMember(null);
          }}
          width={800}
        >
          <Table
            columns={memberColumns}
            dataSource={members}
            rowKey="userId"
            rowSelection={{
              type: "radio",
              selectedRowKeys: selectedMember ? [selectedMember] : [],
              onChange: (selectedRowKeys) => {
                if (selectedRowKeys.length > 0) {
                  setSelectedMember(selectedRowKeys[0] as string);
                } else {
                  setSelectedMember(null);
                }
              },
            }}
          />
        </Modal>
      </Card>
    </>
  );
}
