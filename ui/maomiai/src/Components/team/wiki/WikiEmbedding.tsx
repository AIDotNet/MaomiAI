import { useState, useEffect } from "react";
import { useParams, useNavigate, useSearchParams } from "react-router";
import { Card, Form, Button, message, InputNumber, Select } from "antd";
import { GetApiClient } from "../../ServiceClient";
import type { 
    MaomiAIDocumentCoreHandlersEmbeddingocumentCommand,
    MaomiAIDocumentSharedQueriesResponseQueryWikiFileListItem
} from "../../../ApiClient/models";

export default function WikiEmbedding() {
  const [form] = Form.useForm();
  const [messageApi, contextHolder] = message.useMessage();
  const [loading, setLoading] = useState(false);
  const [documentInfo, setDocumentInfo] = useState<MaomiAIDocumentSharedQueriesResponseQueryWikiFileListItem | null>(null);
  const { teamId, wikiId } = useParams();
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const apiClient = GetApiClient();

  useEffect(() => {
    const documentId = searchParams.get('fileId');
    if (!documentId) {
      messageApi.error("缺少文件ID");
      navigate(`/app/team/${teamId}/wiki/${wikiId}/document`);
      return;
    }

    fetchDocumentInfo(documentId);
  }, [teamId, wikiId, searchParams]);

  const fetchDocumentInfo = async (documentId: string) => {
    if (!teamId || !wikiId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    try {
      setLoading(true);
      const response = await apiClient.api.wiki
        .byTeamId(teamId)
        .byWikiId(wikiId)
        .document.post({
          documentId: documentId
        });

      if (response) {
        setDocumentInfo(response);
      }
    } catch (error) {
      console.error("Failed to fetch document info:", error);
      messageApi.error("获取文档信息失败");
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (values: MaomiAIDocumentCoreHandlersEmbeddingocumentCommand) => {
    if (!teamId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    try {
      setLoading(true);
      await apiClient.api.wiki
        .byTeamId(teamId)
        .embedding.post(values);

      messageApi.success("向量化任务已提交");
      navigate(`/app/team/${teamId}/wiki/${wikiId}/document`);
    } catch (error) {
      console.error("Failed to submit embedding task:", error);
      messageApi.error("提交向量化任务失败");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      {contextHolder}
      <Card title="文档向量化" loading={loading}>
        {documentInfo && (
          <Form
            form={form}
            layout="vertical"
            onFinish={handleSubmit}
            initialValues={{
              documentId: searchParams.get('fileId'),
              wikiId: wikiId,
              tokenizer: "cl100k",
              maxTokensPerParagraph: 1000,
              overlappingTokens: 100
            }}
          >
            <Form.Item label="文件名">
              <span>{documentInfo.fileName}</span>
            </Form.Item>

            <Form.Item
              name="tokenizer"
              label="分词器"
              rules={[{ required: true, message: '请选择分词器' }]}
            >
              <Select
                placeholder="请选择分词器"
                options={[
                  { label: 'p50k', value: 'p50k' },
                  { label: 'cl100k', value: 'cl100k' },
                  { label: 'o200k', value: 'o200k' }
                ]}
              />
            </Form.Item>

            <Form.Item
              name="maxTokensPerParagraph"
              label="每段最大Token数"
              rules={[{ required: true, message: '请输入每段最大Token数' }]}
              tooltip="当对文档进行分段时，每个分段通常包含一个段落。此参数控制每个段落的最大token数量。"
            >
              <InputNumber min={1} max={4000} />
            </Form.Item>

            <Form.Item
              name="overlappingTokens"
              label="重叠Token数"
              rules={[{ required: true, message: '请输入重叠Token数' }]}
              tooltip="分段之间的重叠token数量，用于保持上下文的连贯性。"
            >
              <InputNumber min={0} max={1000} />
            </Form.Item>

            <Form.Item>
              <Button type="primary" htmlType="submit" loading={loading}>
                开始向量化
              </Button>
            </Form.Item>
          </Form>
        )}
      </Card>
    </>
  );
}
