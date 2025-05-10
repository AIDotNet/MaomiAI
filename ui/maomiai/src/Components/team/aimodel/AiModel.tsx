import { useState, useEffect } from "react";
import { Card, Row, Col, Spin, message, List, Form, Input, Button, Modal, Select, Switch } from "antd";
import { GetApiClient } from "../../ServiceClient";
import { useParams } from "react-router";
import { 
    MaomiAIAiModelSharedModelsAiProviderInfo,
    MaomiAIAiModelSharedQueriesResponesQueryAiModelListCommandResponse,
    MaomiAIAiModelSharedModelsAiNotKeyEndpoint,
    MaomiAIAiModelSharedModelsAiEndpoint,
    MaomiAIAiModelSharedModelsAiModelFunction,
    MaomiAIAiModelSharedModelsAiModelFunctionObject
} from "../../../ApiClient/models";
import { RsaHelper } from "../../../helper/RsaHalper";
import { GetServiceInfo } from "../../../InitPage";

const FUNCTION_DESCRIPTIONS: Record<string, string> = {
    "None": "不具备任何功能，不可使用",
    "ChatCompletion": "对话模型",
    "TextGeneration": "文本生成模型",
    "TextEmbeddingGeneration": "嵌入模型",
    "TextToImage": "文本生成图像",
    "TextToAudio": "文本生成音频",
    "AudioToText": "语音识别"
};

class AiProviderInfo{
        /**
     * 默认端点.
     */
        defaultEndpoint?: string | null;
        /**
         * 描述.
         */
        description?: string | null;
        /**
         * Icon 图标地址.
         */
        icon?: string | null;
        /**
         * 名称.
         */
        name?: string | null;
        /**
         * 类型.
         */
    provider?: string | null;
    
    count?: number | null;
}

