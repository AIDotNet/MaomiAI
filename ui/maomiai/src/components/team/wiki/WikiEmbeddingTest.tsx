import { useState, useEffect } from "react";
import { useParams } from "react-router";
import {
  Card,
  Select,
  Input,
  Button,
  Table,
  message,
  Space,
  Tag,
  Typography,
  Row,
  Col,
  Divider,
  Modal,
} from "antd";
import {
  SearchOutlined,
  FullscreenOutlined,
  FullscreenExitOutlined,
} from "@ant-design/icons";
import { GetApiClient } from "../../ServiceClient";
import type {
  QueryWikiDocumentListItem,
  SearchWikiDocumentTextCommandResponse,
  Citation,
  Citation_Partition,
} from "../../../apiClient/models";

const { Text } = Typography;

interface DocumentOption {
  value: string;
  label: string;
  documentId: string;
}

interface SearchResultItem {
  key: string;
  documentId: string;
  fileName: string;
  sourceContentType: string;
  partitionNumber: number;
  sectionNumber: number;
  relevance: number;
  text: string;
  lastUpdate: string;
}

export default function WikiEmbeddingTest() {
  const { teamId, wikiId } = useParams();
  const [messageApi, contextHolder] = message.useMessage();
  const [loading, setLoading] = useState(false);
  const [documents, setDocuments] = useState<DocumentOption[]>([]);
  const [selectedDocumentId, setSelectedDocumentId] = useState<
    string | undefined
  >();
  const [searchText, setSearchText] = useState("");
  const [searchResults, setSearchResults] = useState<SearchResultItem[]>([]);
  const [searchLoading, setSearchLoading] = useState(false);
  const [textModalVisible, setTextModalVisible] = useState(false);
  const [selectedText, setSelectedText] = useState("");
  const [selectedFileName, setSelectedFileName] = useState("");
  const [isFullscreen, setIsFullscreen] = useState(false);

  const apiClient = GetApiClient();

  // 获取已向量化的文档列表
  const fetchEmbeddedDocuments = async () => {
    if (!teamId || !wikiId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    try {
      setLoading(true);
      const response = await apiClient.api.wiki
        .byTeamId(teamId)
        .document.list.post({
          wikiId: wikiId,
          pageNo: 1,
          pageSize: 1000, // 获取所有文档
          query: "",
        });

      if (response?.items) {
        // 只显示已向量化的文档
        const embeddedDocs = response.items
          .filter((item: QueryWikiDocumentListItem) => item.embedding === true)
          .map((item: QueryWikiDocumentListItem) => ({
            value: item.documentId!,
            label: item.fileName!,
            documentId: item.documentId!,
          }));

        setDocuments(embeddedDocs);
      }
    } catch (error) {
      console.error("Failed to fetch documents:", error);
      messageApi.error("获取文档列表失败");
    } finally {
      setLoading(false);
    }
  };

  // 执行搜索
  const handleSearch = async () => {
    if (!searchText.trim()) {
      messageApi.warning("请输入搜索关键词");
      return;
    }

    if (!teamId || !wikiId) {
      messageApi.error("缺少必要的参数");
      return;
    }

    try {
      setSearchLoading(true);
      const command = {
        wikiId: wikiId,
        query: searchText.trim(),
        documentId: selectedDocumentId || undefined, // 如果没有选择文档则搜索整个知识库
      };

      const response: SearchWikiDocumentTextCommandResponse | undefined =
        await apiClient.api.wiki.byTeamId(teamId).document.search.post(command);

      if (response?.searchResult) {
        const { searchResult } = response;

        if (searchResult.noResult) {
          setSearchResults([]);
          messageApi.info("未找到相关结果");
          return;
        }

        if (searchResult.results && searchResult.results.length > 0) {
          const resultItems: SearchResultItem[] = [];

          searchResult.results.forEach(
            (citation: Citation, citationIndex: number) => {
              if (citation.partitions && citation.partitions.length > 0) {
                citation.partitions.forEach(
                  (partition: Citation_Partition, partitionIndex: number) => {
                    resultItems.push({
                      key: `${citationIndex}-${partitionIndex}`,
                      documentId: citation.documentId || "",
                      fileName: citation.sourceName || "",
                      sourceContentType: citation.sourceContentType || "",
                      partitionNumber: partition.partitionNumber || 0,
                      sectionNumber: partition.sectionNumber || 0,
                      relevance: partition.relevance || 0,
                      text: partition.text || "",
                      lastUpdate: partition.lastUpdate || "",
                    });
                  }
                );
              }
            }
          );

          setSearchResults(resultItems);
          messageApi.success(
            `已返回排名靠前的 ${resultItems.length} 个相关结果`
          );
        } else {
          setSearchResults([]);
          messageApi.info("未找到相关结果");
        }
      } else {
        setSearchResults([]);
        messageApi.info("未找到相关结果");
      }
    } catch (error) {
      console.error("Failed to search:", error);
      messageApi.error("搜索失败");
    } finally {
      setSearchLoading(false);
    }
  };

  // 组件初始化时获取文档列表
  useEffect(() => {
    fetchEmbeddedDocuments();
  }, [teamId, wikiId]);

  // 显示文本详情模态窗口
  const showTextModal = (text: string, fileName: string) => {
    setSelectedText(text);
    setSelectedFileName(fileName);
    setTextModalVisible(true);
    setIsFullscreen(false); // 重置全屏状态
  };

  // 切换全屏状态
  const toggleFullscreen = () => {
    setIsFullscreen(!isFullscreen);
  };

  // 关闭模态窗口
  const closeModal = () => {
    setTextModalVisible(false);
    setIsFullscreen(false);
  };

  // 搜索结果表格列配置
  const columns = [
    {
      title: "文档名称",
      dataIndex: "fileName",
      key: "fileName",
      width: 200,
      ellipsis: true,
    },
    {
      title: "文件类型",
      dataIndex: "sourceContentType",
      key: "sourceContentType",
      width: 200,
      ellipsis: true,
    },
    {
      title: "分段号",
      dataIndex: "partitionNumber",
      key: "partitionNumber",
      width: 80,
      align: "center" as const,
    },
    {
      title: "章节号",
      dataIndex: "sectionNumber",
      key: "sectionNumber",
      width: 80,
      align: "center" as const,
    },
    {
      title: "相关度",
      dataIndex: "relevance",
      key: "relevance",
      width: 100,
      align: "center" as const,
      render: (relevance: number) => (
        <Tag
          color={
            relevance > 0.8 ? "green" : relevance > 0.6 ? "orange" : "default"
          }
        >
          {(relevance * 100).toFixed(1)}%
        </Tag>
      ),
    },
    {
      title: "文本内容",
      dataIndex: "text",
      key: "text",
      ellipsis: true,
      render: (text: string, record: SearchResultItem) => (
        <Text
          style={{ fontSize: "12px", cursor: "pointer", color: "#1890ff" }}
          ellipsis={{ tooltip: "点击查看完整内容" }}
          onClick={() => showTextModal(text, record.fileName)}
        >
          {text}
        </Text>
      ),
    },
    {
      title: "最后更新",
      dataIndex: "lastUpdate",
      key: "lastUpdate",
      width: 150,
      render: (lastUpdate: string) => {
        if (!lastUpdate) return "-";
        try {
          return new Date(lastUpdate).toLocaleString("zh-CN");
        } catch {
          return lastUpdate;
        }
      },
    },
  ];

  return (
    <>
      {contextHolder}
      <Card title="向量化测试" loading={loading}>
        {/* 搜索区域 */}
        <Row gutter={16} align="middle">
          <Col span={6}>
            <Select
              placeholder="选择文档（可选）"
              allowClear
              style={{ width: "100%" }}
              value={selectedDocumentId}
              onChange={setSelectedDocumentId}
              options={documents}
              showSearch
              filterOption={(input, option) =>
                (option?.label ?? "")
                  .toLowerCase()
                  .includes(input.toLowerCase())
              }
            />
          </Col>
          <Col span={12}>
            <Input
              placeholder="请输入搜索关键词"
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
              onPressEnter={handleSearch}
              suffix={
                <Button
                  type="text"
                  icon={<SearchOutlined />}
                  onClick={handleSearch}
                  loading={searchLoading}
                />
              }
            />
          </Col>
          <Col span={6}>
            <Button
              type="primary"
              icon={<SearchOutlined />}
              onClick={handleSearch}
              loading={searchLoading}
              disabled={!searchText.trim()}
            >
              搜索
            </Button>
          </Col>
        </Row>

        <Divider />

        {/* 搜索结果表格 */}
        <Table
          columns={columns}
          dataSource={searchResults}
          loading={searchLoading}
          scroll={{ x: 1200 }}
          size="small"
        />
      </Card>

      {/* 文本详情模态窗口 */}
      <Modal
        title={`文本内容 - ${selectedFileName}`}
        open={textModalVisible}
        onCancel={closeModal}
        footer={[
          <Button
            key="fullscreen"
            icon={
              isFullscreen ? <FullscreenExitOutlined /> : <FullscreenOutlined />
            }
            onClick={toggleFullscreen}
          >
            {isFullscreen ? "退出全屏" : "全屏"}
          </Button>,
          <Button key="close" onClick={closeModal}>
            关闭
          </Button>,
        ]}
        width={isFullscreen ? "100vw" : 800}
        style={
          isFullscreen
            ? {
                top: 0,
                maxWidth: "100vw",
                margin: 0,
                paddingBottom: 0,
              }
            : {
                top: 20,
              }
        }
        bodyStyle={
          isFullscreen
            ? {
                height: "calc(100vh - 110px)",
                padding: "24px",
              }
            : {}
        }
      >
        <div
          style={{
            maxHeight: isFullscreen ? "calc(100vh - 150px)" : "60vh",
            overflow: "auto",
            padding: "16px",
            backgroundColor: "#fafafa",
            border: "1px solid #d9d9d9",
            borderRadius: "6px",
            lineHeight: "1.6",
            fontSize: "14px",
          }}
        >
          {selectedText}
        </div>
      </Modal>
    </>
  );
}
