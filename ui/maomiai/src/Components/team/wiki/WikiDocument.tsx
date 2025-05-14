import { useState, useEffect } from "react";
import {
  Card,
  Input,
  Button,
  Upload,
  Modal,
  message,
  Space,
  Typography,
  Row,
  Col,
  Avatar,
  Table,
  List,
  Progress,
} from "antd";
import {
  UploadOutlined,
  SearchOutlined,
  TeamOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined,
  DeleteOutlined,
} from "@ant-design/icons";
import type { UploadFile, UploadProps } from "antd/es/upload/interface";
import { GetApiClient, UploadImage } from "../../../Components/ServiceClient";
import { GetFileMd5 } from "../../../helper/Md5Helper";
import { RcFile } from "antd/es/upload";
import { useParams, useNavigate } from "react-router";
import type { MaomiAIDocumentSharedQueriesQueryWikiFileListCommand } from "../../../ApiClient/models";
import { MaomiAIStoreEnumsUploadImageTypeObject } from "../../../ApiClient/models";
import { FileTypeHelper } from "../../../helper/FileTypeHelper";
import { FileSizeHelper } from "../../../helper/FileSizeHelper";
import {
  formatDateTime,
  parseJsonDateTime,
} from "../../../helper/DateTimeHelper";

const { Title } = Typography;
const { Meta } = Card;

interface TeamItem {
  id: string;
  name: string;
  description: string;
  avatarUrl?: string;
  isAdmin: boolean;
  isDisable: boolean;
  isPublic: boolean;
  isRoot: boolean;
  ownUserId: string;
  ownUserName: string;
  createTime: string;
}

interface DocumentItem {
  documentId: string;
  fileName: string;
  fileSize: string;
  contentType: string;
  createTime: string;
  createUserName: string;
  updateTime: string;
  updateUserName: string;
}

interface UploadStatus {
  file: File;
  status:
    | "waiting"
    | "preuploading"
    | "uploading"
    | "completing"
    | "success"
    | "error";
  progress: number;
  message?: string;
  fileId?: string;
  uploadUrl?: string;
}