export default function AiModel() {
    const { teamId } = useParams();
    const [loading, setLoading] = useState(false);
    const [providers, setProviders] = useState<any | null>(null);
    const [supportProviders, setSupportProviders] = useState<MaomiAIAiModelSharedModelsAiProviderInfo[]>([]);
    const [messageApi, contextHolder] = message.useMessage();
    const apiClient = GetApiClient();
    const [expandedProvider, setExpandedProvider] = useState<string | null>(null);
    const [modelList, setModelList] = useState<MaomiAIAiModelSharedModelsAiNotKeyEndpoint[]>([]);
    const [selectedModel, setSelectedModel] = useState<MaomiAIAiModelSharedModelsAiNotKeyEndpoint | null>(null);
    const [loadingModels, setLoadingModels] = useState(false);
    const [isCreateModalVisible, setIsCreateModalVisible] = useState(false);
    const [currentProvider, setCurrentProvider] = useState<MaomiAIAiModelSharedModelsAiProviderInfo | null>(null);
    const [createForm] = Form.useForm();
    const [providerList, setProviderList] = useState<AiProviderInfo[]>([]);
    const [isModelModalVisible, setIsModelModalVisible] = useState(false);
    const [modelForm] = Form.useForm();

    const fetchData = async () => {
        if (!teamId) {
            messageApi.error("团队ID不存在");
            return;
        }
        try {
            setLoading(true);
            // 先获取支持的供应商列表
            const supportProviderResponse = await apiClient.api.aimodel.support_provider.get();
            if (!supportProviderResponse || !supportProviderResponse.providers) {
                messageApi.error("获取支持的供应商数据失败");
                return;
            }

            // 再获取供应商的模型数量
            const providerListResponse = await apiClient.api.aimodel.byTeamId(teamId).providerlist.get();
            if (!providerListResponse || !providerListResponse.providers) {
                messageApi.error("获取供应商数据失败");
                return;
            }

            // 设置数据
            setSupportProviders(supportProviderResponse.providers);
            setProviders(providerListResponse.providers || null);

            
            let providerGroup: AiProviderInfo[] = [];
            // 遍历已配置的模型列表，按provider聚合数量
            supportProviderResponse.providers.forEach(model => {
                let count = 0;
                const provider = providerListResponse.providers!.find(x => x.key == model.provider);
                if (provider) {
                    count = provider.value ?? 0;
                }
                providerGroup.push({
                    ...model,
                    count: count
                });
            });
            
            setProviderList(providerGroup);

        } catch (error) {
            console.error("Failed to fetch data:", error);
            messageApi.error("获取数据失败");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, [teamId]);

    const handleCardClick = async (provider: AiProviderInfo) => {
        setExpandedProvider(provider.provider || null);
        // Find the corresponding provider info from supportProviders
        const providerInfo = supportProviders.find(p => p.provider === provider.provider);
        if (providerInfo) {
            await fetchModelList(providerInfo);
        }
    };

    const handleModelClick = (model: MaomiAIAiModelSharedModelsAiNotKeyEndpoint) => {
        setSelectedModel(model);
        setIsModelModalVisible(true);
        // 设置表单初始值
        modelForm.setFieldsValue({
            name: model.name,
            modelId: model.modelId,
            deploymentName: model.deploymentName,
            enpoint: model.enpoint,
            function: model.aiFunction || [],
            isSupportFunctionCall: model.isSupportFunctionCall,
            isSupportImg: model.isSupportImg,
            key: ''
        });
    };

    const handleModelModalClose = () => {
        setIsModelModalVisible(false);
        setSelectedModel(null);
        modelForm.resetFields();
    };

    const handleModelSubmit = async () => {
        try {
            const values = await modelForm.validateFields();
            if (!selectedModel || !teamId) return;
            
            const serviceInfo = await GetServiceInfo();

            const encryptedKey = values.key ? 
                RsaHelper.encrypt(serviceInfo.rsaPublic, values.key) :
                "*";
            
            const endpoint: MaomiAIAiModelSharedModelsAiEndpoint = {
                name: values.name,
                modelId: values.modelId,
                deploymentName: values.deploymentName,
                enpoint: values.enpoint,
                provider: selectedModel.provider as any,
                aiFunction: values.function,
                isSupportFunctionCall: values.isSupportFunctionCall,
                isSupportImg: values.isSupportImg,
                key: encryptedKey
            };

            await apiClient.api.aimodel.byTeamId(teamId).update.post({
                endpoint: endpoint,
                modelId: selectedModel.id
            });

            messageApi.success("更新成功");
            setIsModelModalVisible(false);
            setSelectedModel(null);
            modelForm.resetFields();

            // 刷新供应商列表
            await fetchData();
            // 重新获取当前展开的供应商的模型列表
            const providerInfo = supportProviders.find(p => p.provider === selectedModel.provider);
            if (providerInfo) {
                await fetchModelList(providerInfo);
            }
        } catch (error) {
            console.error("Failed to update model:", error);
            messageApi.error("更新失败");
        }
    };

    const handleCreateClick = (e: React.MouseEvent, provider: AiProviderInfo) => {
        e.stopPropagation();
        const providerInfo = supportProviders.find(p => p.provider === provider.provider);
        if (providerInfo) {
            setCurrentProvider(providerInfo);
            setIsCreateModalVisible(true);
            createForm.resetFields();
            if (providerInfo.defaultEndpoint) {
                createForm.setFieldValue('enpoint', providerInfo.defaultEndpoint);
            }
        }
    };

    const handleCreateCancel = () => {
        setIsCreateModalVisible(false);
        setCurrentProvider(null);
        createForm.resetFields();
    };

    const handleCreateSubmit = async () => {
        try {
            const values = await createForm.validateFields();
            if (!currentProvider || !teamId) return;
            
            const serviceInfo = await GetServiceInfo();

            const encryptedKey = RsaHelper.encrypt(
                serviceInfo.rsaPublic,
                values.key
            );
            
            const endpoint: MaomiAIAiModelSharedModelsAiEndpoint = {
                name: values.name,
                modelId: values.modelId,
                deploymentName: values.deploymentName,
                enpoint: values.enpoint,
                provider: currentProvider.provider,
                aiFunction: values.function,
                isSupportFunctionCall: values.isSupportFunctionCall,
                isSupportImg: values.isSupportImg,
                key: encryptedKey
            };

            await apiClient.api.aimodel.byTeamId(teamId).create.post({
                endpoint: endpoint
            });

            messageApi.success("创建成功");
            setIsCreateModalVisible(false);
            setCurrentProvider(null);
            createForm.resetFields();

            // 刷新供应商列表
            await fetchData();
            // 重新获取当前展开的供应商的模型列表
            if (currentProvider) {
                await fetchModelList(currentProvider);
            }
        } catch (error) {
            console.error("Failed to create model:", error);
            messageApi.error("创建失败");
        }
    };

    const fetchModelList = async (provider: MaomiAIAiModelSharedModelsAiProviderInfo) => {
        if (!teamId || !provider.provider) return;
        try {
            setLoadingModels(true);
            const response = await apiClient.api.aimodel.byTeamId(teamId).modellist.post({
                provider: provider.provider
            });
            if (response) {
                setModelList(response.aiModels || []);
            }
        } catch (error) {
            console.error("Failed to fetch model list:", error);
            messageApi.error("获取模型列表失败");
        } finally {
            setLoadingModels(false);
        }
    };

    const getFunctionDisplay = (functions: MaomiAIAiModelSharedModelsAiModelFunction[] | null | undefined) => {
        if (!functions) return [];
        return functions.map(func => ({
            name: func,
            description: FUNCTION_DESCRIPTIONS[func] || '未知功能'
        }));
    };

    return (
        <>
            {contextHolder}
            <div style={{ padding: "24px" }}>
                <Spin spinning={loading}>
                    <div style={{ 
                        display: 'grid',
                        gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
                        gap: '16px',
                        gridAutoRows: 'auto',
                        gridAutoFlow: 'dense'
                    }}>
                        {providerList.map((provider: AiProviderInfo) => {
                            const modelCount = provider.count || 0;
                            const isExpanded = expandedProvider === provider.provider;
                            return (
                                <Card
                                    key={provider.provider}
                                    hoverable
                                    style={{ 
                                        height: "fit-content",
                                        marginBottom: 0
                                    }}
                                    bodyStyle={{ padding: "12px" }}
                                    onClick={() => handleCardClick(provider)}
                                >
                                    <div style={{ display: "flex", flexDirection: "column", gap: "8px" }}>
                                        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                            <h3 style={{ margin: 0, fontSize: "16px" }}>{provider.name}</h3>
                                            <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
                                                <span style={{ 
                                                    fontSize: "13px", 
                                                    color: "#1890ff",
                                                    backgroundColor: "#e6f7ff",
                                                    padding: "2px 8px",
                                                    borderRadius: "10px"
                                                }}>
                                                    {loading ? "加载中..." : `${modelCount} 个模型`}
                                                </span>
                                                <Button 
                                                    type="primary" 
                                                    size="small"
                                                    onClick={(e) => handleCreateClick(e, provider)}
                                                >
                                                    新建
                                                </Button>
                                            </div>
                                        </div>
                                        <p style={{ 
                                            margin: 0, 
                                            color: "#666", 
                                            fontSize: "13px",
                                            height: "36px",
                                            overflow: "hidden",
                                            textOverflow: "ellipsis",
                                            display: "-webkit-box",
                                            WebkitLineClamp: 2,
                                            WebkitBoxOrient: "vertical"
                                        }}>
                                            {provider.description}
                                        </p>
                                        {isExpanded && (
                                            <div style={{ marginTop: "12px" }}>
                                                <Spin spinning={loadingModels}>
                                                    <List
                                                        size="small"
                                                        dataSource={modelList}
                                                        renderItem={(model) => (
                                                            <List.Item 
                                                                onClick={(e) => {
                                                                    e.stopPropagation();
                                                                    handleModelClick(model);
                                                                }}
                                                                style={{ 
                                                                    cursor: "pointer",
                                                                    padding: "8px 12px",
                                                                    borderRadius: "8px",
                                                                    transition: "all 0.3s",
                                                                    border: "1px solid #f0f0f0",
                                                                    marginBottom: "8px"
                                                                }}
                                                                onMouseEnter={(e) => {
                                                                    e.currentTarget.style.backgroundColor = "#f5f5f5";
                                                                    e.currentTarget.style.borderColor = "#d9d9d9";
                                                                }}
                                                                onMouseLeave={(e) => {
                                                                    e.currentTarget.style.backgroundColor = "transparent";
                                                                    e.currentTarget.style.borderColor = "#f0f0f0";
                                                                }}
                                                            >
                                                                <div style={{ width: '100%' }}>
                                                                    <div style={{ 
                                                                        display: 'flex',
                                                                        alignItems: 'center',
                                                                        gap: '8px',
                                                                        marginBottom: '8px'
                                                                    }}>
                                                                        <div style={{ 
                                                                            fontSize: '14px',
                                                                            fontWeight: 500,
                                                                            color: '#262626'
                                                                        }}>
                                                                            {model.name}
                                                                        </div>
                                                                        <div style={{ 
                                                                            display: 'flex', 
                                                                            gap: '6px',
                                                                            fontSize: '12px'
                                                                        }}>
                                                                            {model.isSupportFunctionCall && (
                                                                                <span
                                                                                    style={{
                                                                                        backgroundColor: '#e6f7ff',
                                                                                        color: '#1890ff',
                                                                                        padding: '2px 8px',
                                                                                        borderRadius: '4px',
                                                                                        border: '1px solid #91d5ff'
                                                                                    }}
                                                                                    title="支持Function Call"
                                                                                >
                                                                                    Function Call
                                                                                </span>
                                                                            )}
                                                                            {model.isSupportImg && (
                                                                                <span
                                                                                    style={{
                                                                                        backgroundColor: '#f6ffed',
                                                                                        color: '#52c41a',
                                                                                        padding: '2px 8px',
                                                                                        borderRadius: '4px',
                                                                                        border: '1px solid #b7eb8f'
                                                                                    }}
                                                                                    title="支持图片"
                                                                                >
                                                                                    支持图片
                                                                                </span>
                                                                            )}
                                                                        </div>
                                                                    </div>
                                                                    <div style={{ 
                                                                        display: 'flex', 
                                                                        flexWrap: 'wrap', 
                                                                        gap: '6px',
                                                                        fontSize: '12px'
                                                                    }}>
                                                                        {getFunctionDisplay(model.aiFunction).map((func, index) => (
                                                                            <span 
                                                                                key={index}
                                                                                style={{
                                                                                    backgroundColor: '#f0f0f0',
                                                                                    padding: '2px 8px',
                                                                                    borderRadius: '4px',
                                                                                    color: '#666'
                                                                                }}
                                                                                title={func.description}
                                                                            >
                                                                                {func.name}
                                                                            </span>
                                                                        ))}
                                                                    </div>
                                                                </div>
                                                            </List.Item>
                                                        )}
                                                    />
                                                </Spin>
                                            </div>
                                        )}
                                    </div>
                                </Card>
                            );
                        })}
                    </div>
                </Spin>
            </div>

            <Modal
                title="新建模型"
                open={isCreateModalVisible}
                onCancel={handleCreateCancel}
                onOk={() => createForm.submit()}
                width={600}
            >
                <Form
                    form={createForm}
                    layout="vertical"
                    onFinish={handleCreateSubmit}
                >
                    <Form.Item 
                        label="模型供应商" 
                    >
                        <Input value={currentProvider?.name || ''} disabled />
                    </Form.Item>
                    <Form.Item 
                        label="模型名称" 
                        name="name"
                        rules={[{ required: true, message: '请输入模型名称' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item 
                        label="模型ID" 
                        name="modelId"
                        rules={[{ required: true, message: '请输入模型ID' }]}
                    >
                        <Input onBlur={(e) => {
                            const modelId = e.target.value;
                            if (modelId) {
                                createForm.setFieldValue('deploymentName', modelId);
                            }
                        }} />
                    </Form.Item>
                    <Form.Item 
                        label="部署名称" 
                        name="deploymentName"
                        rules={[{ required: true, message: '请输入部署名称' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item 
                        label="端点" 
                        name="enpoint"
                        rules={[{ required: true, message: '请输入端点' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item 
                        label="功能" 
                        name="function"
                        rules={[{ required: true, message: '请选择功能' }]}
                    >
                        <Select
                            mode="multiple"
                            placeholder="请选择功能"
                            optionLabelProp="label"
                        >
                            {Object.entries(FUNCTION_DESCRIPTIONS).map(([key, description]) => (
                                <Select.Option 
                                    key={key} 
                                    value={key}
                                    label={key}
                                >
                                    <div>
                                        <div>{key}</div>
                                        <div style={{ fontSize: '12px', color: '#999' }}>
                                            {description}
                                        </div>
                                    </div>
                                </Select.Option>
                            ))}
                        </Select>
                    </Form.Item>
                    <Form.Item 
                        label="API Key" 
                        name="key"
                        rules={[{ required: true, message: '请输入API Key' }]}
                    >
                        <Input.Password />
                    </Form.Item>
                    <Form.Item 
                        label="支持Function Call" 
                        name="isSupportFunctionCall"
                        valuePropName="checked"
                    >
                        <Switch />
                    </Form.Item>
                    <Form.Item 
                        label="支持图片" 
                        name="isSupportImg"
                        valuePropName="checked"
                    >
                        <Switch />
                    </Form.Item>
                </Form>
            </Modal>

            <Modal
                title="模型详情"
                open={isModelModalVisible}
                onCancel={handleModelModalClose}
                onOk={() => modelForm.submit()}
                width={600}
            >
                <Form
                    form={modelForm}
                    layout="vertical"
                    onFinish={handleModelSubmit}
                >
                    <Form.Item 
                        label="模型名称" 
                        name="name"
                        rules={[{ required: true, message: '请输入模型名称' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item 
                        label="模型ID" 
                        name="modelId"
                        rules={[{ required: true, message: '请输入模型ID' }]}
                    >
                        <Input onBlur={(e) => {
                            const modelId = e.target.value;
                            if (modelId) {
                                modelForm.setFieldValue('deploymentName', modelId);
                            }
                        }} />
                    </Form.Item>
                    <Form.Item 
                        label="部署名称" 
                        name="deploymentName"
                        rules={[{ required: true, message: '请输入部署名称' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item 
                        label="端点" 
                        name="enpoint"
                        rules={[{ required: true, message: '请输入端点' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item 
                        label="功能" 
                        name="function"
                        rules={[{ required: true, message: '请选择功能' }]}
                    >
                        <Select
                            mode="multiple"
                            placeholder="请选择功能"
                            optionLabelProp="label"
                        >
                            {Object.entries(FUNCTION_DESCRIPTIONS).map(([key, description]) => (
                                <Select.Option 
                                    key={key} 
                                    value={key}
                                    label={key}
                                >
                                    <div>
                                        <div>{key}</div>
                                        <div style={{ fontSize: '12px', color: '#999' }}>
                                            {description}
                                        </div>
                                    </div>
                                </Select.Option>
                            ))}
                        </Select>
                    </Form.Item>
                    <Form.Item 
                        label="API Key" 
                        name="key"
                    >
                        <Input.Password placeholder="不填写则保持原值" />
                    </Form.Item>
                    <Form.Item 
                        label="支持Function Call" 
                        name="isSupportFunctionCall"
                        valuePropName="checked"
                    >
                        <Switch />
                    </Form.Item>
                    <Form.Item 
                        label="支持图片" 
                        name="isSupportImg"
                        valuePropName="checked"
                    >
                        <Switch />
                    </Form.Item>
                </Form>
            </Modal>
        </>
    );
}