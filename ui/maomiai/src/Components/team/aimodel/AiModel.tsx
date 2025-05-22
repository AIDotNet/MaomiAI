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
} from "antd";
import { GetApiClient } from "../../ServiceClient";
import { useParams } from "react-router";
import { QueryAiModelProviderCount, AiNotKeyEndpoint, AddAiEndpointRequest } from '../../../apiClient/models';
import { LOBE_DEFAULT_MODEL_LIST } from '../../../lobechat/types/aiModels';
import { proxyFormRequestError } from "../../../helper/RequestError";
import { RsaHelper } from "../../../helper/RsaHalper";
import { GetServiceInfo } from "../../../InitPage";

export default function AiModel() {
  const { teamId } = useParams();
  const [providers, setProviders] = useState<QueryAiModelProviderCount[]>([]); // 供应商列表
  const [selectedProvider, setSelectedProvider] = useState<string | null>(null); // 当前选中供应商
  const [availableModels, setAvailableModels] = useState<AiNotKeyEndpoint[]>([]); // API返回的可用模型列表
  const [loading, setLoading] = useState(false);
  const [apiLoading, setApiLoading] = useState(false); // API请求的加载状态
  const [createModalVisible, setCreateModalVisible] = useState(false);
  const [selectedModel, setSelectedModel] = useState<any>(null);
  const [createLoading, setCreateLoading] = useState(false);
  const [messageApi, contextHolder] = message.useMessage();
  const [form] = Form.useForm();

  // 获取供应商列表
  const fetchProviders = async () => {
    setLoading(true);
    try {
      const client = GetApiClient();
      const res = await client.api.aimodel.byTeamId(teamId as string).providerlist.get();
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
        .type
        .modellist.post(body);
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

  // 刷新全部
  const handleRefresh = () => {
    fetchProviders();
    fetchModels(selectedProvider || undefined);
  };

  // 切换供应商
  const handleProviderClick = (provider: string | null) => {
    setSelectedProvider(provider);
    fetchModels(provider || undefined);
  };

  // 获取模型是否可用
  const isModelAvailable = (modelId: string) => {
    return availableModels.some(m => m.name === modelId);
  };

  // 获取模型API信息
  const getModelApiInfo = (modelId: string) => {
    return availableModels.find(m => m.name === modelId);
  };

  // 根据供应商筛选模型
  const getFilteredModels = () => {
    if (!selectedProvider) {
      return LOBE_DEFAULT_MODEL_LIST;
    }
    return LOBE_DEFAULT_MODEL_LIST.filter(model => 
      model.providerId.toLowerCase() === selectedProvider.toLowerCase()
    );
  };

  // 处理创建模型
  const handleCreateModel = async (values: any) => {
    setCreateLoading(true);
    try {
      const client = GetApiClient();
      const serviceInfo = await GetServiceInfo();
      const encryptedKey = RsaHelper.encrypt(
        serviceInfo.rsaPublic,
        values.key
      );
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
      messageApi.success('创建成功');
      setCreateModalVisible(false);
      fetchModels(selectedProvider || undefined);
    } catch (e) {
      messageApi.error('创建失败');
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
    const selectedModel = LOBE_DEFAULT_MODEL_LIST.find(m => m.id === modelId);
    if (selectedModel) {
      form.setFieldsValue({
        name: selectedModel.id,
        displayName: selectedModel.displayName || selectedModel.id,
        deploymentName: selectedModel.id,
        provider: selectedModel.providerId,
        aiModelType: selectedModel.type,
        contextWindowTokens: selectedModel.contextWindowTokens,
        abilities: selectedModel.abilities,
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
      });
    }
  }, [createModalVisible, selectedModel, form]);

  return (
    <Row gutter={24} style={{ minHeight: 400 }}>
      {contextHolder}
      <Col span={5}>
        <Card title="模型供应商" bordered={false}>
          <List
            dataSource={[
              { provider: null, count: providers.reduce((sum, p) => sum + (p.count || 0), 0) },
              ...providers
            ]}
            renderItem={item => (
              <List.Item
                style={{
                  background: selectedProvider === item.provider ? '#e6f7ff' : undefined,
                  cursor: 'pointer',
                  borderRadius: 4,
                  marginBottom: 4,
                }}
                onClick={() => handleProviderClick(item.provider || '')}
              >
                <span>{item.provider ? item.provider : '全部模型'}</span>
                <span style={{ float: 'right', color: '#888' }}>（{item.count}）</span>
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
            <div style={{ display: 'flex', gap: 8 }}>
              <Button 
                type="primary"
                onClick={() => {
                  setSelectedModel(null);
                  setCreateModalVisible(true);
                }}
              >
                新建
              </Button>
              <Button onClick={handleRefresh} loading={loading || apiLoading} type="default">
                刷新
              </Button>
            </div>
          }
        >
          <Spin spinning={loading}>
            <List
              grid={{ gutter: 16, column: 3 }}
              dataSource={getFilteredModels()}
              renderItem={model => {
                const isAvailable = isModelAvailable(model.id);
                const apiInfo = getModelApiInfo(model.id);
                return (
                  <List.Item>
                    <Card 
                      title={model.displayName || model.id}
                      extra={
                        <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
                          <Tag color={isAvailable ? "success" : "default"}>
                            {isAvailable ? "可用" : "未配置"}
                          </Tag>
                          {!isAvailable && (
                            <Button 
                              type="primary" 
                              size="small"
                              onClick={() => handleOpenCreateModal(model)}
                            >
                              新建
                            </Button>
                          )}
                        </div>
                      }
                    >
                      <div>类型: {model.type}</div>
                      <div>供应商: {model.providerId}</div>
                      <div>最大Token: {model.contextWindowTokens}</div>
                      {model.description && (
                        <div style={{ marginTop: 8, color: '#666' }}>{model.description}</div>
                      )}
                      {model.abilities && (
                        <div style={{ marginTop: 8 }}>
                          {model.abilities.vision && <Tag color="blue">视觉</Tag>}
                          {model.abilities.functionCall && <Tag color="green">函数调用</Tag>}
                          {model.abilities.files && <Tag color="purple">文件上传</Tag>}
                          {model.abilities.reasoning && <Tag color="orange">推理</Tag>}
                          {model.abilities.search && <Tag color="cyan">搜索</Tag>}
                        </div>
                      )}
                    </Card>
                  </List.Item>
                );
              }}
              locale={{ emptyText: '暂无模型' }}
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
        <Form
          form={form}
          layout="vertical"
          onFinish={handleCreateModel}
        >
          <Form.Item
            label="快速配置"
            name="quickSelect"
          >
            <Select
              showSearch
              placeholder="选择预定义模型"
              onChange={handleModelSelect}
              options={LOBE_DEFAULT_MODEL_LIST.map(model => ({
                label: `${model.displayName || model.id} (${model.providerId})`,
                value: model.id
              }))}
              filterOption={(input, option) =>
                (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
          <Form.Item
            label="显示名称(用于显示信息)"
            name="displayName"
            rules={[{ required: true, message: '请输入显示名称' }]}
          >
            <Input placeholder="请输入显示名称" />
          </Form.Item>
          <Form.Item
            label="模型名称"
            name="name"
            rules={[{ required: true, message: '请输入模型名称' }]}
          >
            <Input placeholder="请输入模型名称" />
          </Form.Item>
          <Form.Item
            label="部署名称(azure才需要用到，默认跟模型名称一致)"
            name="deploymentName"
            rules={[{ required: true, message: '请输入部署名称' }]}
          >
            <Input placeholder="请输入部署名称" />
          </Form.Item>
          <Form.Item
            label="供应商"
            name="provider"
            rules={[{ required: true, message: '请选择供应商' }]}
          >
            <Select
              placeholder="请选择供应商"
              options={providers.map(p => ({
                label: p.provider,
                value: p.provider
              }))}
            />
          </Form.Item>
          <Form.Item
            label="模型类型"
            name="aiModelType"
            rules={[{ required: true, message: '请输入模型类型' }]}
          >
            <Input placeholder="请输入模型类型" />
          </Form.Item>
          <Form.Item
            label="上下文窗口大小"
            name="contextWindowTokens"
            rules={[{ required: true, message: '请输入上下文窗口大小' }]}
          >
            <Input placeholder="请输入上下文窗口大小" />
          </Form.Item>
          <Form.Item
            label="请求端点"
            name="endpoint"
            rules={[{ required: true, message: '请输入请求端点' }]}
          >
            <Input placeholder="请输入请求端点" />
          </Form.Item>
          <Form.Item
            label="API Key"
            name="key"
            rules={[{ required: true, message: '请输入 API Key' }]}
          >
            <Input.Password placeholder="请输入 API Key" />
          </Form.Item>
        </Form>
      </Modal>
    </Row>
  );
}
