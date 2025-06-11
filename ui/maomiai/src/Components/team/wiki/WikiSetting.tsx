import { useState, useEffect } from "react";
import {
  Card,
  Form,
  Input,
  Button,
  message,
  Switch,
  InputNumber,
  Select,
  Space,
  Tooltip,
  App,
} from "antd";
import { GetApiClient } from "../../ServiceClient";
import { useParams } from "react-router";

export default function WikiSetting() {
  const [form] = Form.useForm();
  const [messageApi, contextHolder] = message.useMessage();
  const { modal } = App.useApp();
  const [loading, setLoading] = useState(false);
  const [clearingVectors, setClearingVectors] = useState(false);
  const [modelList, setModelList] = useState<any[]>([]);
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
        .type.modellist.post({
          aiModelType: "embedding",
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
      const response = await apiClient.api.wiki.byTeamId(teamId).config.get({
        queryParameters: {
          wikiId: wikiId,
        },
      });

      if (response) {
        form.setFieldsValue({
          embeddingBatchSize: response.embeddingBatchSize,
          embeddingDimensions: response.embeddingDimensions,
          embeddingModelId: response.embeddingModelId,
          embeddingModelTokenizer: response.embeddingModelTokenizer,
          maxRetries: response.maxRetries,
          isLock: response.isLock,
        });
      }
    } catch (error) {
      console.error("Failed to fetch wiki config:", error);
      messageApi.error("获取知识库配置失败");
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (values: any) => {
    if (!teamId || !wikiId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    try {
      setLoading(true);
      await apiClient.api.wiki.byTeamId(teamId).settings.config.post({
        wikiId: wikiId,
        ...values,
      });

      messageApi.success("保存成功");
    } catch (error) {
      console.error("Failed to update wiki config:", error);
      messageApi.error("保存失败");
    } finally {
      setLoading(false);
    }
  };

  const handleClearVectors = () => {
    console.log("handleClearVectors called");
    console.log("modal:", modal);
    console.log("teamId:", teamId, "wikiId:", wikiId);
    
    modal.confirm({
      title: "确认清空知识库向量",
      content: "此操作将删除所有已生成的向量数据，操作不可逆。确定要继续吗？",
      okText: "确定",
      cancelText: "取消",
      okType: "danger",
      onOk: clearWikiVectors,
    });
  };

  const clearWikiVectors = async () => {
    console.log("clearWikiVectors called");
    if (!teamId || !wikiId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    try {
      setClearingVectors(true);
      console.log("Calling clear API with:", { teamId, wikiId });
      
      await apiClient.api.wiki.byTeamId(teamId).wiki.clear.post({
        wikiId: wikiId,
      });

      console.log("Clear API call successful");
      messageApi.success("知识库向量已清空");
      // 重新获取配置，因为清空后isLock状态可能会改变
      await fetchWikiConfig();
    } catch (error) {
      console.error("Failed to clear wiki vectors:", error);
      messageApi.error("清空向量失败");
    } finally {
      setClearingVectors(false);
    }
  };

  return (
    <>
      {contextHolder}
      <Card title="知识库设置" loading={loading}>
        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          <Form.Item
            name="embeddingBatchSize"
            label="批处理大小"
            rules={[{ required: true, message: "请输入批处理大小" }]}
          >
            <InputNumber
              min={1}
              disabled={form.getFieldValue("isLock")}
              style={{ width: "100%" }}
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
              style={{ width: "100%" }}
            />
          </Form.Item>

          <Form.Item
            name="embeddingModelId"
            label="向量化模型"
            rules={[{ required: true, message: "请选择向量化模型" }]}
          >
            <Select
              disabled={form.getFieldValue("isLock")}
              options={modelList.map((model) => ({
                label: model.name + "(" + model.provider + ")",
                value: model.id,
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
                { label: "o200k", value: "o200k" },
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
              style={{ width: "100%" }}
            />
          </Form.Item>

          <Form.Item name="isLock" label="是否锁定" valuePropName="checked">
            <Space>
              <Switch disabled={form.getFieldValue("isLock")} checked={form.getFieldValue("isLock")}  checkedChildren="锁定" />
              {form.getFieldValue("isLock")
                ? "知识库已生成数据，禁止修改，如必须修改可点击："
                : "第一次向量化文档后自动锁定"}
              <Tooltip title="强制清空该文档的所有向量">
                <Button 
                  type="default" 
                  danger
                  loading={clearingVectors}
                  onClick={handleClearVectors}
                >
                  强制清空知识库向量
                </Button>
              </Tooltip>
            </Space>
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
