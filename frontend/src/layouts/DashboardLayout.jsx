import React, { useState } from 'react';
import {
  HomeOutlined,
  DesktopOutlined,
  AppstoreAddOutlined,
  TeamOutlined
} from '@ant-design/icons';
import { Breadcrumb, Layout, Menu, theme } from 'antd';

import { Link } from 'react-router-dom';

const { Header, Content, Footer, Sider } = Layout;

function getItem(label, key, icon, path) {
  return {
    key,
    icon,
    label: path ? <Link to={path}>{label}</Link> : label,
  };
}
const items = [
  getItem('Home', '1', <HomeOutlined />, '/dashboard'),
  getItem('Clientes', '2', <TeamOutlined />, '/clients'),
  getItem('Productos', '3', <DesktopOutlined />, '/products'),
  getItem('Extras', '4', <AppstoreAddOutlined />, '/extras'),
];

const DashboardLayout = ({ children }) => {
  const [collapsed, setCollapsed] = useState(false);
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sider 
        collapsible 
        collapsed={collapsed} 
        onCollapse={value => setCollapsed(value)}
        breakpoint='lg'
        collapsedWidth='0'
        onBreakpoint={(broken) => {
          setCollapsed(broken);
        }}
      >
        <div className="demo-logo-vertical" style={{ height: 32, margin: 16 }} />
        <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline" items={items} />
      </Sider>
      <Layout>
        <Header style={{ padding: 0, background: colorBgContainer }} />
        <Content style={{ margin: '0 12px' }}>
          <div style={{ padding: '16px 0' }}>
            {children}
          </div>
        </Content>
        <Footer style={{ textAlign: 'center' }}>
          MyM Systems ©{new Date().getFullYear()}
        </Footer>
      </Layout>
    </Layout>
  );
};

export default DashboardLayout;