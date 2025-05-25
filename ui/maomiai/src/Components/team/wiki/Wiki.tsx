import { message, Layout, Menu } from "antd";
import { Content } from "antd/es/layout/layout";
import { Outlet, useLocation, useNavigate, useParams, Link } from "react-router";
import { FileSearchOutlined, FileTextOutlined, SendOutlined, SettingOutlined } from "@ant-design/icons";

const { Sider } = Layout;

export default function Wiki() {
  const [messageApi, contextHolder] = message.useMessage();
  const location = useLocation();
  const navigate = useNavigate();
  const { teamId, wikiId } = useParams();

  // Get the current active menu item based on the route
  const getSelectedKey = () => {
    const path = location.pathname;
    if (path.includes("/setting")) return "setting";
    if (path.includes("/embeddingtest")) return "embeddingtest";
    if (path.includes("/embedding")) return "embedding";
    return "document"; // default
  };

  const menuItems = [
    {
      key: "document",
      icon: <FileTextOutlined />,
      label: <Link to={`/app/team/${teamId}/wiki/${wikiId}/document`}>文档</Link>,
    },
    {
      key: "embedding",
      icon: <SendOutlined />,
      label: <Link to={`/app/team/${teamId}/wiki/${wikiId}/embedding`}>向量化</Link>,
    },
    {
      key: "embeddingtest",
      icon: <FileSearchOutlined />,
      label: <Link to={`/app/team/${teamId}/wiki/${wikiId}/embeddingtest`}>向量化测试</Link>,
    },
    {
      key: "setting",
      icon: <SettingOutlined />,
      label: <Link to={`/app/team/${teamId}/wiki/${wikiId}/setting`}>设置</Link>,
    },
  ];

  return (
    <>
      {contextHolder}
      <Layout style={{ minHeight: "100vh" }}>
        <Sider width={200} theme="light">
          <Menu
            mode="inline"
            selectedKeys={[getSelectedKey()]}
            style={{ height: "100%" }}
            items={menuItems}
          />
        </Sider>
        <Content style={{ padding: "24px" }}>
          <Outlet />
        </Content>
      </Layout>
    </>
  );
}
