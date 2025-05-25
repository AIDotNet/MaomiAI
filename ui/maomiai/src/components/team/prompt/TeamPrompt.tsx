import React, { useState, useEffect } from "react";
import {
  Card,
  Avatar,
  Tag,
  Typography,
  Spin,
  Alert,
  Empty,
  Button,
  Input,
  Space,
  Tooltip,
  Modal,
  Form,
  Select,
  message,
  Row,
  Col,
  Divider,
  Descriptions,
  Upload,
} from "antd";
import {
  SearchOutlined,
  UserOutlined,
  TeamOutlined,
  CalendarOutlined,
  EditOutlined,
  AppstoreOutlined,
  PlusOutlined,
  EyeOutlined,
} from "@ant-design/icons";
import { GetApiClient, UploadImage } from "../../ServiceClient";
import {
  PromptItem,
  QueryPromptListCommand,
  CreatePromptCommand,
  UploadPromptAvatarCommand,
  UploadImageTypeObject,
} from "../../../apiClient/models";
import { useSelector } from "react-redux";
import { useParams } from "react-router";
import { CodeEditor, Markdown } from "@lobehub/ui";

const { Title, Text, Paragraph } = Typography;
const { Search, TextArea } = Input;
const { Option } = Select;

interface RootState {
  count: number;
  username: string;
  teamId: string;
}

// 提示词分类枚举
const PROMPT_CATEGORIES = [
  { key: "all", label: "全部", icon: <AppstoreOutlined /> },
  { key: "academic", label: "学术", icon: <AppstoreOutlined /> },
  { key: "career", label: "职业", icon: <AppstoreOutlined /> },
  { key: "copywriting", label: "文案", icon: <AppstoreOutlined /> },
  { key: "design", label: "设计", icon: <AppstoreOutlined /> },
  { key: "education", label: "教育", icon: <AppstoreOutlined /> },
  { key: "emotion", label: "情感", icon: <AppstoreOutlined /> },
  { key: "entertainment", label: "娱乐", icon: <AppstoreOutlined /> },
  { key: "gaming", label: "游戏", icon: <AppstoreOutlined /> },
  { key: "generic", label: "通用", icon: <AppstoreOutlined /> },
  { key: "lifestyle", label: "生活", icon: <AppstoreOutlined /> },
  { key: "business", label: "商业", icon: <AppstoreOutlined /> },
  { key: "office", label: "办公", icon: <AppstoreOutlined /> },
  { key: "programming", label: "编程", icon: <AppstoreOutlined /> },
  { key: "translation", label: "翻译", icon: <AppstoreOutlined /> },
];

// 获取可选择的分类（排除"全部"）
const SELECTABLE_CATEGORIES = PROMPT_CATEGORIES.filter(
  (cat) => cat.key !== "all"
);

