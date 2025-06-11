import { useState, useEffect } from 'react';
import { Layout, Row, Col } from 'antd';
import { GetApiClient } from '../../ServiceClient';
import type { NoteTreeItem, QueryNoteTreeCommand } from '../../../apiClient/models';
import NoteTree from './components/NoteTree';
import NoteTabs from './components/NoteTabs';
import { NoteProvider, useNoteContext } from './context/NoteContext';

const { Content } = Layout;

// 主要的笔记组件
function NoteContent() {
  const { 
    loading, 
    setLoading, 
    error, 
    setError, 
    treeData, 
    setTreeData,
    fetchCatalog 
  } = useNoteContext();

  // 获取目录树数据
  useEffect(() => {
    fetchCatalog();
  }, [fetchCatalog]);

  if (loading || error) {
    return null; // 错误和加载状态由 NoteTree 组件处理
  }

  return (
    <Layout style={{ height: '100vh', background: '#fff' }}>
      <Content style={{ display: 'flex', height: '100%' }}>
        <Row style={{ width: '100%', height: '100%' }}>
          <Col 
            span={6} 
            style={{ 
              borderRight: '1px solid #f0f0f0',
              height: '100%',
              overflow: 'hidden'
            }}
          >
            <NoteTree />
          </Col>
          <Col span={18} style={{ height: '100%' }}>
            <NoteTabs />
          </Col>
        </Row>
      </Content>
    </Layout>
  );
}

// 导出的主组件，包装了Context Provider
export default function Note() {
  return (
    <NoteProvider>
      <NoteContent />
    </NoteProvider>
  );
}