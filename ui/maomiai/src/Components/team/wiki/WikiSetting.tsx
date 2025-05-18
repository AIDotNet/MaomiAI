import { useState, useEffect } from "react";
import { Card, Form, Input, Button, message, Switch, InputNumber, Select } from "antd";
import { GetApiClient } from "../../ServiceClient";
import { useParams } from "react-router";
import type { 
    MaomiAIDocumentSharedQueriesQueryWikiConfigCommandResponse,
    MaomiAIDocumentSharedQueriesUpdateWikiConfigCommand,
    MaomiAIAiModelSharedQueriesQueryAiModelFunctionListCommand,
    MaomiAIAiModelSharedModelsAiNotKeyEndpoint
} from "../../../apiClient/models";

export default function WikiSetting() {
    const [form] = Form.useForm();
    const [messageApi, contextHolder] = message.useMessage();
    const [loading, setLoading] = useState(false);
    const [modelList, setModelList] = useState<MaomiAIAiModelSharedModelsAiNotKeyEndpoint[]>([]);
    const { teamId, wikiId } = useParams();
    const apiClient = GetApiClient();

    useEffect(() => {
        fetchWikiConfig();
        fetchModelList();
    }, [teamId, wikiId]);

    const fetchModelList = async () => {
        if (!teamId) {
            messageApi.error("缺少必要的参数");
            return;
        }

        try {
            const response = await apiClient.api.aimodel
                .byTeamId(teamId)
                .function_ailist.post({
                    aiModelFunction: 'TextEmbeddingGeneration'
                });

            if (response) {
                setModelList(response.aiModels || []);
            }
        } catch (error) {
            console.error("Failed to fetch model list:", error);
            messageApi.error("获取模型列表失败");
        }
    };

    const fetchWikiConfig = async () => {
        if (!teamId || !wikiId) {
            messageApi.error("缺少必要的参数");
            return;
        }

        try {
            setLoading(true);
            const response = await apiClient.api.wiki
                .byTeamId(teamId)
                .byWikiId(wikiId)
                .config.get();

            if (response) {
                form.setFieldsValue({
                    embeddingBatchSize: response.embeddingBatchSize,
                    embeddingDimensions: response.embeddingDimensions,
                    embeddingModelId: response.embeddingModelId,
                    embeddingModelTokenizer: response.embeddingModelTokenizer,
                    maxRetries: response.maxRetries,
                    isLock: response.isLock
                });
            }
        } catch (error) {
            console.error("Failed to fetch wiki config:", error);
            messageApi.error("获取知识库配置失败");
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (values: MaomiAIDocumentSharedQueriesUpdateWikiConfigCommand) => {
        if (!teamId || !wikiId) {
            messageApi.error("缺少必要的参数");
            return;
        }

        try {
            setLoading(true);
            await apiClient.api.wiki
                .byTeamId(teamId)
                .byWikiId(wikiId)
                .config.post(values);

            messageApi.success("保存成功");
        } catch (error) {
            console.error("Failed to update wiki config:", error);
            messageApi.error("保存失败");
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            {contextHolder}
            <Card title="知识库设置" loading={loading}>
                <Form
                    form={form}
                    layout="vertical"
                    onFinish={handleSubmit}
                >
                    <Form.Item
                        name="embeddingBatchSize"
                        label="批处理大小"
                        rules={[{ required: true, message: "请输入批处理大小" }]}
                    >
                        <InputNumber 
                            min={1} 
                            disabled={form.getFieldValue("isLock")}
                            style={{ width: '100%' }}
                        />
                    </Form.Item>

                    <Form.Item
                        name="embeddingDimensions"
                        label="维度"
                        rules={[{ required: true, message: "请输入维度" }]}
                    >
                        <InputNumber 
                            min={1} 
                            disabled={form.getFieldValue("isLock")}
                            style={{ width: '100%' }}
                        />
                    </Form.Item>

                    <Form.Item
                        name="embeddingModelId"
                        label="向量化模型"
                        rules={[{ required: true, message: "请选择向量化模型" }]}
                    >
                        <Select 
                            disabled={form.getFieldValue("isLock")}
                            options={modelList.map(model => ({
                                label: model.name + "(" + model.modelId + ")",
                                value: model.id
                            }))}
                        />
                    </Form.Item>

                    <Form.Item
                        name="embeddingModelTokenizer"
                        label="分词器"
                        rules={[{ required: true, message: "请选择分词器" }]}
                    >
                        <Select 
                            disabled={form.getFieldValue("isLock")}
                            options={[
                                { label: "50k", value: "50k" },
                                { label: "cl100k", value: "cl100k" },
                                { label: "o200k", value: "o200k" }
                            ]}
                        />
                    </Form.Item>

                    <Form.Item
                        name="maxRetries"
                        label="最大重试次数"
                        rules={[{ required: true, message: "请输入最大重试次数" }]}
                    >
                        <InputNumber 
                            min={0} 
                            disabled={form.getFieldValue("isLock")}
                            style={{ width: '100%' }}
                        />
                    </Form.Item>

                    <Form.Item
                        name="isLock"
                        label="是否锁定"
                        valuePropName="checked"
                    >
                        <Switch disabled={true} />
                    </Form.Item>

                    <Form.Item>
                        <Button 
                            type="primary" 
                            htmlType="submit"
                            disabled={form.getFieldValue("isLock")}
                        >
                            保存设置
                        </Button>
                    </Form.Item>
                </Form>
            </Card>
        </>
    );
}