export default function TeamPrompt() {
  const [prompts, setPrompts] = useState<PromptItem[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [searchKeyword, setSearchKeyword] = useState("");
  const [selectedCategory, setSelectedCategory] = useState("all");

  // 新建弹窗相关状态
  const [createModalVisible, setCreateModalVisible] = useState(false);
  const [createLoading, setCreateLoading] = useState(false);
  const [createForm] = Form.useForm();
  const [messageApi, contextHolder] = message.useMessage();

  // 详情弹窗相关状态
  const [detailModalVisible, setDetailModalVisible] = useState(false);
  const [detailLoading, setDetailLoading] = useState(false);
  const [selectedPrompt, setSelectedPrompt] = useState<PromptItem | null>(null);

  // 编辑模式相关状态
  const [isEditMode, setIsEditMode] = useState(false);
  const [editLoading, setEditLoading] = useState(false);
  const [editForm] = Form.useForm();

  // 头像上传相关状态
  const [avatarUploading, setAvatarUploading] = useState(false);

  const { teamId } = useParams();

  // 获取提示词列表
  const fetchPrompts = async (teamId?: string) => {
    setLoading(true);
    setError(null);

    try {
      const client = GetApiClient();
      const command: QueryPromptListCommand = {
        teamId: teamId || undefined, // 如果不填写teamId，则返回所有公开的提示词
      };

      const response = await client.api.prompt.list.post(command);

      if (response?.items) {
        setPrompts(response.items);
      } else {
        setPrompts([]);
      }
    } catch (err) {
      console.error("获取提示词列表失败:", err);
      setError("获取提示词列表失败，请稍后重试");
    } finally {
      setLoading(false);
    }
  };

  // 创建新提示词
  const handleCreatePrompt = async (values: any) => {
    if (!teamId) {
      messageApi.error("请先选择团队");
      return;
    }

    setCreateLoading(true);
    try {
      const client = GetApiClient();
      const command: CreatePromptCommand = {
        name: values.name,
        description: values.description,
        content: values.content,
        promptType: values.promptType,
        tags: values.tags || [],
      };

      await client.api.prompt.byTeamId(teamId).add.post(command);
      messageApi.success("创建成功");
      setCreateModalVisible(false);
      createForm.resetFields();
      // 刷新列表
      fetchPrompts(teamId);
    } catch (err) {
      console.error("创建提示词失败:", err);
      messageApi.error("创建失败，请稍后重试");
    } finally {
      setCreateLoading(false);
    }
  };

  // 打开创建弹窗
  const handleOpenCreateModal = () => {
    setCreateModalVisible(true);
    // 如果当前选中了特定分类，预设分类值
    if (selectedCategory !== "all") {
      createForm.setFieldsValue({ promptType: selectedCategory });
    }
  };

  // 关闭创建弹窗
  const handleCloseCreateModal = () => {
    setCreateModalVisible(false);
    createForm.resetFields();
  };

  // 获取单个提示词详情
  const fetchPromptDetail = async (promptId: string) => {
    setDetailLoading(true);
    try {
      const client = GetApiClient();
      const command: QueryPromptListCommand = {
        promptId: promptId,
      };

      const response = await client.api.prompt.list.post(command);

      if (response?.items && response.items.length > 0) {
        setSelectedPrompt(response.items[0]);
      } else {
        messageApi.error("获取提示词详情失败");
      }
    } catch (err) {
      console.error("获取提示词详情失败:", err);
      messageApi.error("获取提示词详情失败，请稍后重试");
    } finally {
      setDetailLoading(false);
    }
  };

  // 打开详情弹窗
  const handleOpenDetailModal = (prompt: PromptItem) => {
    setDetailModalVisible(true);
    setSelectedPrompt(prompt);
    if (prompt.id) {
      fetchPromptDetail(prompt.id);
    }
  };

  // 关闭详情弹窗
  const handleCloseDetailModal = () => {
    setDetailModalVisible(false);
    setSelectedPrompt(null);
    setIsEditMode(false);
    editForm.resetFields();
  };

  // 进入编辑模式
  const handleEnterEditMode = () => {
    if (selectedPrompt) {
      setIsEditMode(true);
      // 设置表单初始值
      editForm.setFieldsValue({
        name: selectedPrompt.name,
        description: selectedPrompt.description,
        content: selectedPrompt.content,
        promptType: selectedPrompt.promptType,
        tags: selectedPrompt.tags || [],
      });
    }
  };

  // 取消编辑
  const handleCancelEdit = () => {
    setIsEditMode(false);
    editForm.resetFields();
  };

  // 保存编辑
  const handleSaveEdit = async (values: any) => {
    if (!selectedPrompt?.id || !teamId) {
      messageApi.error("缺少必要参数");
      return;
    }

    setEditLoading(true);
    try {
      const client = GetApiClient();
      const command = {
        promptId: selectedPrompt.id,
        name: values.name,
        description: values.description,
        content: values.content,
        promptType: values.promptType,
        tags: values.tags || [],
      };

      await client.api.prompt.byTeamId(teamId).update.post(command);
      messageApi.success("更新成功");

      // 更新本地状态
      setSelectedPrompt({
        ...selectedPrompt,
        ...values,
      });

      // 刷新列表
      fetchPrompts(teamId);
      setIsEditMode(false);
      editForm.resetFields();
    } catch (err) {
      console.error("更新提示词失败:", err);
      messageApi.error("更新失败，请稍后重试");
    } finally {
      setEditLoading(false);
    }
  };

  // 处理头像上传
  const handleAvatarUpload = async (file: File) => {
    if (!file) {
      messageApi.error("请选择一个文件");
      return;
    }
    if (!selectedPrompt?.id) {
      messageApi.error("提示词ID不存在");
      return;
    }

    try {
      setAvatarUploading(true);
      const client = GetApiClient();
      
      // 上传头像文件
      const preUploadResponse = await UploadImage(client, file, UploadImageTypeObject.DocumentFile);

      // 更新提示词头像
      const command: UploadPromptAvatarCommand = {
        fileId: preUploadResponse.fileId,
        promptId: selectedPrompt.id,
      };

      await client.api.prompt.byTeamId(teamId || "").avatar.post(command);

      messageApi.success("头像更新成功");
      
      // 刷新提示词详情和列表
      if (selectedPrompt.id) {
        await fetchPromptDetail(selectedPrompt.id);
      }
      fetchPrompts(teamId);
    } catch (error) {
      console.error("upload avatar error:", error);
      const typedError = error as {
        detail?: string;
      };
      if (typedError.detail) {
        messageApi.error(typedError.detail);
      } else {
        messageApi.error("头像上传失败");
      }
    } finally {
      setAvatarUploading(false);
    }
  };

  // 组件挂载时获取数据
  useEffect(() => {
    fetchPrompts(teamId);
  }, [teamId]);

  // 过滤提示词列表
  const filteredPrompts = prompts.filter((prompt) => {
    // 搜索关键词过滤
    const matchesSearch =
      !searchKeyword ||
      prompt.name?.toLowerCase().includes(searchKeyword.toLowerCase()) ||
      prompt.description?.toLowerCase().includes(searchKeyword.toLowerCase()) ||
      prompt.tags?.some((tag) =>
        tag.toLowerCase().includes(searchKeyword.toLowerCase())
      );

    // 分类过滤
    const matchesCategory =
      selectedCategory === "all" ||
      prompt.promptType?.toLowerCase() === selectedCategory;

    return matchesSearch && matchesCategory;
  });

  // 格式化时间
  const formatTime = (timeStr?: string | null) => {
    if (!timeStr) return "未知";
    return new Date(timeStr).toLocaleString("zh-CN");
  };

  // 获取提示词类型标签颜色
  const getPromptTypeColor = (promptType?: any) => {
    const typeColors: Record<string, string> = {
      academic: "blue",
      career: "green",
      copywriting: "orange",
      design: "purple",
      education: "cyan",
      emotion: "pink",
      entertainment: "red",
      gaming: "volcano",
      generic: "default",
      lifestyle: "lime",
      business: "gold",
      office: "geekblue",
      programming: "magenta",
      translation: "blue",
    };
    return typeColors[promptType?.toLowerCase()] || "default";
  };

  // 处理分类选择
  const handleCategorySelect = (key: string) => {
    setSelectedCategory(key);
  };

  // 渲染助手卡片
  const renderPromptCard = (item: PromptItem) => (
    <Col xs={24} sm={12} md={8} lg={6} xl={4} key={item.id}>
      <Card
        hoverable
        style={{
          borderRadius: "8px",
          border: "1px solid #f0f0f0",
          transition: "all 0.3s ease",
          position: "relative",
          cursor: "pointer",
        }}
        bodyStyle={{
          padding: "16px",
          display: "flex",
          flexDirection: "column",
        }}
        onClick={() => handleOpenDetailModal(item)}
      >
        {/* 操作按钮组 */}
        <div
          style={{ position: "absolute", top: "8px", right: "8px", zIndex: 1 }}
        >
          <Space>
            <Tooltip title="查看详情">
              <Button
                type="text"
                icon={<EyeOutlined />}
                size="small"
                onClick={(e) => {
                  e.stopPropagation();
                  handleOpenDetailModal(item);
                }}
              />
            </Tooltip>
            <Tooltip title="编辑">
              <Button
                type="text"
                icon={<EditOutlined />}
                size="small"
                onClick={(e) => {
                  e.stopPropagation();
                  handleOpenDetailModal(item);
                  // 延迟进入编辑模式，确保弹窗已打开
                  setTimeout(() => {
                    handleEnterEditMode();
                  }, 100);
                }}
              />
            </Tooltip>
          </Space>
        </div>

        <div style={{ textAlign: "center", marginBottom: "12px" }}>
          <Avatar
            src={item.avatarPath}
            icon={<UserOutlined />}
            size={48}
            style={{
              marginBottom: "8px",
              border: "2px solid #f0f0f0",
            }}
          />
          <div>
            <Title level={5} style={{ margin: "0 0 4px 0", fontSize: "14px" }}>
              {item.name}
            </Title>
            <Text type="secondary" style={{ fontSize: "11px" }}>
              {item.createUserName || "未知创建者"}
            </Text>
          </div>
        </div>

        <div style={{ flex: 1, marginBottom: "12px" }}>
          <Paragraph
            ellipsis={{ rows: 2 }}
            style={{
              fontSize: "12px",
              color: "#666",
              lineHeight: "1.4",
              margin: 0,
            }}
          >
            {item.description || "暂无描述"}
          </Paragraph>
        </div>

        <Space>
          {item.promptType && (
            <Tag
              color={getPromptTypeColor(item.promptType)}
              style={{ fontSize: "10px", marginBottom: "5px" }}
            >
              {PROMPT_CATEGORIES.find(
                (cat) => cat.key === item.promptType?.toLowerCase()
              )?.label || item.promptType}
            </Tag>
          )}
          {item.tags && item.tags.length > 0 && (
            <>
              {item.tags.slice(0, 2).map((tag, index) => (
                <Tag
                  key={index}
                  style={{ fontSize: "10px", marginBottom: "5px" }}
                >
                  {tag}
                </Tag>
              ))}
              {item.tags.length > 2 && (
                <Tag style={{ fontSize: "10px", marginBottom: "5px" }}>
                  +{item.tags.length - 2}
                </Tag>
              )}
            </>
          )}
        </Space>
        <br />
        <Divider />

        <Space direction="vertical">
          {item.teamName && (
            <div>
              <TeamOutlined />
              {item.teamName}
            </div>
          )}
          <div>
            <CalendarOutlined />
            {formatTime(item.createTime)}
          </div>
        </Space>
      </Card>
    </Col>
  );

  return (
    <>
      {contextHolder}
      <div
        style={{ padding: "24px", background: "#f5f5f5", minHeight: "100vh" }}
      >
        {/* 顶部区域 */}
        <div style={{ marginBottom: "24px" }}>
          <Title level={2} style={{ marginBottom: "16px", color: "#1f1f1f" }}>
            助手列表
            <Text
              type="secondary"
              style={{
                fontSize: "16px",
                fontWeight: "normal",
                marginLeft: "12px",
              }}
            >
              {filteredPrompts.length}
            </Text>
          </Title>

          {/* 搜索和操作栏 */}
          <Row gutter={[16, 16]} style={{ marginBottom: "20px" }}>
            <Col xs={24} sm={16} md={18}>
              <Search
                placeholder="搜索助手名称、描述或标签..."
                allowClear
                size="large"
                style={{ width: "100%" }}
                prefix={<SearchOutlined />}
                value={searchKeyword}
                onChange={(e) => setSearchKeyword(e.target.value)}
              />
            </Col>
            <Col xs={24} sm={8} md={6}>
              <Space style={{ width: "100%" }}>
                <Button
                  type="primary"
                  size="large"
                  icon={<PlusOutlined />}
                  onClick={handleOpenCreateModal}
                  style={{ borderRadius: "8px" }}
                >
                  新建助手
                </Button>
                <Button
                  size="large"
                  onClick={() => fetchPrompts(teamId)}
                  loading={loading}
                  style={{ borderRadius: "8px" }}
                >
                  刷新
                </Button>
              </Space>
            </Col>
          </Row>

          {/* 分类筛选 */}
          <Card
            style={{
              marginBottom: "20px",
              borderRadius: "12px",
              border: "1px solid #f0f0f0",
            }}
            bodyStyle={{ padding: "16px" }}
          >
            <Row gutter={[8, 8]}>
              {PROMPT_CATEGORIES.map((category) => (
                <Col key={category.key}>
                  <Button
                    type={
                      selectedCategory === category.key ? "primary" : "default"
                    }
                    size="middle"
                    icon={category.icon}
                    onClick={() => handleCategorySelect(category.key)}
                    style={{
                      borderRadius: "20px",
                      height: "36px",
                      paddingLeft: "16px",
                      paddingRight: "16px",
                    }}
                  >
                    {category.label}
                  </Button>
                </Col>
              ))}
            </Row>
          </Card>
        </div>

        {/* 错误提示 */}
        {error && (
          <Alert
            message="加载失败"
            description={error}
            type="error"
            showIcon
            style={{ marginBottom: "24px", borderRadius: "8px" }}
            action={
              <Button size="small" onClick={() => fetchPrompts(teamId)}>
                重试
              </Button>
            }
          />
        )}

        {/* 助手卡片网格 */}
        <Spin spinning={loading}>
          {filteredPrompts.length === 0 && !loading ? (
            <Empty
              description={
                searchKeyword
                  ? `没有找到包含 "${searchKeyword}" 的助手`
                  : selectedCategory !== "all"
                  ? `${
                      PROMPT_CATEGORIES.find(
                        (cat) => cat.key === selectedCategory
                      )?.label
                    } 分类下暂无助手`
                  : teamId
                  ? "当前团队还没有助手，快去创建一个吧！"
                  : "暂无公开的助手"
              }
              style={{
                padding: "80px 0",
                background: "#fff",
                borderRadius: "12px",
                border: "1px solid #f0f0f0",
              }}
            />
          ) : (
            <Row gutter={[16, 16]}>{filteredPrompts.map(renderPromptCard)}</Row>
          )}
        </Spin>
      </div>

      {/* 新建助手弹窗 */}
      <Modal
        title="新建助手"
        open={createModalVisible}
        onOk={() => createForm.submit()}
        onCancel={handleCloseCreateModal}
        confirmLoading={createLoading}
        width={600}
        okText="创建"
        cancelText="取消"
        style={{ borderRadius: "12px" }}
      >
        <Form
          form={createForm}
          layout="vertical"
          onFinish={handleCreatePrompt}
          initialValues={{
            promptType:
              selectedCategory !== "all" ? selectedCategory : undefined,
          }}
        >
          <Form.Item
            name="name"
            label="助手名称"
            rules={[
              { required: true, message: "请输入助手名称" },
              { max: 50, message: "名称不能超过50个字符" },
            ]}
          >
            <Input placeholder="请输入助手名称" size="large" />
          </Form.Item>

          <Form.Item
            name="description"
            label="助手描述"
            rules={[{ max: 200, message: "描述不能超过200个字符" }]}
          >
            <TextArea
              placeholder="请输入助手描述"
              rows={3}
              showCount
              maxLength={200}
            />
          </Form.Item>

          <Form.Item
            name="promptType"
            label="分类"
            rules={[{ required: true, message: "请选择分类" }]}
          >
            <Select placeholder="请选择分类" size="large">
              {SELECTABLE_CATEGORIES.map((category) => (
                <Option key={category.key} value={category.key}>
                  <Space>
                    {category.icon}
                    {category.label}
                  </Space>
                </Option>
              ))}
            </Select>
          </Form.Item>

          <Form.Item
            name="content"
            label="助手设定"
            rules={[
              { required: true, message: "请输入助手设定" },
              { max: 2000, message: "内容不能超过5000个字符" },
            ]}
          >
            <TextArea
              placeholder="请输入助手的详细设定，支持 Markdown 格式"
              rows={8}
              showCount
              maxLength={5000}
            />
          </Form.Item>

          <Form.Item name="tags" label="标签" help="按回车键添加标签，最多10个">
            <Select
              mode="tags"
              placeholder="请输入标签"
              style={{ width: "100%" }}
              maxCount={10}
              maxTagTextLength={20}
              size="large"
            />
          </Form.Item>
        </Form>
      </Modal>

      {/* 助手详情弹窗 */}
      <Modal
        title={
          <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
            <Upload
              name="avatar"
              showUploadList={false}
              beforeUpload={(file) => {
                handleAvatarUpload(file);
                return false;
              }}
              accept="image/*"
              disabled={avatarUploading}
            >
              <Tooltip title="点击更换头像">
                <Spin spinning={avatarUploading} size="small">
                  <Avatar
                    src={selectedPrompt?.avatarPath}
                    icon={<UserOutlined />}
                    size={40}
                    style={{ cursor: "pointer" }}
                  />
                </Spin>
              </Tooltip>
            </Upload>
            <div>
              <div style={{ fontSize: "18px", fontWeight: "600" }}>
                {selectedPrompt?.name || "助手详情"}
              </div>
              <div
                style={{ fontSize: "12px", color: "#666", marginTop: "2px" }}
              >
                {selectedPrompt?.createUserName || "未知创建者"} ·{" "}
                {formatTime(selectedPrompt?.createTime)}
              </div>
            </div>
          </div>
        }
        open={detailModalVisible}
        onCancel={handleCloseDetailModal}
        footer={
          isEditMode
            ? [
                <Button key="cancel" onClick={handleCancelEdit}>
                  取消
                </Button>,
                <Button
                  key="save"
                  type="primary"
                  loading={editLoading}
                  onClick={() => editForm.submit()}
                >
                  保存
                </Button>,
              ]
            : [
                <Button key="close" onClick={handleCloseDetailModal}>
                  关闭
                </Button>,
                <Button
                  key="edit"
                  type="primary"
                  icon={<EditOutlined />}
                  onClick={handleEnterEditMode}
                >
                  编辑
                </Button>,
              ]
        }
        width={"100vh"}
        style={{ borderRadius: "12px" }}
        styles={{
          body: { maxHeight: "70vh", overflowY: "auto" },
        }}
      >
        <Spin spinning={detailLoading}>
          {selectedPrompt && (
            <div style={{ padding: "16px 0" }}>
              {isEditMode ? (
                // 编辑模式 - 显示表单
                <Form
                  form={editForm}
                  layout="vertical"
                  onFinish={handleSaveEdit}
                  initialValues={{
                    name: selectedPrompt.name,
                    description: selectedPrompt.description,
                    content: selectedPrompt.content,
                    promptType: selectedPrompt.promptType,
                    tags: selectedPrompt.tags || [],
                  }}
                >
                  <Form.Item
                    name="name"
                    label="助手名称"
                    rules={[
                      { required: true, message: "请输入助手名称" },
                      { max: 50, message: "名称不能超过50个字符" },
                    ]}
                  >
                    <Input placeholder="请输入助手名称" size="large" />
                  </Form.Item>

                  <Form.Item
                    name="description"
                    label="助手描述"
                    rules={[{ max: 200, message: "描述不能超过200个字符" }]}
                  >
                    <TextArea
                      placeholder="请输入助手描述"
                      rows={3}
                      showCount
                      maxLength={200}
                    />
                  </Form.Item>

                  <Form.Item
                    name="promptType"
                    label="分类"
                    rules={[{ required: true, message: "请选择分类" }]}
                  >
                    <Select placeholder="请选择分类" size="large">
                      {SELECTABLE_CATEGORIES.map((category) => (
                        <Option key={category.key} value={category.key}>
                          <Space>
                            {category.icon}
                            {category.label}
                          </Space>
                        </Option>
                      ))}
                    </Select>
                  </Form.Item>

                  <Form.Item
                    name="tags"
                    label="标签"
                    help="按回车键添加标签，最多10个"
                  >
                    <Select
                      mode="tags"
                      placeholder="请输入标签"
                      style={{ width: "100%" }}
                      maxCount={10}
                      maxTagTextLength={20}
                      size="large"
                    />
                  </Form.Item>

                  <Form.Item
                    name="content"
                    label="助手设定"
                    rules={[
                      { required: true, message: "请输入助手设定" },
                      { max: 5000, message: "内容不能超过5000个字符" },
                    ]}
                  >
                    <CodeEditor
                      language="markdown"
                      placeholder="请输入助手的详细设定，支持 Markdown 格式"
                      value={selectedPrompt.content ?? ""}
                      onValueChange={(t) => (selectedPrompt.content = t)}
                      width={"100%"}
                      maxLength={2000}
                    />
                  </Form.Item>
                </Form>
              ) : (
                // 查看模式 - 显示详情
                <>
                  {/* 基本信息 */}
                  <Descriptions
                    column={2}
                    style={{ marginBottom: "24px" }}
                    items={[
                      {
                        key: "description",
                        label: "描述",
                        children: selectedPrompt.description || "暂无描述",
                        span: 2,
                      },
                      {
                        key: "category",
                        label: "分类",
                        children: selectedPrompt.promptType ? (
                          <Tag
                            color={getPromptTypeColor(
                              selectedPrompt.promptType
                            )}
                          >
                            {PROMPT_CATEGORIES.find(
                              (cat) =>
                                cat.key ===
                                selectedPrompt.promptType?.toLowerCase()
                            )?.label || selectedPrompt.promptType}
                          </Tag>
                        ) : (
                          "未分类"
                        ),
                      },
                      {
                        key: "team",
                        label: "团队",
                        children: selectedPrompt.teamName ? (
                          <Space>
                            <TeamOutlined />
                            {selectedPrompt.teamName}
                          </Space>
                        ) : (
                          "公开助手"
                        ),
                      },
                      {
                        key: "tags",
                        label: "标签",
                        children:
                          selectedPrompt.tags &&
                          selectedPrompt.tags.length > 0 ? (
                            <Space wrap>
                              {selectedPrompt.tags.map((tag, index) => (
                                <Tag key={index}>{tag}</Tag>
                              ))}
                            </Space>
                          ) : (
                            "暂无标签"
                          ),
                        span: 2,
                      },
                    ]}
                  />

                  <Divider orientation="left">助手设定</Divider>

                  {/* 助手设定内容 - 使用 Markdown 渲染 */}
                  <div
                    style={{
                      background: "#fafafa",
                      border: "1px solid #f0f0f0",
                      borderRadius: "8px",
                      padding: "16px",
                      minHeight: "200px",
                    }}
                  >
                    {selectedPrompt.content ? (
                      <Markdown>{selectedPrompt.content}</Markdown>
                    ) : (
                      <div
                        style={{
                          color: "#999",
                          textAlign: "center",
                          padding: "40px 0",
                        }}
                      >
                        暂无助手设定内容
                      </div>
                    )}
                  </div>
                </>
              )}
            </div>
          )}
        </Spin>
      </Modal>
    </>
  );
}
