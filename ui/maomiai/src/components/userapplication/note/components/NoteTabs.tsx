import React, { useEffect } from 'react';
import { Tabs, Empty, Space } from 'antd';
import { FileTextOutlined } from '@ant-design/icons';
import { useNoteContext } from '../context/NoteContext';
import NoteEditor from './NoteEditor';

const { TabPane } = Tabs;

export default function NoteTabs() {
  const {
    openTabs,
    activeTabKey,
    closeTab,
    switchTab
  } = useNoteContext();

  // 处理标签页切换
  const handleTabChange = (activeKey: string) => {
    switchTab(activeKey);
  };

  // 处理标签页关闭
  const handleTabEdit = (targetKey: string | React.MouseEvent | React.KeyboardEvent, action: 'add' | 'remove') => {
    if (action === 'remove') {
      closeTab(targetKey as string);
    }
  };

  if (openTabs.length === 0) {
    return (
      <div style={{ 
        height: '100%',
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center',
        background: '#fafafa'
      }}>
        <Empty
          image={Empty.PRESENTED_IMAGE_SIMPLE}
          description={
            <Space direction="vertical" align="center">
              <FileTextOutlined style={{ fontSize: '48px', color: '#d9d9d9' }} />
              <span style={{ color: '#999', fontSize: '16px' }}>
                请从左侧选择一个笔记
              </span>
            </Space>
          }
        />
      </div>
    );
  }

  return (
    <div style={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      <Tabs
        type="editable-card"
        activeKey={activeTabKey}
        onChange={handleTabChange}
        onEdit={handleTabEdit}
        style={{ 
          height: '100%', 
          display: 'flex', 
          flexDirection: 'column',
          margin: 0
        }}
        tabBarStyle={{ 
          margin: 0,
          paddingLeft: '16px',
          paddingRight: '16px',
          borderBottom: '1px solid #f0f0f0',
          background: '#fff',
          flexShrink: 0
        }}
        destroyInactiveTabPane={false}
      >
        {openTabs.map(tab => (
          <TabPane
            tab={
              <Space size="small">
                <span>{tab.titleEmoji || '📝'}</span>
                <span>{tab.title || '未命名笔记'}</span>
              </Space>
            }
            key={tab.key}
            closable
            style={{ 
              height: '100%',
              overflow: 'hidden',
              display: 'flex',
              flexDirection: 'column'
            }}
          >
            <div style={{
              height: '100%',
              display: 'flex',
              flexDirection: 'column'
            }}>
              <NoteEditor noteTab={tab} />
            </div>
          </TabPane>
        ))}
      </Tabs>
    </div>
  );
} 