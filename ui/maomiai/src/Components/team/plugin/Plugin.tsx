import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { Layout, List, Button, Space, message, Spin, Modal, Form, Input, Select, Row, Col, Upload, Progress, Table, Tooltip } from "antd";
import { PlusOutlined, DatabaseOutlined, MinusCircleOutlined, UploadOutlined, DeleteOutlined, EditOutlined, ExclamationCircleOutlined } from "@ant-design/icons";
import { GetApiClient } from "../../ServiceClient";
import { proxyRequestError } from "../../../helper/RequestError";
import { FileTypeHelper } from "../../../helper/FileTypeHelper";
import { GetFileMd5 } from "../../../helper/Md5Helper";
import type { UploadFile } from "antd/es/upload/interface";

const { Sider, Content } = Layout;
const { Option } = Select;
const { TextArea } = Input;

interface PluginGroup {
    id: string;
    name: string;
    description?: string;
}

interface PluginItem {
    id: string;
    name: string;
    summary?: string;
    path?: string;
    groupId?: string;
    groupName?: string;
}

interface ImportMCPForm {
    name: string;
    description?: string;
    serverUrl: string;
    headers?: { key: string; value: string }[];
    queries?: { key: string; value: string }[];
}

interface EditGroupForm {
    name: string;
    description?: string;
    serverUrl?: string;
    headers?: { key: string; value: string }[];
    queries?: { key: string; value: string }[];
}

interface OpenApiUploadStatus {
    file: File;
    status: "waiting" | "preuploading" | "uploading" | "completing" | "success" | "error";
    progress: number;
    message?: string;
    fileId?: string;
    uploadUrl?: string;
}

