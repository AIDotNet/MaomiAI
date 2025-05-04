import { Col, Row, Card, Form, Input, Button, message } from "antd";
import { useNavigate } from "react-router";
import { RsaHelper } from "../../helper/RsaHalper";
import "./Login.css";
import { GetServiceInfo, ServiceClient } from "../ServiceClient";

export default function Login() {
  const [form] = Form.useForm();
  const [messageApi, contextHolder] = message.useMessage();
  const navigate = useNavigate();

  const onFinish = async (values: any) => {
    try {
      const serviceInfo = await GetServiceInfo();

      if (serviceInfo === undefined) {
        messageApi.error("不能读取服务器信息");
        throw new Error("ServiceInfo is not defined");
      }
      const rsaPublicKey = serviceInfo.rsaPublic;
      console.log(serviceInfo);
      if (!rsaPublicKey) {
        messageApi.error("不能读取服务器信息");
        throw new Error("RSA public key is not defined");
      }

      const encryptedPassword = RsaHelper.encrypt(
        rsaPublicKey,
        values.password
      );
      const response = await ServiceClient.api.user.login.post({
        userName: values.username,
        password: encryptedPassword,
      });

      if (response) {
        messageApi.success("登录成功，正在重定向到主页");
        setTimeout(() => {
          navigate("/home");
        }, 1000);
      } else {
        messageApi.error("登录失败");
      }
    } catch (error) {
      const typedError = error as {
        detail?: string;
        errors?: Record<string, string[]>;
      };
      if (typedError.errors && Object.keys(typedError.errors).length > 0) {
        let errors = Object.entries(typedError.errors).map(
          ([fieldName, errorMessages]) => ({
            name: Object.keys(errorMessages)[0],
            errors: Object.values(errorMessages)[0],
          })
        );
        form.setFields(errors);
      } else if (typedError.detail) {
        messageApi.error(typedError.detail);
      } else {
        messageApi.error("登录失败");
      }
    }
  };

  return (
    <>
      {contextHolder}

      <Row
        justify="center"
        align="middle"
        style={{
          minHeight: "90vh",
          minWidth: "80vh",
          width: "80%",
          margin: "0 auto",
          background: "#f0f2f5",
        }}
      >
        <Col span={20}>
          <Card
            title={
              <div
                style={{
                  textAlign: "center",
                  fontSize: "24px",
                  fontWeight: "bold",
                }}
              >
                登录
              </div>
            }
            style={{
              boxShadow: "0 4px 8px rgba(0,0,0,0.1)",
              borderRadius: "8px",
            }}
          >
            <Form
              form={form}
              name="login"
              onFinish={onFinish}
              layout="vertical"
              size="large"
            >
              <Form.Item
                name="username"
                label="用户名"
                rules={[{ required: true, message: "请输入用户名" }]}
              >
                <Input placeholder="请输入您的用户名" />
              </Form.Item>

              <Form.Item
                name="password"
                label="密码"
                rules={[{ required: true, message: "请输入密码" }]}
              >
                <Input.Password placeholder="请输入密码" />
              </Form.Item>

              <Form.Item style={{ marginTop: "24px" }}>
                <Button
                  type="primary"
                  htmlType="submit"
                  block
                  size="large"
                  style={{ height: "40px", borderRadius: "4px" }}
                >
                  立即登录
                </Button>
              </Form.Item>

              <Form.Item style={{ marginBottom: 0, textAlign: "center" }}>
                <Button
                  type="link"
                  onClick={() => navigate("/register")}
                  style={{ fontSize: "14px" }}
                >
                  没有账号？点击注册
                </Button>
              </Form.Item>
            </Form>
          </Card>
        </Col>
      </Row>
    </>
  );
}