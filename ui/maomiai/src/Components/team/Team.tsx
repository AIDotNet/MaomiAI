import { useState, useEffect } from "react";
import {
  Tabs,
  Input,
  Select,
  Card,
  Avatar,
  Space,
  Tag,
  Pagination,
  Spin,
  Row,
  Col,
  Button,
  Modal,
  Form,
  message,
} from "antd";
import { SearchOutlined, TeamOutlined, PlusOutlined } from "@ant-design/icons";
import { GetApiClient } from "../ServiceClient";
import type { MaomiAITeamSharedQueriesResponsesTeamSimpleResponse } from "../../ApiClient/models";
import { useNavigate } from "react-router";

const { TabPane } = Tabs;
const { Search } = Input;
const { Option } = Select;

export default function Team() {
  const apiClient = GetApiClient();
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState("1");
  const [loading, setLoading] = useState(false);
  const [teams, setTeams] = useState<
    MaomiAITeamSharedQueriesResponsesTeamSimpleResponse[]
  >([]);
  const [total, setTotal] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  // Search filters
  const [keyword, setKeyword] = useState("");
  const [isOwner, setIsOwner] = useState<boolean | null>(null);
  const [isAdmin, setIsAdmin] = useState<boolean | null>(null);

  // Create team modal
  const [isCreateModalVisible, setIsCreateModalVisible] = useState(false);
  const [createForm] = Form.useForm();
  const [creating, setCreating] = useState(false);
  const [messageApi, contextHolder] = message.useMessage();

  const fetchTeams = async () => {
    setLoading(true);
    try {
      const response = await apiClient.api.team.joined_list.post({
        keyword: keyword,
        isRoot: activeTab === "1" ? isOwner : activeTab === "3" ? true : null,
        isAdmin: activeTab === "1" ? isAdmin : activeTab === "2" ? true : null,
        pageNo: currentPage,
        pageSize: pageSize,
      });

      if (response) {
        setTeams(response.items || []);
        setTotal(parseInt(response.total || "0"));
      }
    } catch (error) {
      console.error("Failed to fetch teams:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTeams();
  }, [activeTab, currentPage, pageSize, keyword, isOwner, isAdmin]);

  const handleTabChange = (key: string) => {
    setActiveTab(key);
    setCurrentPage(1);
    setKeyword("");
    setIsOwner(null);
    setIsAdmin(null);
  };

  const handleSearch = (value: string) => {
    setKeyword(value);
    setCurrentPage(1);
  };

  const handlePageChange = (page: number, pageSize?: number) => {
    setCurrentPage(page);
    if (pageSize) {
      setPageSize(pageSize);
    }
  };

  const handleCreateTeam = async (values: {
    name: string;
    description: string;
  }) => {
    setCreating(true);
    try {
      const response = await apiClient.api.team.create.post({
        name: values.name,
        description: values.description,
      });

      if (response?.id) {
        message.success("团队创建成功");
        setIsCreateModalVisible(false);
        createForm.resetFields();
        // 跳转到团队页面
        navigate(`/app/team/${response.id}/dashboard`);
      }
    } catch (error) {
      console.error("upload file error:", error);
      const typedError = error as {
        detail?: string;
      };
      if (typedError.detail) {
        messageApi.error(typedError.detail || "创建团队失败");
      } else {
        messageApi.error("创建团队失败");
      }
    } finally {
      setCreating(false);
    }
  };

  const renderTeamCard = (
    team: MaomiAITeamSharedQueriesResponsesTeamSimpleResponse
  ) => (
    <>
      {contextHolder}
      <Card
        key={team.id}
        hoverable
        style={{ height: "100%" }}
        bodyStyle={{ padding: "12px" }}
        onClick={() => navigate(`/app/team/${team.id}/dashboard`)}
      >
        <Space direction="vertical" size="small" style={{ width: "100%" }}>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          >
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
          <p
            style={{
              margin: "8px 0",
              color: "#666",
              fontSize: "13px",
              height: "36px",
              overflow: "hidden",
              textOverflow: "ellipsis",
              display: "-webkit-box",
              WebkitLineClamp: 2,
              WebkitBoxOrient: "vertical",
            }}
          >
            {team.description}
          </p>
          <div style={{ color: "#999", fontSize: "12px" }}>
            创建者: {team.ownUserName}
          </div>
        </Space>
      </Card>
    </>
  );

  const renderFilters = () => (
    <Space style={{ marginBottom: 16 }} wrap>
      <Search
        placeholder="搜索团队"
        allowClear
        onSearch={handleSearch}
        style={{ width: 200 }}
      />
      {activeTab === "1" && (
        <>
          <Select
            placeholder="团队角色"
            allowClear
            style={{ width: 120 }}
            onChange={(value) => setIsOwner(value)}
          >
            <Option value={true}>所有者</Option>
            <Option value={false}>成员</Option>
          </Select>
          <Select
            placeholder="管理员"
            allowClear
            style={{ width: 120 }}
            onChange={(value) => setIsAdmin(value)}
          >
            <Option value={true}>是</Option>
            <Option value={false}>否</Option>
          </Select>
        </>
      )}
    </Space>
  );

  return (
    <div style={{ padding: "24px" }}>
      <Tabs activeKey={activeTab} onChange={handleTabChange}>
        <TabPane tab="我的团队" key="1">
          {renderFilters()}
        </TabPane>
        <TabPane tab="管理的团队" key="2">
          {renderFilters()}
        </TabPane>
        <TabPane tab="创建的团队" key="3">
          <Space style={{ marginBottom: 16 }} wrap>
            {renderFilters()}
            <Button
              type="primary"
              icon={<PlusOutlined />}
              onClick={() => setIsCreateModalVisible(true)}
            >
              创建团队
            </Button>
          </Space>
        </TabPane>
      </Tabs>

      <Spin spinning={loading}>
        <Row gutter={[16, 16]} style={{ marginTop: 16 }}>
          {teams.map((team) => (
            <Col key={team.id} xs={24} sm={12} md={8} lg={6} xl={4.8}>
              {renderTeamCard(team)}
            </Col>
          ))}
        </Row>
      </Spin>

      <div style={{ marginTop: 16, textAlign: "right" }}>
        <Pagination
          current={currentPage}
          pageSize={pageSize}
          total={total}
          onChange={handlePageChange}
          showSizeChanger
          showQuickJumper
          showTotal={(total) => `共 ${total} 条`}
        />
      </div>

      <Modal
        title="创建团队"
        open={isCreateModalVisible}
        onCancel={() => {
          setIsCreateModalVisible(false);
          createForm.resetFields();
        }}
        footer={null}
      >
        <Form form={createForm} onFinish={handleCreateTeam} layout="vertical">
          <Form.Item
            name="name"
            label="团队名称"
            rules={[
              { required: true, message: "请输入团队名称" },
              { min: 1, message: "团队名称不能为空" },
            ]}
          >
            <Input placeholder="请输入团队名称" />
          </Form.Item>
          <Form.Item
            name="description"
            label="团队描述"
            rules={[
              { required: true, message: "请输入团队描述" },
              { max: 255, message: "团队描述不能超过255个字符" },
            ]}
          >
            <Input.TextArea
              placeholder="请输入团队描述"
              maxLength={255}
              showCount
              rows={4}
            />
          </Form.Item>
          <Form.Item>
            <Space style={{ width: "100%", justifyContent: "flex-end" }}>
              <Button
                onClick={() => {
                  setIsCreateModalVisible(false);
                  createForm.resetFields();
                }}
              >
                取消
              </Button>
              <Button type="primary" htmlType="submit" loading={creating}>
                创建
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