export default function Plugin() {
    const { teamId } = useParams<{ teamId: string }>();
    const [messageApi, contextHolder] = message.useMessage();
    const [loading, setLoading] = useState(false);
    const [groups, setGroups] = useState<PluginGroup[]>([]);
    const [selectedGroupId, setSelectedGroupId] = useState<string | null>(null);
    const [plugins, setPlugins] = useState<PluginItem[]>([]);
    const [pluginsLoading, setPluginsLoading] = useState(false);
    
    // MCP导入弹窗相关状态
    const [mcpModalVisible, setMcpModalVisible] = useState(false);
    const [mcpForm] = Form.useForm<ImportMCPForm>();
    const [mcpImporting, setMcpImporting] = useState(false);
    
    // 分组编辑弹窗相关状态
    const [editGroupModalVisible, setEditGroupModalVisible] = useState(false);
    const [editGroupForm] = Form.useForm<EditGroupForm>();
    const [editingGroup, setEditingGroup] = useState<PluginGroup | null>(null);
    const [editGroupLoading, setEditGroupLoading] = useState(false);
    
    // OpenAPI上传弹窗相关状态
    const [openApiModalVisible, setOpenApiModalVisible] = useState(false);
    const [uploadFiles, setUploadFiles] = useState<File[]>([]);
    const [uploadStatuses, setUploadStatuses] = useState<OpenApiUploadStatus[]>([]);
    const [uploading, setUploading] = useState(false);
    const [openApiName, setOpenApiName] = useState<string>('');

    // 获取插件分组列表
    const fetchPluginGroups = async () => {
        if (!teamId) {
            messageApi.error("团队ID不存在");
            return;
        }

        try {
            setLoading(true);
            const client = GetApiClient();
            
            // 使用API客户端获取插件分组列表
            const response = await client.api.plugin.byTeamId(teamId).grouplist.post({});
            
            if (response && response.items) {
                // 转换API响应格式到本地类型
                const mappedGroups: PluginGroup[] = response.items
                    .filter(item => item.id && item.name) // 过滤掉无效项
                    .map(item => ({
                        id: item.id!,
                        name: item.name!,
                        description: item.description || undefined
                    }));
                setGroups(mappedGroups);
            } else {
                setGroups([]);
            }
        } catch (error: any) {
            messageApi.error("获取插件分组失败");
            proxyRequestError(error, messageApi);
            console.error("获取插件分组失败:", error);
        } finally {
            setLoading(false);
        }
    };

    // 获取插件列表
    const fetchPlugins = async (groupId: string) => {
        if (!teamId) {
            messageApi.error("团队ID不存在");
            return;
        }

        try {
            setPluginsLoading(true);
            const client = GetApiClient();
            
            // 使用API客户端获取插件列表
            const response = await client.api.plugin.byTeamId(teamId).pluginlist.post({
                groupId: groupId
            });
            
            if (response && response.items) {
                // 转换API响应格式到本地类型
                const mappedPlugins: PluginItem[] = response.items
                    .filter(item => item.id && item.name) // 过滤掉无效项
                    .map(item => ({
                        id: item.id!,
                        name: item.name!,
                        summary: item.summary || undefined,
                        path: item.path || undefined,
                        groupId: item.groupId || undefined,
                        groupName: item.groupName || undefined
                    }));
                setPlugins(mappedPlugins);
            } else {
                setPlugins([]);
            }
        } catch (error: any) {
            messageApi.error("获取插件列表失败");
            proxyRequestError(error, messageApi);
            console.error("获取插件列表失败:", error);
        } finally {
            setPluginsLoading(false);
        }
    };

    // 处理分组点击
    const handleGroupClick = (group: PluginGroup) => {
        setSelectedGroupId(group.id);
        fetchPlugins(group.id);
    };

    // 处理编辑分组
    const handleEditGroup = async (e: React.MouseEvent, group: PluginGroup) => {
        e.stopPropagation(); // 阻止事件冒泡，避免触发分组点击
        
        if (!teamId) {
            messageApi.error("团队ID不存在");
            return;
        }

        try {
            setEditingGroup(group);
            setEditGroupModalVisible(true);
            
            // 获取分组详细信息
            const client = GetApiClient();
            const response = await client.api.plugin.byTeamId(teamId).grouplist.post({
                groupId: group.id
            });
            
            if (response && response.items && response.items.length > 0) {
                const groupDetail = response.items[0];
                
                // 解析 header 和 query 参数
                let headers: { key: string; value: string }[] = [];
                let queries: { key: string; value: string }[] = [];
                
                // 解析 header
                if (groupDetail.header) {
                    try {
                        const headerObj = JSON.parse(groupDetail.header);
                        headers = Object.entries(headerObj).map(([key, value]) => ({
                            key,
                            value: String(value)
                        }));
                    } catch (error) {
                        console.warn("解析 header 参数失败:", error);
                    }
                }
                
                // 解析 query
                if (groupDetail.query) {
                    try {
                        const queryObj = JSON.parse(groupDetail.query);
                        queries = Object.entries(queryObj).map(([key, value]) => ({
                            key,
                            value: String(value)
                        }));
                    } catch (error) {
                        console.warn("解析 query 参数失败:", error);
                    }
                }
                
                // 设置表单值
                editGroupForm.setFieldsValue({
                    name: groupDetail.name || group.name,
                    description: groupDetail.description || '',
                    serverUrl: groupDetail.server || '',
                    headers: headers,
                    queries: queries
                });
            } else {
                // 如果没有获取到详细信息，使用基本信息
                editGroupForm.setFieldsValue({
                    name: group.name,
                    description: group.description || '',
                    serverUrl: '',
                    headers: [],
                    queries: []
                });
            }
        } catch (error: any) {
            messageApi.error("获取分组详细信息失败");
            proxyRequestError(error, messageApi);
            console.error("获取分组详细信息失败:", error);
            
            // 出错时仍然打开弹窗，但只设置基本信息
            editGroupForm.setFieldsValue({
                name: group.name,
                description: group.description || '',
                serverUrl: '',
                headers: [],
                queries: []
            });
        }
    };

    // 提交分组编辑
    const handleEditGroupSubmit = async (values: EditGroupForm) => {
        if (!teamId || !editingGroup) {
            messageApi.error("缺少必要参数");
            return;
        }

        try {
            setEditGroupLoading(true);
            
            // 转换headers和queries为JSON字符串格式
            let headersJson = '{}'; // 默认为空对象
            let queriesJson = '{}'; // 默认为空对象
            
            if (values.headers && values.headers.length > 0) {
                const headers: Record<string, string> = {};
                values.headers.forEach(item => {
                    if (item.key && item.value) {
                        headers[item.key] = item.value;
                    }
                });
                if (Object.keys(headers).length > 0) {
                    headersJson = JSON.stringify(headers);
                }
            }
            
            if (values.queries && values.queries.length > 0) {
                const queries: Record<string, string> = {};
                values.queries.forEach(item => {
                    if (item.key && item.value) {
                        queries[item.key] = item.value;
                    }
                });
                if (Object.keys(queries).length > 0) {
                    queriesJson = JSON.stringify(queries);
                }
            }
            
            const client = GetApiClient();
            
            // 更新分组信息
            await client.api.plugin.byTeamId(teamId).update_group.post({
                groupId: editingGroup.id,
                name: values.name,
                description: values.description,
                serverUrl: values.serverUrl,
                header: headersJson,
                query: queriesJson
            });
            
            messageApi.success("分组信息更新成功");
            setEditGroupModalVisible(false);
            setEditingGroup(null);
            await fetchPluginGroups();
        } catch (error: any) {
            messageApi.error("更新分组信息失败");
            proxyRequestError(error, messageApi);
            console.error("更新分组信息失败:", error);
        } finally {
            setEditGroupLoading(false);
        }
    };

    // 取消编辑分组
    const handleEditGroupCancel = () => {
        setEditGroupModalVisible(false);
        setEditingGroup(null);
        editGroupForm.resetFields();
    };

    // 处理删除分组
    const handleDeleteGroup = async (e: React.MouseEvent, group: PluginGroup) => {
        console.log("删除分组按钮被点击", group);
        e.stopPropagation(); // 阻止事件冒泡，避免触发分组点击
        
        // 先用简单的confirm测试
        if (window.confirm(`确定要删除分组"${group.name}"吗？`)) {
            if (!teamId) {
                messageApi.error("团队ID不存在");
                return;
            }

            try {
                const client = GetApiClient();
                
                // 调用删除分组API
                await client.api.plugin.byTeamId(teamId).delete_group.delete({
                    groupId: group.id
                });
                
                messageApi.success("分组删除成功");
                
                // 如果删除的是当前选中的分组，清空选中状态
                if (selectedGroupId === group.id) {
                    setSelectedGroupId(null);
                    setPlugins([]);
                }
                
                // 重新获取分组列表
                await fetchPluginGroups();
            } catch (error: any) {
                messageApi.error("删除分组失败");
                proxyRequestError(error, messageApi);
                console.error("删除分组失败:", error);
            }
        }
    };

    // 处理导入MCP
    const handleImportMCP = () => {
        console.log("点击导入MCP按钮");
        console.log("当前modal状态:", mcpModalVisible);
        setMcpModalVisible(true);
        mcpForm.resetFields();
        console.log("设置modal状态为true");
    };

    // 处理URL变化，自动解析query参数
    const handleUrlChange = (url: string) => {
        try {
            const urlObj = new URL(url);
            const searchParams = urlObj.searchParams;
            
            // 如果URL中有query参数，自动填充到queries字段
            if (searchParams.toString()) {
                const queries: { key: string; value: string }[] = [];
                searchParams.forEach((value, key) => {
                    queries.push({ key, value });
                });
                
                // 设置queries字段
                mcpForm.setFieldValue('queries', queries);
                
                // 移除URL中的query参数
                const cleanUrl = `${urlObj.protocol}//${urlObj.host}${urlObj.pathname}`;
                mcpForm.setFieldValue('serverUrl', cleanUrl);
            }
        } catch (error) {
            // URL格式不正确时不处理
        }
    };

    // 处理MCP导入表单提交
    const handleMcpSubmit = async (values: ImportMCPForm) => {
        if (!teamId) {
            messageApi.error("团队ID不存在");
            return;
        }

        try {
            setMcpImporting(true);
            
            // 转换headers和queries为JSON字符串格式
            let headersJson = '{}'; // 默认为空对象
            let queriesJson = '{}'; // 默认为空对象
            
            if (values.headers && values.headers.length > 0) {
                const headers: Record<string, string> = {};
                values.headers.forEach(item => {
                    if (item.key && item.value) {
                        headers[item.key] = item.value;
                    }
                });
                if (Object.keys(headers).length > 0) {
                    headersJson = JSON.stringify(headers);
                }
            }
            
            if (values.queries && values.queries.length > 0) {
                const queries: Record<string, string> = {};
                values.queries.forEach(item => {
                    if (item.key && item.value) {
                        queries[item.key] = item.value;
                    }
                });
                if (Object.keys(queries).length > 0) {
                    queriesJson = JSON.stringify(queries);
                }
            }
            
            const submitData = {
                name: values.name,
                serverUrl: values.serverUrl,
                description: values.description,
                header: headersJson,
                query: queriesJson
            };
            
            const client = GetApiClient();
            
            // 使用API客户端导入MCP插件
            await client.api.plugin.byTeamId(teamId).import_mcp.post(submitData);
            
            messageApi.success("MCP插件导入成功");
            setMcpModalVisible(false);
            mcpForm.resetFields();
            // 重新获取分组列表以显示新导入的插件
            await fetchPluginGroups();
        } catch (error: any) {
            messageApi.error("导入MCP插件失败");
            proxyRequestError(error, messageApi);
            console.error("导入MCP插件失败:", error);
        } finally {
            setMcpImporting(false);
        }
    };

    // 取消MCP导入
    const handleMcpCancel = () => {
        setMcpModalVisible(false);
        mcpForm.resetFields();
    };

    // 处理导入OpenAPI
    const handleImportOpenAPI = () => {
        setOpenApiModalVisible(true);
        setUploadFiles([]);
        setUploadStatuses([]);
        setOpenApiName('');
    };

    // 处理文件选择
    const handleFileSelect = (file: File) => {
        // 检查文件是否已经存在
        if (uploadFiles.some((f) => f.name === file.name)) {
            messageApi.warning(`文件 ${file.name} 已经存在`);
            return false;
        }

        // 检查文件类型 (只支持 JSON 和 YAML)
        const fileExtension = file.name.toLowerCase().split('.').pop();
        if (!['json', 'yaml', 'yml'].includes(fileExtension || '')) {
            messageApi.error('只支持 JSON 和 YAML 格式的文件');
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

    // 删除文件
    const handleRemoveFile = (index: number) => {
        setUploadFiles((prev) => prev.filter((_, i) => i !== index));
        setUploadStatuses((prev) => prev.filter((_, i) => i !== index));
    };

    // 预上传文件
    const preUploadFile = async (file: File, index: number) => {
        try {
            if (!teamId) {
                throw new Error("Missing teamId");
            }

            // 获取文件类型
            const fileType = FileTypeHelper.getFileType(file);

            // 获取文件 MD5
            const md5 = await GetFileMd5(file);

            const client = GetApiClient();
            
            // 预上传
            const preUploadResponse = await client.api.plugin
                .byTeamId(teamId)
                .pre_upload_openapi.post({
                    fileName: file.name,
                    fileSize: file.size,
                    contentType: fileType,
                    mD5: md5,
                });

            if (!preUploadResponse?.fileId) {
                throw new Error("Invalid pre-upload response: missing fileId");
            }

            // 如果文件已存在，跳过上传步骤，但仍需要调用完成接口
            if (preUploadResponse.isExist) {
                // 更新状态为完成中
                setUploadStatuses((prev) =>
                    prev.map((status, i) =>
                        i === index
                            ? {
                                ...status,
                                status: "completing",
                                progress: 90,
                                message: "文件已存在，正在完成导入...",
                                fileId: preUploadResponse.fileId || undefined,
                            }
                            : status
                    )
                );

                return {
                    fileId: preUploadResponse.fileId || undefined,
                    uploadUrl: preUploadResponse.uploadUrl || undefined,
                    isExist: true,
                };
            }

            // 文件不存在，需要上传
            if (!preUploadResponse.uploadUrl) {
                throw new Error("Invalid pre-upload response: missing uploadUrl");
            }

            // 更新状态
            setUploadStatuses((prev) =>
                prev.map((status, i) =>
                    i === index
                        ? {
                            ...status,
                            status: "uploading",
                            progress: 30,
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
                            progress: 90,
                        }
                        : status
                )
            );

            return {
                fileId: preUploadResponse.fileId || undefined,
                uploadUrl: preUploadResponse.uploadUrl || undefined,
                isExist: false,
            };
        } catch (error) {
            console.error("Pre-upload failed:", error);
            setUploadStatuses((prev) =>
                prev.map((status, i) =>
                    i === index
                        ? {
                            ...status,
                            status: "error",
                            message: error instanceof Error ? error.message : "Unknown error",
                        }
                        : status
                )
            );
            return undefined;
        }
    };

    // 完成上传
    const completeUpload = async (index: number, fileId: string) => {
        if (!teamId) {
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
            await client.api.plugin
                .byTeamId(teamId)
                .complate_upload_openapi.post({
                    fileId: fileId,
                    name: openApiName.trim(),
                });

            setUploadStatuses((prev) => {
                const newStatuses = [...prev];
                const currentStatus = newStatuses[index];
                const isExistingFile = currentStatus.message?.includes("文件已存在");
                newStatuses[index] = {
                    ...newStatuses[index],
                    status: "success",
                    progress: 100,
                    message: isExistingFile ? "导入成功（文件已存在）" : "上传成功",
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

    // 处理上传确认
    const handleUploadConfirm = async () => {
        if (!openApiName.trim()) {
            messageApi.warning("请输入插件名称");
            return;
        }
        
        if (uploadFiles.length === 0) {
            messageApi.warning("请选择要上传的文件");
            return;
        }

        setUploading(true);

        // 更新所有文件状态为预上传中
        setUploadStatuses((prev) =>
            prev.map((status) => ({
                ...status,
                status: "preuploading",
                progress: 10,
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

                // 跳过已完成的文件
                if (uploadStatuses[i].status === "success") {
                    continue;
                }

                // 2. 完成上传（无论文件是否已存在，都需要调用）
                await completeUpload(i, preUploadSuccess.fileId!);
            } catch (error) {
                allSuccess = false;
                console.error("Upload error:", error);
            }
        }

        setUploading(false);

        // 如果所有文件都上传成功，关闭弹窗并刷新列表
        if (allSuccess) {
            messageApi.success("所有文件上传成功");
            setOpenApiModalVisible(false);
            setUploadFiles([]);
            setUploadStatuses([]);
            setOpenApiName('');
            await fetchPluginGroups();
        }
    };

    // 取消上传
    const handleUploadCancel = () => {
        setOpenApiModalVisible(false);
        setUploadFiles([]);
        setUploadStatuses([]);
        setOpenApiName('');
    };

    // 获取状态文本
    const getStatusText = (status: OpenApiUploadStatus["status"]) => {
        switch (status) {
            case "waiting":
                return "等待中";
            case "preuploading":
                return "准备上传";
            case "uploading":
                return "上传中";
            case "completing":
                return "完成中";
            case "success":
                return "成功";
            case "error":
                return "失败";
            default:
                return "未知";
        }
    };

    // 获取状态颜色
    const getStatusColor = (status: OpenApiUploadStatus["status"]) => {
        switch (status) {
            case "waiting":
                return "#d9d9d9";
            case "preuploading":
            case "uploading":
            case "completing":
                return "#1890ff";
            case "success":
                return "#52c41a";
            case "error":
                return "#ff4d4f";
            default:
                return "#d9d9d9";
        }
    };

    // 插件表格列定义
    const pluginColumns = [
        {
            title: '插件名称',
            dataIndex: 'name',
            key: 'name',
            width: 200,
        },
        {
            title: '描述',
            dataIndex: 'summary',
            key: 'summary',
            ellipsis: true,
        },
        {
            title: '路径',
            dataIndex: 'path',
            key: 'path',
            width: 300,
            render: (text: string) => (
                <span style={{ fontFamily: 'monospace', fontSize: '12px' }}>
                    {text}
                </span>
            ),
        },
        {
            title: '操作',
            key: 'action',
            width: 120,
            render: (_: any, record: PluginItem) => (
                <Space>
                    <Button type="link" size="small">
                        编辑
                    </Button>
                    <Button type="link" size="small" danger>
                        删除
                    </Button>
                </Space>
            ),
        },
    ];

    useEffect(() => {
        fetchPluginGroups();
    }, [teamId]);

    return (
        <>
            {contextHolder}
            <Layout style={{ height: '100%', background: '#fff' }}>
                <Sider 
                    width={300} 
                    style={{ 
                        background: '#fafafa', 
                        borderRight: '1px solid #f0f0f0',
                        padding: '16px 0'
                    }}
                >
                    <div style={{ padding: '0 16px', marginBottom: '16px' }}>
                        <h3 style={{ margin: 0, fontSize: '16px', fontWeight: 500 }}>插件分组</h3>
                    </div>
                    
                    <Spin spinning={loading}>
                        <List
                            dataSource={groups}
                            renderItem={(group) => (
                                <List.Item
                                    style={{ 
                                        padding: '12px 16px',
                                        margin: '0 8px',
                                        borderRadius: '6px',
                                        cursor: 'pointer',
                                        transition: 'background-color 0.2s',
                                        backgroundColor: selectedGroupId === group.id ? '#e6f7ff' : 'transparent',
                                        borderLeft: selectedGroupId === group.id ? '3px solid #1890ff' : '3px solid transparent'
                                    }}
                                    onMouseEnter={(e) => {
                                        if (selectedGroupId !== group.id) {
                                            e.currentTarget.style.backgroundColor = '#f5f5f5';
                                        }
                                    }}
                                    onMouseLeave={(e) => {
                                        if (selectedGroupId !== group.id) {
                                            e.currentTarget.style.backgroundColor = 'transparent';
                                        }
                                    }}
                                    onClick={() => handleGroupClick(group)}
                                >
                                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', width: '100%' }}>
                                        <List.Item.Meta
                                            title={<span style={{ fontSize: '14px' }}>{group.name}</span>}
                                            description={
                                                group.description ? (
                                                    <span style={{ fontSize: '12px', color: '#666' }}>
                                                        {group.description}
                                                    </span>
                                                ) : null
                                            }
                                        />
                                        <Space size="small">
                                            <Tooltip title="编辑分组">
                                                <Button
                                                    type="text"
                                                    size="small"
                                                    icon={<EditOutlined />}
                                                    onClick={(e) => handleEditGroup(e, group)}
                                                    style={{ 
                                                        opacity: 0.6,
                                                        transition: 'opacity 0.2s'
                                                    }}
                                                    onMouseEnter={(e) => {
                                                        e.currentTarget.style.opacity = '1';
                                                    }}
                                                    onMouseLeave={(e) => {
                                                        e.currentTarget.style.opacity = '0.6';
                                                    }}
                                                />
                                            </Tooltip>
                                            <Tooltip title="删除分组">
                                                <Button
                                                    type="text"
                                                    size="small"
                                                    icon={<DeleteOutlined />}
                                                    onClick={(e) => {
                                                        console.log("按钮onClick被触发");
                                                        handleDeleteGroup(e, group);
                                                    }}
                                                    style={{ 
                                                        color: '#ff4d4f'
                                                    }}
                                                    danger
                                                />
                                            </Tooltip>
                                        </Space>
                                    </div>
                                </List.Item>
                            )}
                            locale={{ emptyText: loading ? "加载中..." : "暂无分组" }}
                        />
                    </Spin>
                </Sider>
                
                <Content style={{ padding: '24px' }}>
                    {!selectedGroupId ? (
                        <>
                            <div style={{ marginBottom: '24px' }}>
                                <h2 style={{ margin: 0, fontSize: '20px', fontWeight: 500 }}>插件管理</h2>
                                <p style={{ margin: '8px 0 0 0', color: '#666' }}>
                                    管理和导入各类插件，扩展团队功能
                                </p>
                            </div>
                            
                            <Space size="large">
                                <Button 
                                    type="primary" 
                                    icon={<PlusOutlined />}
                                    size="large"
                                    onClick={handleImportMCP}
                                >
                                    导入MCP
                                </Button>
                                
                                <Button 
                                    icon={<DatabaseOutlined />}
                                    size="large"
                                    onClick={handleImportOpenAPI}
                                >
                                    导入OpenAPI
                                </Button>
                            </Space>
                        </>
                    ) : (
                        <>
                            <div style={{ marginBottom: '24px' }}>
                                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                    <div>
                                        <h2 style={{ margin: 0, fontSize: '20px', fontWeight: 500 }}>
                                            {groups.find(g => g.id === selectedGroupId)?.name} 的插件
                                        </h2>
                                        <p style={{ margin: '8px 0 0 0', color: '#666' }}>
                                            管理分组内的插件
                                        </p>
                                    </div>
                                    <Button onClick={() => setSelectedGroupId(null)}>
                                        返回分组列表
                                    </Button>
                                </div>
                            </div>
                            
                            <Table
                                dataSource={plugins}
                                columns={pluginColumns}
                                rowKey="id"
                                loading={pluginsLoading}
                                pagination={false}
                                locale={{ 
                                    emptyText: pluginsLoading ? "加载中..." : "暂无插件" 
                                }}
                            />
                        </>
                    )}
                </Content>
            </Layout>

            {/* MCP导入弹窗 */}
            {console.log("Modal渲染状态:", mcpModalVisible)}
            <Modal
                title="导入MCP插件"
                open={mcpModalVisible}
                onOk={() => mcpForm.submit()}
                onCancel={handleMcpCancel}
                confirmLoading={mcpImporting}
                width={800}
                destroyOnClose
            >
                <Form
                    form={mcpForm}
                    layout="vertical"
                    onFinish={handleMcpSubmit}
                    style={{ marginTop: '16px' }}
                    initialValues={{ headers: [], queries: [] }}
                >
                    <Form.Item
                        name="name"
                        label="插件名称"
                        rules={[
                            { required: true, message: '请输入插件名称' },
                            { max: 50, message: '插件名称不能超过50个字符' }
                        ]}
                    >
                        <Input placeholder="请输入插件名称" />
                    </Form.Item>

                    <Form.Item
                        name="serverUrl"
                        label="MCP服务地址"
                        rules={[
                            { required: true, message: '请输入MCP服务地址' },
                            { type: 'url', message: '请输入有效的URL地址' }
                        ]}
                    >
                        <Input 
                            placeholder="例如: https://example.com/mcp-server" 
                            onBlur={(e) => handleUrlChange(e.target.value)}
                        />
                    </Form.Item>

                    <Form.Item
                        name="description"
                        label="描述"
                        rules={[
                            { max: 200, message: '描述不能超过200个字符' }
                        ]}
                    >
                        <TextArea 
                            rows={3} 
                            placeholder="请输入插件描述（可选）"
                            showCount
                            maxLength={200}
                        />
                    </Form.Item>

                    {/* Headers配置 */}
                    <Form.Item label="请求头(Headers)">
                        <Form.List name="headers">
                            {(fields, { add, remove }) => (
                                <>
                                    {fields.map(({ key, name, ...restField }) => (
                                        <Row key={key} gutter={8} style={{ marginBottom: 8 }}>
                                            <Col span={10}>
                                                <Form.Item
                                                    {...restField}
                                                    name={[name, 'key']}
                                                    rules={[{ required: true, message: '请输入Header名称' }]}
                                                >
                                                    <Input placeholder="Header名称" />
                                                </Form.Item>
                                            </Col>
                                            <Col span={12}>
                                                <Form.Item
                                                    {...restField}
                                                    name={[name, 'value']}
                                                    rules={[{ required: true, message: '请输入Header值' }]}
                                                >
                                                    <Input placeholder="Header值" />
                                                </Form.Item>
                                            </Col>
                                            <Col span={2}>
                                                <MinusCircleOutlined 
                                                    onClick={() => remove(name)}
                                                    style={{ color: '#ff4d4f', fontSize: '16px', lineHeight: '32px' }}
                                                />
                                            </Col>
                                        </Row>
                                    ))}
                                    <Button
                                        type="dashed"
                                        onClick={() => add()}
                                        block
                                        icon={<PlusOutlined />}
                                    >
                                        添加Header
                                    </Button>
                                </>
                            )}
                        </Form.List>
                    </Form.Item>

                    {/* Query参数配置 */}
                    <Form.Item label="Query参数">
                        <Form.List name="queries">
                            {(fields, { add, remove }) => (
                                <>
                                    {fields.map(({ key, name, ...restField }) => (
                                        <Row key={key} gutter={8} style={{ marginBottom: 8 }}>
                                            <Col span={10}>
                                                <Form.Item
                                                    {...restField}
                                                    name={[name, 'key']}
                                                    rules={[{ required: true, message: '请输入参数名' }]}
                                                >
                                                    <Input placeholder="参数名" />
                                                </Form.Item>
                                            </Col>
                                            <Col span={12}>
                                                <Form.Item
                                                    {...restField}
                                                    name={[name, 'value']}
                                                    rules={[{ required: true, message: '请输入参数值' }]}
                                                >
                                                    <Input placeholder="参数值" />
                                                </Form.Item>
                                            </Col>
                                            <Col span={2}>
                                                <MinusCircleOutlined 
                                                    onClick={() => remove(name)}
                                                    style={{ color: '#ff4d4f', fontSize: '16px', lineHeight: '32px' }}
                                                />
                                            </Col>
                                        </Row>
                                    ))}
                                    <Button
                                        type="dashed"
                                        onClick={() => add()}
                                        block
                                        icon={<PlusOutlined />}
                                    >
                                        添加Query参数
                                    </Button>
                                </>
                            )}
                        </Form.List>
                    </Form.Item>
                </Form>
            </Modal>

            {/* 分组编辑弹窗 */}
            <Modal
                title="编辑分组信息"
                open={editGroupModalVisible}
                onOk={() => editGroupForm.submit()}
                onCancel={handleEditGroupCancel}
                confirmLoading={editGroupLoading}
                width={800}
                destroyOnClose
            >
                <Form
                    form={editGroupForm}
                    layout="vertical"
                    onFinish={handleEditGroupSubmit}
                    style={{ marginTop: '16px' }}
                    initialValues={{ headers: [], queries: [] }}
                >
                    <Form.Item
                        name="name"
                        label="分组名称"
                        rules={[
                            { required: true, message: '请输入分组名称' },
                            { max: 50, message: '分组名称不能超过50个字符' }
                        ]}
                    >
                        <Input placeholder="请输入分组名称" />
                    </Form.Item>

                    <Form.Item
                        name="serverUrl"
                        label="服务器地址"
                        rules={[
                            { type: 'url', message: '请输入有效的URL地址' }
                        ]}
                    >
                        <Input placeholder="例如: https://example.com/mcp-server（可选）" />
                    </Form.Item>

                    <Form.Item
                        name="description"
                        label="分组描述"
                        rules={[
                            { max: 200, message: '描述不能超过200个字符' }
                        ]}
                    >
                        <TextArea 
                            rows={3} 
                            placeholder="请输入分组描述（可选）"
                            showCount
                            maxLength={200}
                        />
                    </Form.Item>

                    {/* Headers配置 */}
                    <Form.Item label="请求头(Headers)">
                        <Form.List name="headers">
                            {(fields, { add, remove }) => (
                                <>
                                    {fields.map(({ key, name, ...restField }) => (
                                        <Row key={key} gutter={8} style={{ marginBottom: 8 }}>
                                            <Col span={10}>
                                                <Form.Item
                                                    {...restField}
                                                    name={[name, 'key']}
                                                    rules={[{ required: true, message: '请输入Header名称' }]}
                                                >
                                                    <Input placeholder="Header名称" />
                                                </Form.Item>
                                            </Col>
                                            <Col span={12}>
                                                <Form.Item
                                                    {...restField}
                                                    name={[name, 'value']}
                                                    rules={[{ required: true, message: '请输入Header值' }]}
                                                >
                                                    <Input placeholder="Header值" />
                                                </Form.Item>
                                            </Col>
                                            <Col span={2}>
                                                <MinusCircleOutlined 
                                                    onClick={() => remove(name)}
                                                    style={{ color: '#ff4d4f', fontSize: '16px', lineHeight: '32px' }}
                                                />
                                            </Col>
                                        </Row>
                                    ))}
                                    <Button
                                        type="dashed"
                                        onClick={() => add()}
                                        block
                                        icon={<PlusOutlined />}
                                    >
                                        添加Header
                                    </Button>
                                </>
                            )}
                        </Form.List>
                    </Form.Item>

                    {/* Query参数配置 */}
                    <Form.Item label="Query参数">
                        <Form.List name="queries">
                            {(fields, { add, remove }) => (
                                <>
                                    {fields.map(({ key, name, ...restField }) => (
                                        <Row key={key} gutter={8} style={{ marginBottom: 8 }}>
                                            <Col span={10}>
                                                <Form.Item
                                                    {...restField}
                                                    name={[name, 'key']}
                                                    rules={[{ required: true, message: '请输入参数名' }]}
                                                >
                                                    <Input placeholder="参数名" />
                                                </Form.Item>
                                            </Col>
                                            <Col span={12}>
                                                <Form.Item
                                                    {...restField}
                                                    name={[name, 'value']}
                                                    rules={[{ required: true, message: '请输入参数值' }]}
                                                >
                                                    <Input placeholder="参数值" />
                                                </Form.Item>
                                            </Col>
                                            <Col span={2}>
                                                <MinusCircleOutlined 
                                                    onClick={() => remove(name)}
                                                    style={{ color: '#ff4d4f', fontSize: '16px', lineHeight: '32px' }}
                                                />
                                            </Col>
                                        </Row>
                                    ))}
                                    <Button
                                        type="dashed"
                                        onClick={() => add()}
                                        block
                                        icon={<PlusOutlined />}
                                    >
                                        添加Query参数
                                    </Button>
                                </>
                            )}
                        </Form.List>
                    </Form.Item>
                </Form>
            </Modal>

            {/* OpenAPI 文件上传弹窗 */}
            <Modal
                title="导入OpenAPI文件"
                open={openApiModalVisible}
                onOk={handleUploadConfirm}
                onCancel={handleUploadCancel}
                confirmLoading={uploading}
                width={800}
                destroyOnClose
                okText="开始上传"
                cancelText="取消"
            >
                <div style={{ marginTop: '16px' }}>
                    <div style={{ marginBottom: '16px' }}>
                        <div style={{ marginBottom: '12px' }}>
                            <label style={{ display: 'block', marginBottom: '4px', fontWeight: 500 }}>
                                插件名称 <span style={{ color: '#ff4d4f' }}>*</span>
                            </label>
                            <Input
                                placeholder="请输入插件名称"
                                value={openApiName}
                                onChange={(e) => setOpenApiName(e.target.value)}
                                maxLength={50}
                                showCount
                            />
                        </div>
                        <div style={{ marginBottom: '4px' }}>
                            <label style={{ display: 'block', marginBottom: '4px', fontWeight: 500 }}>
                                选择文件
                            </label>
                        </div>
                        <Upload
                            beforeUpload={handleFileSelect}
                            multiple
                            showUploadList={false}
                            accept=".json,.yaml,.yml"
                        >
                            <Button icon={<UploadOutlined />} type="dashed" block>
                                选择OpenAPI文件 (支持 JSON/YAML 格式)
                            </Button>
                        </Upload>
                    </div>

                    {uploadFiles.length > 0 && (
                        <div>
                            <h4 style={{ marginBottom: '12px' }}>待上传文件:</h4>
                            {uploadFiles.map((file, index) => {
                                const status = uploadStatuses[index];
                                return (
                                    <div
                                        key={index}
                                        style={{
                                            border: '1px solid #f0f0f0',
                                            borderRadius: '6px',
                                            padding: '12px',
                                            marginBottom: '8px',
                                            backgroundColor: '#fafafa'
                                        }}
                                    >
                                        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '8px' }}>
                                            <span style={{ fontWeight: 500 }}>{file.name}</span>
                                            <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                                                <span style={{ color: getStatusColor(status.status), fontSize: '12px' }}>
                                                    {getStatusText(status.status)}
                                                </span>
                                                {status.status === "waiting" && (
                                                    <Button
                                                        type="text"
                                                        size="small"
                                                        icon={<DeleteOutlined />}
                                                        onClick={() => handleRemoveFile(index)}
                                                        style={{ color: '#ff4d4f' }}
                                                    />
                                                )}
                                            </div>
                                        </div>
                                        <div style={{ fontSize: '12px', color: '#666', marginBottom: '8px' }}>
                                            大小: {(file.size / 1024).toFixed(2)} KB
                                        </div>
                                        {status.status !== "waiting" && (
                                            <Progress
                                                percent={status.progress}
                                                size="small"
                                                status={
                                                    status.status === "error" ? "exception" : 
                                                    status.status === "success" ? "success" : "active"
                                                }
                                                showInfo={false}
                                            />
                                        )}
                                        {status.message && (
                                            <div style={{ 
                                                fontSize: '12px', 
                                                color: status.status === "error" ? '#ff4d4f' : '#52c41a',
                                                marginTop: '4px'
                                            }}>
                                                {status.message}
                                            </div>
                                        )}
                                    </div>
                                );
                            })}
                        </div>
                    )}

                    {uploadFiles.length === 0 && (
                        <div style={{ 
                            textAlign: 'center', 
                            color: '#999', 
                            padding: '40px 0',
                            border: '1px dashed #d9d9d9',
                            borderRadius: '6px'
                        }}>
                            请选择要上传的OpenAPI文件
                        </div>
                    )}
                </div>
            </Modal>
        </>
    );
}