import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router";
import { GetApiClient } from "../../ServiceClient";
import type { MaomiAIDocumentSharedQueriesResponseQueryWikiSimpleInfoResponse } from "../../../ApiClient/models";
import {
  Input,
  Button,
  Space,
  Modal,
  Form,
  message,
  Card,
  Tag,
  Avatar,
  Flex,
} from "antd";
import {
  SearchOutlined,
  PlusOutlined,
  BookOutlined,
  LockOutlined,
  GlobalOutlined,
} from "@ant-design/icons";

export default function WikiList() {
  const { teamId } = useParams();
  const navigate = useNavigate();
  const [wikiList, setWikiList] = useState<
    MaomiAIDocumentSharedQueriesResponseQueryWikiSimpleInfoResponse[]
  >([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchText, setSearchText] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [form] = Form.useForm();

  useEffect(() => {
    const apiClient = GetApiClient();
    const fetchWikiList = async () => {
      try {
        setLoading(true);
        const response = await apiClient.api.wiki.byTeamId(teamId!).wikis.get();
        setWikiList(response || []);
      } catch (err) {
        setError("获取知识库列表失败");
        console.error("Failed to fetch wiki list:", err);
      } finally {
        setLoading(false);
      }
    };

    if (teamId) {
      fetchWikiList();
    }
  }, [teamId]);

  const handleSearch = () => {
    // TODO: Implement search functionality
    console.log("Searching for:", searchText);
  };

  const handleCreate = () => {
    setIsModalOpen(true);
  };

  const handleModalCancel = () => {
    setIsModalOpen(false);
    form.resetFields();
  };

  const handleModalOk = async () => {
    try {
      const values = await form.validateFields();
      const apiClient = GetApiClient();

      // 创建知识库
      await apiClient.api.wiki.byTeamId(teamId!).create.post({
        name: values.name,
        description: values.description,
      });

      message.success("创建知识库成功");
      setIsModalOpen(false);
      form.resetFields();

      // 刷新列表
      const response = await apiClient.api.wiki.byTeamId(teamId!).wikis.get();
      setWikiList(response || []);
    } catch (err) {
      message.error("创建知识库失败");
      console.error("Failed to create wiki:", err);
    }
  };

  const handleCardClick = (wikiId: string) => {
    navigate(`/app/team/${teamId}/wiki/${wikiId}/document`);
  };

  const filteredWikiList = wikiList.filter(
    (wiki) =>
      wiki.name?.toLowerCase().includes(searchText.toLowerCase()) ||
      wiki.description?.toLowerCase().includes(searchText.toLowerCase())
  );

  const handleCreateWiki = () => {
    return;
    <Modal
      title="创建知识库"
      open={isModalOpen}
      onOk={handleModalOk}
      onCancel={handleModalCancel}
      okText="创建"
      cancelText="取消"
    >
      <Form form={form} layout="vertical">
        <Form.Item
          name="name"
          label="知识库名称"
          rules={[{ required: true, message: "请输入知识库名称" }]}
        >
          <Input placeholder="请输入知识库名称" />
        </Form.Item>
        <Form.Item name="description" label="知识库描述">
          <Input.TextArea placeholder="请输入知识库描述" rows={4} />
        </Form.Item>
      </Form>
    </Modal>;
  };

  if (loading) {
    return <div>加载中...</div>;
  }

  if (error) {
    return <div className="text-red-500">{error}</div>;
  }

  return (
    <>
      <div className="p-4">
        <div className="mb-6">
          <Space.Compact style={{ width: "50vh", minWidth: "600px" }}>
            <Input
              placeholder="搜索知识库"
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
              onPressEnter={handleSearch}
              prefix={<SearchOutlined />}
              style={{ marginRight: "10px" }}
            />
            <Button
              type="primary"
              onClick={handleSearch}
              style={{ marginRight: "10px" }}
            >
              搜索
            </Button>
            <Button
              type="primary"
              icon={<PlusOutlined />}
              onClick={handleCreate}
            >
              新建
            </Button>
          </Space.Compact>
        </div>

        <h2 className="text-2xl font-bold mb-4">知识库列表</h2>
        <Flex wrap gap="middle">
          {filteredWikiList.map((wiki) => (
            <Card
              key={wiki.wikiId}
              hoverable
              style={{ width: 400, maxWidth: '100%' }}
              onClick={() => handleCardClick(wiki.wikiId!)}
            >
              <Flex gap="middle" align="flex-start">
                <Avatar
                  size={48}
                  icon={<BookOutlined />}
                  className="bg-blue-100 text-blue-500"
                />
                <Flex vertical flex={1}>
                  <Flex align="center" gap="small">
                    <h3 className="text-base font-semibold text-gray-800 m-0 break-words">
                      {wiki.name}
                    </h3>
                    {wiki.isPublic ? (
                      <Tag
                        icon={<GlobalOutlined />}
                        color="green"
                      >
                        公开
                      </Tag>
                    ) : (
                      <Tag
                        icon={<LockOutlined />}
                        color="default"
                      >
                        私有
                      </Tag>
                    )}
                  </Flex>
                  {wiki.description && (
                    <p className="text-gray-600 text-sm line-clamp-4 mt-2">
                      {wiki.description}
                    </p>
                  )}
                </Flex>
              </Flex>
            </Card>
          ))}
        </Flex>
      </div>
      {handleCreateWiki()}
    </>
  );
}
