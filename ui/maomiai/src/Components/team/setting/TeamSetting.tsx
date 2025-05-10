import { useState, useEffect } from 'react';
import { Card, Form, Input, Button, Upload, message, Switch, Avatar, Descriptions, Space, Row, Col, Tooltip } from 'antd';
import { UserOutlined, UploadOutlined } from '@ant-design/icons';
import type { RcFile } from 'antd/es/upload';
import { GetApiClient, UploadImage } from '../../ServiceClient';
import { MaomiAIStoreEnumsUploadImageTypeObject, MaomiAITeamSharedQueriesResponsesQueryTeamDetailCommandResponse } from '../../../ApiClient/models';
import { useNavigate, useParams } from 'react-router';


export default function TeamSetting() {
    const [form] = Form.useForm();
    const [avatarForm] = Form.useForm();
    const [messageApi, contextHolder] = message.useMessage();
    const apiClient = GetApiClient();
    const [teamDetail, setTeamDetail] = useState<MaomiAITeamSharedQueriesResponsesQueryTeamDetailCommandResponse | null>(null);
    const [loading, setLoading] = useState(false);
    const [uploading, setUploading] = useState(false);
    const { teamId } = useParams();
    const navigate = useNavigate();
    // Fetch team details
    const fetchTeamDetail = async () => {
        if (!teamId) {
            messageApi.error("团队ID不存在");
            return;
          }
        try {
            setLoading(true);
            const response = await apiClient.api.team.byId(teamId).teamdetail.get();
            if (response) {
                setTeamDetail(response);
                // Update form with team details
                form.setFieldsValue({
                    name: response.name,
                    description: response.description,
                    isPublic: response.isPublic,
                    isDisable: response.isDisable,
                    markdown: response.markdown
                });
            }
        } catch (error) {
            console.error('Failed to fetch team details:', error);
            messageApi.error('获取团队详情失败');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchTeamDetail();
    }, [teamId]);

    // Handle team info update
    const handleTeamInfoUpdate = async (values: any) => {
        if (!teamId) {
            messageApi.error("团队ID不存在");
            return;
          }
        try {
            await apiClient.api.team.byId(teamId).update.post({
                name: values.name,
                description: values.description,
                isPublic: values.isPublic,
                isDisable: values.isDisable,
                markdown: values.markdown
            });
            messageApi.success('团队信息更新成功');
            // Refresh team details after update
            fetchTeamDetail();
        } catch (error) {
            console.error('Failed to update team info:', error);
            const typedError = error as {
              detail?: string;
            };
            if (typedError.detail) {
              messageApi.error(typedError.detail);
            } else {
              messageApi.error("更新团队信息失败");
            }
            messageApi.error("更新团队信息失败");
            return;
        }
    };

    // Handle avatar upload
    const handleAvatarUpload = async (file: File) => {
        if (!file) {
            messageApi.error('请选择一个文件');
            return;
        }

        try {
            setUploading(true);
            // 上传头像
            const preUploadResponse = await UploadImage(apiClient, file, MaomiAIStoreEnumsUploadImageTypeObject.TeamAvatar);

            // 更新头像
            await apiClient.api.team.config.uploadavatar.post({
                fileId: preUploadResponse.fileId,
                teamId: teamId
            });

            messageApi.success('头像更新成功');
            // 刷新团队信息
            fetchTeamDetail();
        } catch (error) {
            console.error('upload file error:', error);
            const typedError = error as {
                detail?: string;
            };
            if (typedError.detail) {
                messageApi.error(typedError.detail);
            } else {
                messageApi.error('头像上传失败');
            }
        } finally {
            setUploading(false);
        }
    };

    return (
        <>
            {contextHolder}
            <Card title="团队设置" loading={loading}>
                <Form
                    form={form}
                    layout="vertical"
                    onFinish={handleTeamInfoUpdate}
                >
                    <Form.Item
                        label="团队头像"
                    >
                        <Upload
                            name="avatar"
                            showUploadList={false}
                            beforeUpload={(file) => {
                                handleAvatarUpload(file);
                                return false;
                            }}
                            accept="image/*"
                        >
                            <Tooltip title="点击更换头像">
                                <Avatar 
                                    size={64} 
                                    src={teamDetail?.avatarUrl} 
                                    icon={<UserOutlined />}
                                    style={{ cursor: 'pointer' }}
                                />
                            </Tooltip>
                        </Upload>
                    </Form.Item>

                    <Form.Item
                        name="name"
                        label="团队名称"
                        rules={[{ required: true, message: '请输入团队名称' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        name="description"
                        label="团队描述"
                        rules={[{ required: true, message: '请输入团队描述' }]}
                    >
                        <Input.TextArea rows={4} />
                    </Form.Item>
                    <Form.Item
                        name="markdown"
                        label="团队详细介绍"
                    >
                        <Input.TextArea rows={6} />
                    </Form.Item>
                    <Form.Item
                        name="isPublic"
                        label="是否公开"
                        valuePropName="checked"
                    >
                        <Switch />
                    </Form.Item>
                    <Form.Item
                        name="isDisable"
                        label="是否禁用"
                        valuePropName="checked"
                    >
                        <Switch />
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" htmlType="submit">
                            保存设置
                        </Button>
                    </Form.Item>
                </Form>
            </Card>
        </>
    );
} 