export default function WikiDocument() {
  const params = useParams();
  const navigate = useNavigate();
  const [documents, setDocuments] = useState<DocumentItem[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchText, setSearchText] = useState("");
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    total: 0,
  });
  const [isUploadModalVisible, setIsUploadModalVisible] = useState(false);
  const [uploadFiles, setUploadFiles] = useState<File[]>([]);
  const [uploadStatuses, setUploadStatuses] = useState<UploadStatus[]>([]);

  const fetchDocuments = async (page = 1, pageSize = 10, search = "") => {
    if (!params.teamId || !params.wikiId) {
      message.error("缺少必要的参数");
      return;
    }

    try {
      setLoading(true);
      const client = GetApiClient();
      const requestBody: MaomiAIDocumentSharedQueriesQueryWikiFileListCommand =
        {
          pageNo: page,
          pageSize,
          search,
        };

      const response = await client.api.wiki
        .byTeamId(params.teamId)
        .byWikiId(params.wikiId)
        .documents.post(requestBody);

      if (response?.items) {
        const formattedDocuments: DocumentItem[] = response.items.map(
          (item) => ({
            documentId: item.documentId || "",
            fileName: item.fileName || "",
            fileSize:
              FileSizeHelper.formatFileSize(
                typeof item.fileSize === "string"
                  ? parseInt(item.fileSize, 10)
                  : item.fileSize ?? 0
              ) || "未知大小",
            contentType: item.contentType || "",
            createTime: parseJsonDateTime(item.createTime || ""),
            createUserName: item.createUserName || "",
            updateTime: parseJsonDateTime(item.updateTime || ""),
            updateUserName: item.updateUserName || "",
          })
        );

        setDocuments(formattedDocuments);
        setPagination({
          current: page,
          pageSize,
          total: response.total || 0,
        });
      }
    } catch (error) {
      message.error("获取文档列表失败");
      console.error("Fetch documents error:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDocuments();
  }, []);

  const handleSearch = () => {
    fetchDocuments(1, pagination.pageSize, searchText);
  };

  const handleTableChange = (newPagination: any) => {
    fetchDocuments(newPagination.current, newPagination.pageSize, searchText);
  };

  const handleUploadClick = () => {
    setIsUploadModalVisible(true);
  };

  const handleUploadModalCancel = () => {
    setIsUploadModalVisible(false);
    setUploadFiles([]);
    setUploadStatuses([]);
  };

  const handleFileSelect = (file: File) => {
    // 检查文件是否已经存在
    if (uploadFiles.some((f) => f.name === file.name)) {
      message.warning(`文件 ${file.name} 已经存在`);
      return false;
    }

    setUploadFiles((prev) => [...prev, file]);
    setUploadStatuses((prev) => [
      ...prev,
      {
        file,
        status: "waiting",
        progress: 0,
      },
    ]);
    return false;
  };

  const handleRemoveFile = (index: number) => {
    setUploadFiles((prev) => prev.filter((_, i) => i !== index));
    setUploadStatuses((prev) => prev.filter((_, i) => i !== index));
  };

  const preUploadFile = async (file: File, index: number) => {
    try {
      if (!params.teamId || !params.wikiId) {
        throw new Error("Missing teamId or wikiId");
      }

      // 获取文件类型
      const fileType = FileTypeHelper.getFileType(file);

      // 获取文件 MD5
      const md5 = await GetFileMd5(file);

      const client = GetApiClient();
      // 预上传
      const preUploadResponse = await client.api.wiki
        .byTeamId(params.teamId)
        .byWikiId(params.wikiId)
        .preupload.post({
          fileName: file.name,
          fileSize: file.size,
          contentType: fileType,
          mD5: md5,
        });

      if (!preUploadResponse?.uploadUrl || !preUploadResponse?.fileId) {
        throw new Error("Invalid pre-upload response");
      }

      // 更新状态
      setUploadStatuses((prev) =>
        prev.map((status, i) =>
          i === index
            ? {
                ...status,
                status: "uploading",
                fileId: preUploadResponse.fileId || undefined,
                uploadUrl: preUploadResponse.uploadUrl || undefined,
              }
            : status
        )
      );

      // 上传文件内容
      const uploadResponse = await fetch(preUploadResponse.uploadUrl, {
        method: "PUT",
        headers: {
          "Content-Type": fileType,
        },
        body: file,
      });

      if (!uploadResponse.ok) {
        throw new Error("Failed to upload file content");
      }

      // 更新状态
      setUploadStatuses((prev) =>
        prev.map((status, i) =>
          i === index
            ? {
                ...status,
                status: "completing",
                fileId: preUploadResponse.fileId || undefined,
                uploadUrl: preUploadResponse.uploadUrl || undefined,
              }
            : status
        )
      );

      return {
        fileId: preUploadResponse.fileId || undefined,
        uploadUrl: preUploadResponse.uploadUrl || undefined,
      };
    } catch (error) {
      console.error("Pre-upload failed:", error);
      setUploadStatuses((prev) =>
        prev.map((status, i) =>
          i === index
            ? {
                ...status,
                status: "error",
                error: error instanceof Error ? error.message : "Unknown error",
              }
            : status
        )
      );
      return undefined;
    }
  };

  const completeUpload = async (index: number, fileId: string) => {
    if (!params.teamId || !params.wikiId) {
      setUploadStatuses((prev) => {
        const newStatuses = [...prev];
        newStatuses[index] = {
          ...newStatuses[index],
          status: "error",
          progress: 0,
          message: "缺少必要的参数",
        };
        return newStatuses;
      });
      return false;
    }

    try {
      const client = GetApiClient();
      await client.api.wiki
        .byTeamId(params.teamId)
        .byWikiId(params.wikiId)
        .complate_upload.post({
          fileId: fileId,
          isSuccess: true,
        });

      setUploadStatuses((prev) => {
        const newStatuses = [...prev];
        newStatuses[index] = {
          ...newStatuses[index],
          status: "success",
          progress: 100,
          message: "上传成功",
        };
        return newStatuses;
      });

      return true;
    } catch (error) {
      setUploadStatuses((prev) => {
        const newStatuses = [...prev];
        newStatuses[index] = {
          ...newStatuses[index],
          status: "error",
          progress: 0,
          message: error instanceof Error ? error.message : "完成上传失败",
        };
        return newStatuses;
      });
      return false;
    }
  };

  const handleUploadConfirm = async () => {
    if (uploadFiles.length === 0) {
      message.warning("请选择要上传的文件");
      return;
    }

    // 更新所有文件状态为预上传中
    setUploadStatuses((prev) =>
      prev.map((status) => ({
        ...status,
        status: "preuploading",
        progress: 0,
      }))
    );

    let allSuccess = true;

    // 逐个处理文件
    for (let i = 0; i < uploadFiles.length; i++) {
      const file = uploadFiles[i];
      const status = uploadStatuses[i];

      // 跳过已完成的文件
      if (status.status === "success") {
        continue;
      }

      try {
        // 1. 预上传和上传文件
        const preUploadSuccess = await preUploadFile(file, i);
        if (!preUploadSuccess) {
          allSuccess = false;
          continue;
        }

        // 如果文件已存在，跳过后续步骤
        if (uploadStatuses[i].status === "success") {
          continue;
        }

        // 2. 完成上传
        await completeUpload(i, preUploadSuccess.fileId!);
      } catch (error) {
        allSuccess = false;
        console.error("Upload error:", error);
      }
    }

    // 如果所有文件都上传成功，关闭弹窗并刷新列表
    if (allSuccess) {
      message.success("所有文件上传成功");
      setIsUploadModalVisible(false);
      setUploadFiles([]);
      setUploadStatuses([]);
      fetchDocuments(pagination.current, pagination.pageSize, searchText);
    }
  };

  const columns = [
    {
      title: "文件名称",
      dataIndex: "fileName",
      key: "fileName",
      width: 200,
    },
    {
      title: "文件大小",
      dataIndex: "fileSize",
      key: "fileSize",
      width: 120,
    },
    {
      title: "文件类型",
      dataIndex: "contentType",
      key: "contentType",
      width: 120,
    },
    {
      title: "创建时间",
      dataIndex: "createTime",
      key: "createTime",
      width: 180,
      render: (text: string) => formatDateTime(text),
    },
    {
      title: "创建人",
      dataIndex: "createUserName",
      key: "createUserName",
      width: 120,
    },
    {
      title: "更新时间",
      dataIndex: "updateTime",
      key: "updateTime",
      width: 180,
      render: (text: string) => formatDateTime(text),
    },
    {
      title: "更新人",
      dataIndex: "updateUserName",
      key: "updateUserName",
      width: 120,
    },
    {
      title: "操作",
      key: "action",
      width: 80,  // http://localhost:4000/app/team/07eb6ca6-8a38-4555-8202-d6072f34f801/wiki/b00a896d-ad45-4e83-bc53-68a3b2c24e39/embedding?fileId=ce47b566-9070-4502-ad95-fd8a1f66123a
      render: (_: unknown, record: DocumentItem) => (
        <Button
          type="link"
          onClick={() => navigate(`/app/team/${params.teamId}/wiki/${params.wikiId}/embedding?fileId=${record.documentId}`)}
        >
          量化
        </Button>
      ),
    },
  ];

  const getStatusText = (status: UploadStatus["status"]) => {
    switch (status) {
      case "waiting":
        return "等待上传";
      case "preuploading":
        return "预上传中";
      case "uploading":
        return "上传中";
      case "completing":
        return "完成上传";
      case "success":
        return "上传成功";
      case "error":
        return "上传失败";
      default:
        return "";
    }
  };

  return (
    <div style={{ padding: "24px" }}>
      <Row
        justify="space-between"
        align="middle"
        style={{ marginBottom: "16px" }}
      >
        <Col>
          <Space>
            <Input
              placeholder="搜索文档"
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
              style={{ width: 200 }}
              onPressEnter={handleSearch}
            />
            <Button
              type="primary"
              icon={<SearchOutlined />}
              onClick={handleSearch}
            >
              查询
            </Button>
          </Space>
        </Col>
        <Col>
          <Button icon={<UploadOutlined />} onClick={handleUploadClick}>
            上传文档
          </Button>
        </Col>
      </Row>

      <Table
        columns={columns}
        dataSource={documents}
        rowKey="documentId"
        pagination={{
          ...pagination,
          showSizeChanger: true,
          showQuickJumper: true,
          showTotal: (total) => `共 ${total} 条`,
          pageSizeOptions: ["10", "20", "50", "100"],
        }}
        loading={loading}
        onChange={handleTableChange}
        scroll={{ x: 1200 }}
      />

      <Modal
        title="上传文档"
        open={isUploadModalVisible}
        onCancel={handleUploadModalCancel}
        width={600}
        footer={[
          <Button key="cancel" onClick={handleUploadModalCancel}>
            取消
          </Button>,
          <Button
            key="upload"
            type="primary"
            onClick={handleUploadConfirm}
            loading={uploadStatuses.some(
              (status) =>
                status.status === "preuploading" ||
                status.status === "uploading" ||
                status.status === "completing"
            )}
            disabled={uploadFiles.length === 0}
          >
            确认上传
          </Button>,
        ]}
      >
        <Upload.Dragger
          multiple
          showUploadList={false}
          beforeUpload={handleFileSelect}
        >
          <p className="ant-upload-drag-icon">
            <UploadOutlined />
          </p>
          <p className="ant-upload-text">点击或拖拽文件到此区域上传</p>
          <p className="ant-upload-hint">支持单个或批量上传</p>
        </Upload.Dragger>

        {uploadStatuses.length > 0 && (
          <List
            style={{ marginTop: 16 }}
            dataSource={uploadStatuses}
            renderItem={(status, index) => (
              <List.Item
                actions={[
                  <Button
                    key="delete"
                    type="text"
                    icon={<DeleteOutlined />}
                    onClick={() => handleRemoveFile(index)}
                    disabled={
                      status.status === "preuploading" ||
                      status.status === "uploading" ||
                      status.status === "completing"
                    }
                  />,
                ]}
              >
                <div style={{ width: "100%" }}>
                  <div
                    style={{
                      display: "flex",
                      alignItems: "center",
                      marginBottom: 8,
                    }}
                  >
                    <span style={{ flex: 1 }}>{status.file.name}</span>
                    {status.status === "success" && (
                      <CheckCircleOutlined style={{ color: "#52c41a" }} />
                    )}
                    {status.status === "error" && (
                      <CloseCircleOutlined style={{ color: "#ff4d4f" }} />
                    )}
                  </div>
                  <Progress
                    percent={status.progress}
                    status={status.status === "error" ? "exception" : undefined}
                    size="small"
                  />
                  <div
                    style={{
                      marginTop: 4,
                      color: status.status === "error" ? "#ff4d4f" : "#52c41a",
                    }}
                  >
                    {status.message || getStatusText(status.status)}
                  </div>
                </div>
              </List.Item>
            )}
          />
        )}
      </Modal>
    </div>
  );
}
