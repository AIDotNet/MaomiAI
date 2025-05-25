import { useState, useEffect } from "react";
import {
  Card,
  Row,
  Col,
  Spin,
  message,
  List,
  Button,
  Tag,
  Modal,
  Form,
  Input,
  Select,
  Tooltip,
  Space,
  Typography,
} from "antd";
const { Text, Link } = Typography;

import { GetApiClient } from "../../ServiceClient";
import { useParams } from "react-router";
import {
  QueryAiModelProviderCount,
  AiNotKeyEndpoint,
  AddAiEndpointRequest,
  UpdateAiEndpointRequest,
} from "../../../apiClient/models";
import { LOBE_DEFAULT_MODEL_LIST } from "../../../lobechat/types/aiModels";
import { proxyFormRequestError } from "../../../helper/RequestError";
import { RsaHelper } from "../../../helper/RsaHalper";
import { GetServiceInfo } from "../../../InitPage";
import { EyeOutlined, FunctionOutlined } from "@ant-design/icons";

export default function AiModel() {
  const { teamId } = useParams();
  const [providers, setProviders] = useState<QueryAiModelProviderCount[]>([]); // 供应商列表
  const [selectedProvider, setSelectedProvider] = useState<string | null>(null); // 当前选中供应商
  const [availableModels, setAvailableModels] = useState<AiNotKeyEndpoint[]>(
    []
  ); // API返回的可用模型列表
  const [loading, setLoading] = useState(false);
  const [apiLoading, setApiLoading] = useState(false); // API请求的加载状态
  const [createModalVisible, setCreateModalVisible] = useState(false);
  const [editModalVisible, setEditModalVisible] = useState(false);
  const [selectedModel, setSelectedModel] = useState<any>(null);
  const [createLoading, setCreateLoading] = useState(false);
  const [editLoading, setEditLoading] = useState(false);
  const [messageApi, contextHolder] = message.useMessage();
  const [form] = Form.useForm();
  const [editForm] = Form.useForm();

  // 获取供应商列表
  const fetchProviders = async () => {
    setLoading(true);
    try {
      const client = GetApiClient();
      const res = await client.api.aimodel
        .byTeamId(teamId as string)
        .providerlist.get();
      setProviders(res?.providers || []);
    } catch (e) {
      messageApi.error("获取模型供应商失败");
    } finally {
      setLoading(false);
    }
  };

  // 获取模型列表
  const fetchModels = async (provider?: string) => {
    setApiLoading(true);
    try {
      const client = GetApiClient();
      const body: any = {};
      if (provider) body.provider = provider;
      const res = await client.api.aimodel
        .byTeamId(teamId as string)
        .type.modellist.post(body);
      setAvailableModels(res?.aiModels || []);
    } catch (e) {
      messageApi.error("获取模型列表失败");
    } finally {
      setApiLoading(false);
    }
  };

  // 初始化加载
  useEffect(() => {
    fetchProviders();
    fetchModels();
  }, [teamId]);

  // 当providers更新时，选择数量最多的供应商
  useEffect(() => {
    if (providers.length > 0) {
      const maxCountProvider = providers.reduce((max, current) =>
        (current.count || 0) > (max.count || 0) ? current : max
      );
      setSelectedProvider(maxCountProvider.provider || null);
      fetchModels(maxCountProvider.provider || undefined);
    }
  }, [providers]);

  // 刷新全部
  const handleRefresh = () => {
    fetchProviders();
    fetchModels(); // 获取所有模型，不限定供应商
  };

  // 切换供应商
  const handleProviderClick = (provider: string | null) => {
    setSelectedProvider(provider);
    fetchModels(provider || undefined);
  };

  // 获取模型是否可用
  const isModelAvailable = (modelId: string) => {
    return availableModels.some((m) => m.name === modelId);
  };

  // 获取模型API信息
  const getModelApiInfo = (modelId: string) => {
    return availableModels.find((m) => m.name === modelId);
  };

  // 根据供应商筛选模型
  const getFilteredModels = () => {
    if (!selectedProvider) {
      return [];
    }

    // 以API返回的模型为基础，搜索前端的模型信息
    const modelsWithInfo = availableModels.map((apiModel) => {
      // 从前端模型列表中搜索对应的模型信息
      const frontendModel = LOBE_DEFAULT_MODEL_LIST.find(
        (m) => m.id === apiModel.name
      );

      if (frontendModel) {
        // 如果找到了前端模型信息，使用前端信息，但保留API的一些信息
        return {
          ...frontendModel,
          // 保留API返回的信息作为apiInfo
          apiInfo: apiModel,
        };
      } else {
        // 如果没找到前端模型信息，构造一个基本的模型对象
        return {
          id: apiModel.name || "unknown",
          displayName: apiModel.displayName || apiModel.name || "unknown",
          type: apiModel.aiModelType || "unknown",
          description: `模型: ${apiModel.name || "unknown"}`,
          providerId: apiModel.provider || "unknown",
          contextWindowTokens: apiModel.contextWindowTokens,
          abilities: apiModel.abilities,
          apiInfo: apiModel,
        };
      }
    });

    return modelsWithInfo;
  };

  // 获取所有可用的模型类型
  const getAvailableModelTypes = () => {
    const types = new Set<string>();
    LOBE_DEFAULT_MODEL_LIST.forEach((model) => {
      if (model.type) {
        types.add(model.type);
      }
    });
    return Array.from(types).map((type) => ({
      label: type,
      value: type,
    }));
  };

  // 处理创建模型
  const handleCreateModel = async (values: any) => {
    setCreateLoading(true);
    try {
      const client = GetApiClient();
      const serviceInfo = await GetServiceInfo();
      const encryptedKey = RsaHelper.encrypt(serviceInfo.rsaPublic, values.key);
      const command: AddAiEndpointRequest = {
        name: values.name,
        displayName: values.displayName,
        deploymentName: values.deploymentName,
        endpoint: values.endpoint,
        key: encryptedKey,
        provider: values.provider,
        aiModelType: values.aiModelType,
        contextWindowTokens: values.contextWindowTokens,
        abilities: values.abilities,
      };

      await client.api.aimodel.byTeamId(teamId as string).add.post(command);
      messageApi.success("创建成功");
      setCreateModalVisible(false);
      fetchModels(selectedProvider || undefined);
    } catch (e) {
      messageApi.error("创建失败");
      proxyFormRequestError(e, messageApi, form);
    } finally {
      setCreateLoading(false);
    }
  };

  // 打开创建模型弹窗
  const handleOpenCreateModal = (model: any) => {
    setSelectedModel(model);
    setCreateModalVisible(true);
  };

  // 处理模型选择
  const handleModelSelect = (modelId: string) => {
    const selectedModel = LOBE_DEFAULT_MODEL_LIST.find((m) => m.id === modelId);
    if (selectedModel) {
      form.setFieldsValue({
        name: selectedModel.id,
        displayName: selectedModel.displayName || selectedModel.id,
        deploymentName: selectedModel.id,
        provider: selectedModel.providerId,
        aiModelType: selectedModel.type,
        contextWindowTokens: selectedModel.contextWindowTokens,
        abilities: selectedModel.abilities,
        key: "", // 清空key字段
      });
    }
  };

  // 当弹窗显示时设置表单值
  useEffect(() => {
    if (createModalVisible && selectedModel) {
      form.setFieldsValue({
        name: selectedModel.id,
        displayName: selectedModel.displayName || selectedModel.id,
        deploymentName: selectedModel.id,
        provider: selectedModel.providerId,
        aiModelType: selectedModel.type,
        contextWindowTokens: selectedModel.contextWindowTokens,
        abilities: selectedModel.abilities,
        key: "", // 清空key字段
      });
    }
  }, [createModalVisible, selectedModel, form]);

  // 当新建弹窗打开时清空key字段
  useEffect(() => {
    if (createModalVisible) {
      form.setFieldValue("key", "");
    }
  }, [createModalVisible, form]);

  // 处理编辑模型
  const handleEditModel = async (values: any) => {
    setEditLoading(true);
    try {
      const client = GetApiClient();
      const serviceInfo = await GetServiceInfo();
      const command: UpdateAiEndpointRequest = {
        modelId: selectedModel.id,
        name: values.name,
        displayName: values.displayName,
        deploymentName: values.deploymentName,
        endpoint: values.endpoint,
        key: values.key
          ? RsaHelper.encrypt(serviceInfo.rsaPublic, values.key)
          : "*",
        provider: values.provider,
        aiModelType: values.aiModelType,
        contextWindowTokens: values.contextWindowTokens,
        abilities: values.abilities,
      };

      await client.api.aimodel.byTeamId(teamId as string).update.post(command);
      messageApi.success("更新成功");
      setEditModalVisible(false);
      fetchModels(selectedProvider || undefined);
    } catch (e) {
      messageApi.error("更新失败");
      proxyFormRequestError(e, messageApi, editForm);
    } finally {
      setEditLoading(false);
    }
  };

  // 打开编辑模型弹窗
  const handleOpenEditModal = (model: AiNotKeyEndpoint) => {
    setSelectedModel(model);
    setEditModalVisible(true);
  };

  // 当弹窗显示时设置表单值
  useEffect(() => {
    if (editModalVisible && selectedModel) {
      const formValues = {
        name: selectedModel.name,
        displayName: selectedModel.displayName || selectedModel.name,
        deploymentName: selectedModel.deploymentName || selectedModel.name,
        provider: selectedModel.provider,
        aiModelType: selectedModel.aiModelType,
        contextWindowTokens: selectedModel.contextWindowTokens,
        abilities: selectedModel.abilities,
        endpoint: selectedModel.endpoint,
        key: "", // 每次都清空key字段
      };
      editForm.setFieldsValue(formValues);
    }
  }, [editModalVisible, selectedModel, editForm]);

  return (
    <Row gutter={24} style={{ minHeight: 400 }}>
      {contextHolder}
      <Col span={5}>
        <Card title="模型供应商" bordered={false}>
          <List
            dataSource={providers.filter((p) => (p.count || 0) > 0)}
            renderItem={(item) => (
              <List.Item
                style={{
                  background:
                    selectedProvider === item.provider ? "#e6f7ff" : undefined,
                  cursor: "pointer",
                  borderRadius: 4,
                  marginBottom: 4,
                }}
                onClick={() => handleProviderClick(item.provider || "")}
              >
                <span>{item.provider}</span>
                <span style={{ float: "right", color: "#888" }}>
                  （{item.count}）
                </span>
              </List.Item>
            )}
          />
        </Card>
      </Col>
      <Col span={19}>
        <Card
          title="模型列表"
          bordered={false}
          extra={
            <div style={{ display: "flex", gap: 8 }}>
              <Button
                type="primary"
                onClick={() => {
                  setSelectedModel(null);
                  setCreateModalVisible(true);
                }}
              >
                新建
              </Button>
              <Button
                onClick={handleRefresh}
                loading={loading || apiLoading}
                type="default"
              >
                刷新
              </Button>
            </div>
          }
        >
          <Spin spinning={loading}>
            <List
              grid={{ gutter: 16, column: 3 }}
              dataSource={getFilteredModels()}
              renderItem={(model) => {
                const isAvailable = true; // 所有显示的模型都是可用的，因为它们来自API返回
                const apiInfo = model.apiInfo || getModelApiInfo(model.id);
                return (
                  <List.Item>
                    <Card
                      title={
                        <Space>
                          <span>{model.id}</span>
                          <Tag>{model.type}</Tag>
                        </Space>
                      }
                      extra={
                        <Space>
                          <Tag color={isAvailable ? "success" : "default"}>
                            {isAvailable ? "可用" : "未配置"}
                          </Tag>
                        </Space>
                      }
                    >
                      <Space direction="vertical">
                        <Space>
                          <Text>{model.description}</Text>
                        </Space>
                        <Space style={{ marginTop: 10 }}>
                          {model.contextWindowTokens ? (
                            <Tag color="blue">
                              {Math.round(model.contextWindowTokens / 1000)}k
                            </Tag>
                          ) : (
                            <Tag color="default">-</Tag>
                          )}
                          {model.abilities && (
                            <Space>
                              {model.abilities.vision && (
                                <Tooltip title="该模型支持视觉识别">
                                  <EyeOutlined style={{ color: "#55b467" }} />
                                </Tooltip>
                              )}
                              {model.abilities.functionCall && (
                                <Tooltip title="该模型支持 function call">
                                  <FunctionOutlined
                                    style={{ color: "#369eff" }}
                                  />{" "}
                                </Tooltip>
                              )}
                              {model.abilities?.files && (
                                <Tag color="purple">文件上传</Tag>
                              )}
                              {(model.abilities as any)?.reasoning && (
                                <Tag color="orange">推理</Tag>
                              )}
                              {(model.abilities as any)?.search && (
                                <Tag color="cyan">搜索</Tag>
                              )}
                            </Space>
                          )}
                        </Space>
                      </Space>
                      {isAvailable && apiInfo && (
                        <div
                          style={{
                            marginTop: 12,
                            borderTop: "1px solid #f0f0f0",
                            paddingTop: 12,
                          }}
                        >
                          <List
                            size="small"
                            dataSource={[apiInfo]}
                            renderItem={(item) => (
                              <List.Item
                                style={{
                                  padding: "8px 12px",
                                  background: "#fafafa",
                                  borderRadius: "6px",
                                  cursor: "pointer",
                                  border: "1px solid #e0e0e0",
                                  transition: "all 0.2s ease",
                                }}
                                onMouseEnter={(e) => {
                                  e.currentTarget.style.background = "#f0f0f0";
                                  e.currentTarget.style.borderColor = "#d0d0d0";
                                }}
                                onMouseLeave={(e) => {
                                  e.currentTarget.style.background = "#fafafa";
                                  e.currentTarget.style.borderColor = "#e0e0e0";
                                }}
                                onClick={() => handleOpenEditModal(item)}
                              >
                                <span style={{ fontWeight: 500 }}>
                                  {item.displayName || item.name}
                                </span>
                              </List.Item>
                            )}
                          />
                        </div>
                      )}
                    </Card>
                  </List.Item>
                );
              }}
              locale={{ emptyText: "暂无模型" }}
            />
          </Spin>
        </Card>
      </Col>

      <Modal
        title="创建模型"
        open={createModalVisible}
        onCancel={() => setCreateModalVisible(false)}
        onOk={() => form.submit()}
        confirmLoading={createLoading}
        width={600}
      >
        <Form form={form} layout="vertical" onFinish={handleCreateModel}>
          <Form.Item label="快速配置" name="quickSelect">
            <Select
              showSearch
              placeholder="选择预定义模型"
              allowClear={true}
              onChange={handleModelSelect}
              options={LOBE_DEFAULT_MODEL_LIST.map((model) => ({
                label: `${model.displayName || model.id} (${model.providerId})`,
                value: model.id,
              }))}
              filterOption={(input, option) =>
                (option?.label ?? "")
                  .toLowerCase()
                  .includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item
            label="显示名称(用于显示信息)"
            name="displayName"
            rules={[{ required: true, message: "请输入显示名称" }]}
          >
            <Input placeholder="请输入显示名称" />
          </Form.Item>
          <Form.Item
            label="模型名称"
            name="name"
            rules={[{ required: true, message: "请输入模型名称" }]}
          >
            <Input placeholder="请输入模型名称" />
          </Form.Item>
          <Form.Item
            label="部署名称(azure才需要用到，默认跟模型名称一致)"
            name="deploymentName"
            rules={[{ required: true, message: "请输入部署名称" }]}
          >
            <Input placeholder="请输入部署名称" />
          </Form.Item>
          <Form.Item
            label="供应商"
            name="provider"
            rules={[{ required: true, message: "请选择供应商" }]}
          >
            <Select
              showSearch
              placeholder="请选择供应商"
              options={providers.map((p) => ({
                label: p.provider,
                value: p.provider,
              }))}
            />
          </Form.Item>
          <Form.Item
            label="模型类型"
            name="aiModelType"
            rules={[{ required: true, message: "请选择模型类型" }]}
          >
            <Select
              placeholder="请选择模型类型"
              options={getAvailableModelTypes()}
            />
          </Form.Item>
          <Form.Item
            label="上下文窗口大小"
            name="contextWindowTokens"
            rules={[{ required: true, message: "请输入上下文窗口大小" }]}
          >
            <Input placeholder="请输入上下文窗口大小" />
          </Form.Item>
          <Form.Item
            label="请求端点"
            name="endpoint"
            rules={[{ required: true, message: "请输入请求端点" }]}
          >
            <Input placeholder="请输入请求端点" />
          </Form.Item>
          <Form.Item
            label="API Key"
            name="key"
            rules={[{ required: true, message: "请输入 API Key" }]}
          >
            <Input.Password placeholder="请输入 API Key" />
          </Form.Item>
        </Form>
      </Modal>

      {/* 编辑模型弹窗 */}
      <Modal
        title="编辑模型"
        open={editModalVisible}
        onCancel={() => setEditModalVisible(false)}
        onOk={() => editForm.submit()}
        confirmLoading={editLoading}
        width={600}
      >
        <Form form={editForm} layout="vertical" onFinish={handleEditModel}>
          <Form.Item
            label="显示名称(用于显示信息)"
            name="displayName"
            rules={[{ required: true, message: "请输入显示名称" }]}
          >
            <Input placeholder="请输入显示名称" />
          </Form.Item>
          <Form.Item
            label="模型名称"
            name="name"
            rules={[{ required: true, message: "请输入模型名称" }]}
          >
            <Input placeholder="请输入模型名称" />
          </Form.Item>
          <Form.Item
            label="部署名称(azure才需要用到，默认跟模型名称一致)"
            name="deploymentName"
            rules={[{ required: true, message: "请输入部署名称" }]}
          >
            <Input placeholder="请输入部署名称" />
          </Form.Item>
          <Form.Item
            label="供应商"
            name="provider"
            rules={[{ required: true, message: "请选择供应商" }]}
          >
            <Select
              showSearch
              placeholder="请选择供应商"
              options={providers.map((p) => ({
                label: p.provider,
                value: p.provider,
              }))}
            />
          </Form.Item>
          <Form.Item
            label="模型类型"
            name="aiModelType"
            rules={[{ required: true, message: "请选择模型类型" }]}
          >
            <Select
              placeholder="请选择模型类型"
              options={getAvailableModelTypes()}
            />
          </Form.Item>
          <Form.Item
            label="上下文窗口大小"
            name="contextWindowTokens"
            rules={[{ required: true, message: "请输入上下文窗口大小" }]}
          >
            <Input placeholder="请输入上下文窗口大小" />
          </Form.Item>
          <Form.Item
            label="请求端点"
            name="endpoint"
            rules={[{ required: true, message: "请输入请求端点" }]}
          >
            <Input placeholder="请输入请求端点" />
          </Form.Item>
          <Form.Item label="API Key" name="key">
            <Input.Password placeholder="不修改请留空" />
          </Form.Item>
        </Form>
      </Modal>
    </Row>
  );
}
