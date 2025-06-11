import React from 'react';
import { Tree, Button, Spin, Alert, Space, Typography } from 'antd';
import { PlusOutlined, FileTextOutlined, FolderOutlined, FolderOpenOutlined } from '@ant-design/icons';
import { useNoteContext } from '../context/NoteContext';
import type { TreeData } from '../context/NoteContext';

const { Title } = Typography;

// 自定义树节点标题渲染器
const renderTreeNodeTitle = (
  nodeData: TreeData, 
  onCreateNote: (parentId: string | null) => void
) => {
  return (
    <div 
      style={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center',
        width: '100%',
        paddingRight: '8px'
      }}
    >
      <Space size="small" style={{ flex: 1 }}>
        {nodeData.id === 'root' ? (
          <FolderOutlined style={{ color: '#1890ff' }} />
        ) : (
          <FileTextOutlined style={{ color: '#52c41a' }} />
        )}
        <span style={{ 
          fontSize: '14px',
          color: nodeData.id === 'root' ? '#1890ff' : '#262626'
        }}>
          {typeof nodeData.title === 'string' ? nodeData.title : '未命名'}
        </span>
      </Space>
      <Button
        type="text"
        size="small"
        icon={<PlusOutlined />}
        onClick={(e) => {
          e.stopPropagation();
          onCreateNote(nodeData.id === 'root' ? null : nodeData.id);
        }}
        style={{ 
          opacity: 0,
          transition: 'opacity 0.2s',
          padding: '4px'
        }}
        className="tree-add-button"
      />
    </div>
  );
};

export default function NoteTree() {
  const {
    treeData,
    selectedKeys,
    setSelectedKeys,
    expandedKeys,
    setExpandedKeys,
    loading,
    error,
    createNote,
    openNote,
    findNodeById
  } = useNoteContext();

  // 处理树节点选择
  const handleSelect = async (selectedKeys: React.Key[]) => {
    if (selectedKeys.length > 0) {
      const selectedKey = selectedKeys[0] as string;
      const selectedNode = findNodeById(treeData, selectedKey);
      
      if (selectedNode && selectedNode.isNote && selectedNode.noteId) {
        setSelectedKeys([selectedKey]);
        await openNote(selectedNode.noteId);
      }
    }
  };

  // 处理树节点展开
  const handleExpand = (expandedKeys: React.Key[]) => {
    setExpandedKeys(expandedKeys as string[]);
  };

  // 重新构建树数据，添加自定义渲染
  const buildTreeDataWithCustomTitle = (nodes: TreeData[]): any[] => {
    return nodes.map(node => ({
      ...node,
      title: renderTreeNodeTitle(node, createNote),
      children: node.children ? buildTreeDataWithCustomTitle(node.children as TreeData[]) : undefined,
      icon: (props: any) => {
        const expanded = props?.expanded || false;
        if (node.id === 'root') {
          return expanded ? <FolderOpenOutlined /> : <FolderOutlined />;
        }
        return <FileTextOutlined />;
      }
    }));
  };

  if (loading) {
    return (
      <div style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '200px',
        flexDirection: 'column'
      }}>
        <Spin size="large" />
        <div style={{ marginTop: '16px', color: '#666' }}>加载笔记目录...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div style={{ padding: '16px' }}>
        <Alert
          message="加载失败"
          description={error}
          type="error"
          showIcon
          action={
            <Button size="small" onClick={() => window.location.reload()}>
              重试
            </Button>
          }
        />
      </div>
    );
  }

  return (
    <div style={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      {/* 标题栏 */}
      <div style={{ 
        padding: '16px 16px 8px 16px',
        borderBottom: '1px solid #f0f0f0',
        background: '#fafafa'
      }}>
        <Space align="center" style={{ width: '100%', justifyContent: 'space-between' }}>
          <Title level={5} style={{ margin: 0, color: '#262626' }}>
            笔记目录
          </Title>
          <Button
            type="primary"
            size="small"
            icon={<PlusOutlined />}
            onClick={() => createNote(null)}
          >
            新建
          </Button>
        </Space>
      </div>

      {/* 树形结构 */}
      <div style={{ 
        flex: 1, 
        padding: '8px',
        overflow: 'auto'
      }}>
        <style>
          {`
            .ant-tree-list-holder-inner .tree-add-button {
              opacity: 0 !important;
            }
            .ant-tree-treenode:hover .tree-add-button {
              opacity: 0.7 !important;
            }
            .ant-tree-treenode:hover .tree-add-button:hover {
              opacity: 1 !important;
            }
            .ant-tree .ant-tree-node-content-wrapper {
              width: calc(100% - 24px);
            }
            .ant-tree .ant-tree-node-content-wrapper:hover {
              background-color: #f5f5f5;
            }
            .ant-tree .ant-tree-node-content-wrapper.ant-tree-node-selected {
              background-color: #e6f7ff;
            }
          `}
        </style>
        <Tree
          selectedKeys={selectedKeys}
          expandedKeys={expandedKeys}
          treeData={buildTreeDataWithCustomTitle(treeData)}
          onSelect={handleSelect}
          onExpand={handleExpand}
          showIcon
          blockNode
          style={{ 
            background: 'transparent'
          }}
        />
      </div>
    </div>
  );
} 