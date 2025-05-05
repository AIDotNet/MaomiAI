import React from "react";
import { Form, Input, Button, Upload, message, Card, Row, Col } from "antd";
import { UploadOutlined } from "@ant-design/icons";
import "./User.css";
import { GetApiClient } from "../ServiceClient";
import { GetFileMd5 } from "../../helper/Md5Helper";

export default function User() {
  const [passwordForm] = Form.useForm();
  const [avatarForm] = Form.useForm();
  const [messageApi, contextHolder] = message.useMessage();

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

  const handleAvatarSubmit = async (values: any) => {
    try {
      const client = GetApiClient();
      const file = values.avatar[0].originFileObj;
      if (!file) {
        messageApi.error("请选择一个文件");
        return;
      }

      const md5 = await GetFileMd5(file);
      const preUploadResponse = await client.api.store.pre_upload_image.post({
        contentType: file.type,
        fileName: file.name,
        mD5: md5,
        imageType: "UserAvatar",
        fileSize: file.size,
      });

      if (!preUploadResponse || !preUploadResponse.uploadUrl) {
        messageApi.error("获取预签名URL失败");
        throw new Error("获取预签名URL失败");
      }

      const uploadUrl = preUploadResponse.uploadUrl;
      if (!uploadUrl) {
        messageApi.error("获取预签名URL失败");
        throw new Error("获取预签名URL失败");
      }

      // 使用 fetch API 上传到预签名的 S3 URL
      const uploadResponse = await fetch(uploadUrl, {
        method: "PUT",
        body: file,
        headers: {
          "Content-Type": file.type,
        },
      });

      if (!uploadResponse.ok) {
        messageApi.error("文件上传失败");
        throw new Error("文件上传失败");
      }

      // 如果文件已存在，则直接使用文件ID
      await client.api.user.uploadavatar.post({
        fileId: preUploadResponse.fileId,
      });

      messageApi.success("头像更新成功");
      return;
    } catch (error) {
      console.error("Avatar update error:", error);
      const typedError = error as {
        detail?: string;
      };
      if (typedError.detail) {
        messageApi.error(typedError.detail);
      } else {
        messageApi.error("头像上传失败");
      }
    }
  };

  return (
    <>
      {contextHolder}
      <Row justify="center" align="middle" className="user-container">
        <Col span={20}>
          <Card title="用户信息更新" className="user-card">
            <Form
              form={passwordForm}
              name="passwordUpdate"
              onFinish={handlePasswordSubmit}
              layout="vertical"
              size="large"
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

            <Form
              form={avatarForm}
              name="avatarUpdate"
              onFinish={handleAvatarSubmit}
              layout="vertical"
              size="large"
            >
              <Form.Item
                name="avatar"
                label="修改头像"
                valuePropName="fileList"
                getValueFromEvent={(e) => e.fileList}
                rules={[{ required: true, message: "请选择一个头像文件" }]}
              >
                <Upload
                  name="avatar"
                  listType="picture"
                  maxCount={1}
                  beforeUpload={() => false}
                >
                  <Button icon={<UploadOutlined />}>选择头像</Button>
                </Upload>
              </Form.Item>
              <Form.Item>
                <Button
                  type="primary"
                  htmlType="submit"
                  style={{ width: "200px" }}
                  block
                >
                  提交头像更新
                </Button>
              </Form.Item>
            </Form>
          </Card>
        </Col>
      </Row>
    </>
  );
}
