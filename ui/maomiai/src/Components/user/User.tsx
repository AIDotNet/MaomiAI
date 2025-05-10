import { Form, Input, Button, Upload, message, Card, Row, Col, Avatar, Tooltip } from "antd";
import { UploadOutlined, UserOutlined } from "@ant-design/icons";
import "./User.css";
import { GetApiClient, UploadImage } from "../ServiceClient";
import { useEffect, useState } from "react";

export default function User() {
  const [passwordForm] = Form.useForm();
  const [userInfo, setUserInfo] = useState({
    avatar: "",
    userName: "",
    nickName: "",
  });
  const [messageApi, contextHolder] = message.useMessage();
  const [uploading, setUploading] = useState(false);

  const fetchUserInfo = async () => {
    let client = GetApiClient();
    const userInfoResponse = await client.api.user.info.get();
    if (userInfoResponse) {
      setUserInfo({
        avatar: userInfoResponse.avatar || "",
        userName: userInfoResponse.userName || "",
        nickName: userInfoResponse.nickName || "",
      });
    }
  };

  useEffect(() => {
    fetchUserInfo();
    return () => {};
  }, []);

  const handlePasswordSubmit = async (values: any) => {
    try {
      // Handle password update logic here
      console.log("Password:", values.password);
      messageApi.success("密码更新成功");
    } catch (error) {
      console.error("Password update error:", error);
      messageApi.error("密码更新失败");
    }
  };

  const handleAvatarUpload = async (file: File) => {
    const client = GetApiClient();
    if (!file) {
      messageApi.error("请选择一个文件");
      return;
    }

    try {
      setUploading(true);
      // 上传头像
      const preUploadResponse = await UploadImage(client, file, "UserAvatar");

      // 更新头像
      await client.api.user.uploadavatar.post({
        fileId: preUploadResponse.fileId,
      });

      messageApi.success("头像更新成功");
      // Refresh user information to update avatar
      fetchUserInfo();
    } catch (error) {
      console.error("upload file error:", error);
      const typedError = error as {
        detail?: string;
      };
      if (typedError.detail) {
        messageApi.error(typedError.detail);
      } else {
        messageApi.error("头像上传失败");
      }
    } finally {
      setUploading(false);
    }
  };

  return (
    <>
      {contextHolder}
      <Row justify="center" align="middle" className="user-container">
        <Col span={20}>
          <Card title="用户信息更新" className="user-card">
            {/* Flex container for avatar and user info */}
            <div style={{ display: "flex", alignItems: "center", marginBottom: "20px" }}>
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
                    size={100}
                    src={userInfo.avatar}
                    icon={<UserOutlined />}
                    style={{ cursor: 'pointer', marginRight: "20px" }}
                  />
                </Tooltip>
              </Upload>
              <div>
                <div>
                  <strong>用户名:</strong> {userInfo.userName}
                </div>
                <div>
                  <strong>用户昵称:</strong> {userInfo.nickName}
                </div>
              </div>
            </div>

            <Form
              form={passwordForm}
              name="passwordUpdate"
              onFinish={handlePasswordSubmit}
              layout="vertical"
              size="large"
              style={{ marginBottom: "40px" }}
            >
              <Form.Item
                name="password"
                label="修改密码"
                rules={[{ required: true, message: "请输入新密码" }]}
              >
                <Input.Password placeholder="请输入新密码" />
              </Form.Item>
              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  style={{ width: "200px" }}
                  block
                >
                  提交密码更新
                </Button>
              </Form.Item>
            </Form>
          </Card>
        </Col>
      </Row>
    </>
  );
}
