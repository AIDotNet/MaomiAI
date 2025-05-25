import { useState, useEffect } from "react";
import { useParams, useNavigate, useSearchParams } from "react-router";
import { Card, Form, Button, message, InputNumber, Select, Table, Space, Tooltip } from "antd";
import { GetApiClient } from "../../ServiceClient";
import { ReloadOutlined } from "@ant-design/icons";
import { formatDateTime } from "../../../helper/DateTimeHelper";

export default function WikiEmbedding() {
  const [form] = Form.useForm();
  const [messageApi, contextHolder] = message.useMessage();
  const [loading, setLoading] = useState(false);
  const [clearLoading, setClearLoading] = useState(false);
  const [documentInfo, setDocumentInfo] = useState<any>(null);
  const [tasks, setTasks] = useState<any[]>([]);
  const [tasksLoading, setTasksLoading] = useState(false);
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
    fetchTasks(documentId);
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
        .document.info.post({
          wikiId: wikiId,
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

  const fetchTasks = async (documentId: string) => {
    if (!teamId || !wikiId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    try {
      setTasksLoading(true);
      const response = await apiClient.api.wiki
        .byTeamId(teamId)
        .document.tasks.post({
          wikiId: wikiId,
          documentId: documentId
        });

      if (response) {
        setTasks(response);
      }
    } catch (error) {
      console.error("Failed to fetch tasks:", error);
      messageApi.error("获取任务列表失败");
    } finally {
      setTasksLoading(false);
    }
  };

  const handleSubmit = async (values: any) => {
    if (!teamId || !wikiId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    const documentId = searchParams.get('fileId');
    if (!documentId) {
      messageApi.error("缺少文件ID");
      return;
    }

    try {
      setLoading(true);
      const command = {
        documentId: documentId,
        wikiId: wikiId,
        tokenizer: values.tokenizer,
        maxTokensPerParagraph: values.maxTokensPerParagraph,
        overlappingTokens: values.overlappingTokens
      };

      await apiClient.api.wiki
        .byTeamId(teamId)
        .document.embedding.post(command);

      messageApi.success("向量化任务已提交");
      fetchTasks(documentId);
    } catch (error) {
      console.error("Failed to submit embedding task:", error);
      messageApi.error("提交向量化任务失败");
    } finally {
      setLoading(false);
    }
  };

  const handleCancelTask = async (id: string, documentId: string) => {
    if (!teamId || !wikiId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    try {
      await apiClient.api.wiki
        .byTeamId(teamId)
        .document.canal_tasks.post({
          wikiId: wikiId,
          taskId: id,
          documentId: documentId
        });

      messageApi.success("任务已取消");
      fetchTasks(documentId);
    } catch (error) {
      console.error("Failed to cancel task:", error);
      messageApi.error("取消任务失败");
    }
  };

  const handleClearVectors = async () => {
    if (!teamId || !wikiId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    const documentId = searchParams.get('fileId');
    if (!documentId) {
      messageApi.error("缺少文件ID");
      return;
    }

    try {
      setClearLoading(true);
      await apiClient.api.wiki
        .byTeamId(teamId)
        .wiki.clear.post({
          wikiId: wikiId,
          documentId: documentId
        });

      messageApi.success("向量已清空");
      fetchTasks(documentId);
    } catch (error) {
      console.error("Failed to clear vectors:", error);
      messageApi.error("清空向量失败");
    } finally {
      setClearLoading(false);
    }
  };

  const canCancelTask = (state: any) => {
    return state.toLowerCase() === "None".toLowerCase() || state === "Wait".toLowerCase() || state === "Processing".toLowerCase();
  };

  const taskColumns = [
    {
      title: "任务ID",
      dataIndex: "id",
      key: "id",
      width: 220,
    },
    {
      title: "文件名",
      dataIndex: "fileName",
      key: "fileName",
      width: 200,
    },
    {
      title: "分词器",
      dataIndex: "tokenizer",
      key: "tokenizer",
      width: 120,
    },
    {
      title: "每段最大Token数",
      dataIndex: "maxTokensPerParagraph",
      key: "maxTokensPerParagraph",
      width: 150,
    },
    {
      title: "重叠Token数",
      dataIndex: "overlappingTokens",
      key: "overlappingTokens",
      width: 120,
    },
    {
      title: "状态",
      dataIndex: "state",
      key: "state",
      width: 120,
    },
    {
      title: "执行信息",
      dataIndex: "message",
      key: "message",
      width: 200,
    },
    {
      title: "创建时间",
      dataIndex: "createTime",
      key: "createTime",
      width: 180,
      render: (text: string) => formatDateTime(text),
    },
    {
      title: "操作",
      key: "action",
      width: 120,
      render: (_: any, record: any) => (
        <Space>
          {canCancelTask(record.state) && (
            <Button
              type="link"
              danger
              onClick={() => handleCancelTask(record.id!, record.documentId!)}
            >
              取消
            </Button>
          )}
        </Space>
      ),
    },
  ];

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
              <Space>
                <Button type="primary" htmlType="submit" loading={loading}>
                  开始向量化
                </Button>
                <Tooltip title="清空该文档的所有向量">
                  <Button 
                    type="default" 
                    onClick={handleClearVectors} 
                    loading={clearLoading}
                    danger
                  >
                    清空向量
                  </Button>
                </Tooltip>
              </Space>
            </Form.Item>
          </Form>
        )}
      </Card>

      <Card 
        title={
          <Space>
            任务列表
            <Button
              type="text"
              icon={<ReloadOutlined />}
              onClick={() => {
                const documentId = searchParams.get('fileId');
                if (documentId) {
                  fetchTasks(documentId);
                }
              }}
            />
          </Space>
        }
        style={{ marginTop: 16 }}
      >
        <Table
          columns={taskColumns}
          dataSource={tasks}
          rowKey="id"
          loading={tasksLoading}
          scroll={{ x: 1200 }}
          pagination={false}
        />
      </Card>
    </>
  );
}
