import React, { useState, useEffect, useRef, useCallback } from 'react';
import { Card, Input, Button, Space, message, Typography } from 'antd';
import { EditOutlined, SaveOutlined } from '@ant-design/icons';
import { EmojiPicker } from '@lobehub/ui';
import { useNoteContext, type NoteTab } from '../context/NoteContext';
import Cherry from 'cherry-markdown';
import 'cherry-markdown/dist/cherry-markdown.css';

const { Title } = Typography;

interface NoteEditorProps {
  noteTab: NoteTab;
}

export default function NoteEditor({ noteTab }: NoteEditorProps) {
  const { updateNote, saveNoteContent, setOpenTabs, cherryInstances } = useNoteContext();
  const [isEditingTitle, setIsEditingTitle] = useState(false);
  const [titleValue, setTitleValue] = useState(noteTab.title);
  const [hasUnsavedChanges, setHasUnsavedChanges] = useState(false);
  const editorRef = useRef<HTMLDivElement>(null);
  const saveTimeoutRef = useRef<NodeJS.Timeout | undefined>(undefined);

  // 保存内容
  const handleSave = useCallback(async (content?: string) => {
    try {
      const cherry = cherryInstances.current[noteTab.key];
      const saveContent = content || (cherry ? cherry.getValue() : noteTab.content);
      
      await saveNoteContent(noteTab.key, saveContent || '');
      setHasUnsavedChanges(false);
    } catch (error) {
      console.error('保存失败:', error);
      message.error('保存失败');
    }
  }, [noteTab.key, noteTab.content, saveNoteContent]);

  // 初始化Cherry编辑器
  const initCherry = useCallback(() => {
    if (!editorRef.current) return;

    const containerId = `cherry-${noteTab.key}`;
    
    // 清理旧实例
    if (cherryInstances.current[noteTab.key]) {
      try {
        cherryInstances.current[noteTab.key].destroy();
        delete cherryInstances.current[noteTab.key];
      } catch (error) {
        console.error('清理Cherry实例失败:', error);
      }
    }

    try {
      const cherry = new Cherry({
        id: containerId,
        value: noteTab.content || '# 开始编写你的笔记\n\n在这里输入笔记内容...\n\n支持 **Markdown** 语法！',
        callback: {
          afterChange: (text: string) => {
            // 更新本地状态
            setOpenTabs(prev => prev.map(t => 
              t.key === noteTab.key 
                ? { ...t, content: text }
                : t
            ));
            
            setHasUnsavedChanges(true);
            
            // 自动保存 - 3秒后保存
            if (saveTimeoutRef.current) {
              clearTimeout(saveTimeoutRef.current);
            }
            saveTimeoutRef.current = setTimeout(() => {
              handleSave(text);
            }, 3000);
          }
        }
      });

      cherryInstances.current[noteTab.key] = cherry;
    } catch (error) {
      console.error('Cherry编辑器初始化失败:', error);
      // 显示错误提示或降级方案
      if (editorRef.current) {
        editorRef.current.innerHTML = `
          <div style="padding: 16px; text-align: center; color: #999;">
            <p>编辑器加载失败</p>
            <p>请刷新页面重试</p>
          </div>
        `;
      }
    }
  }, [noteTab.key, noteTab.content, setOpenTabs, handleSave]);

  // 手动保存
  const handleManualSave = useCallback(() => {
    if (saveTimeoutRef.current) {
      clearTimeout(saveTimeoutRef.current);
    }
    handleSave();
  }, [handleSave]);

  // 保存标题
  const handleSaveTitle = useCallback(async () => {
    if (!noteTab.id || titleValue === noteTab.title) {
      setIsEditingTitle(false);
      return;
    }

    try {
      await updateNote(noteTab.id, { title: titleValue });
      
      // 更新本地状态
      setOpenTabs(prev => prev.map(t => 
        t.key === noteTab.key 
          ? { ...t, title: titleValue }
          : t
      ));
      
      setIsEditingTitle(false);
      message.success('标题已保存');
    } catch (error) {
      console.error('保存标题失败:', error);
      message.error('保存标题失败');
    }
  }, [noteTab.id, noteTab.title, titleValue, updateNote, setOpenTabs, noteTab.key]);

  // 更新表情符号
  const handleEmojiChange = useCallback(async (emoji: string) => {
    if (!noteTab.id) return;

    try {
      await updateNote(noteTab.id, { titleEmoji: emoji });
      
      // 更新本地状态
      setOpenTabs(prev => prev.map(t => 
        t.key === noteTab.key 
          ? { ...t, titleEmoji: emoji }
          : t
      ));
      
      message.success('图标已更新');
    } catch (error) {
      console.error('更新图标失败:', error);
      message.error('更新图标失败');
    }
  }, [noteTab.id, noteTab.key, updateNote, setOpenTabs]);

  // 键盘快捷键
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.ctrlKey && e.key === 's') {
        e.preventDefault();
        handleManualSave();
      }
    };

    document.addEventListener('keydown', handleKeyDown);
    return () => document.removeEventListener('keydown', handleKeyDown);
  }, [handleManualSave]);

  // 初始化编辑器
  useEffect(() => {
    const timer = setTimeout(() => {
      initCherry();
    }, 100);

    return () => {
      clearTimeout(timer);
      if (saveTimeoutRef.current) {
        clearTimeout(saveTimeoutRef.current);
      }
    };
  }, [initCherry]);

  // 同步标题值
  useEffect(() => {
    setTitleValue(noteTab.title);
  }, [noteTab.title]);

  return (
    <div style={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      {/* 标题栏 */}
      <Card 
        size="small"
        style={{ 
          borderRadius: 0,
          borderBottom: '1px solid #f0f0f0',
          borderLeft: 'none',
          borderRight: 'none',
          borderTop: 'none'
        }}
        bodyStyle={{ padding: '12px 16px' }}
      >
        <Space align="center" style={{ width: '100%' }}>
          {/* 表情符号选择器 */}
          <EmojiPicker
            defaultAvatar={noteTab.titleEmoji || '📝'}
            onChange={handleEmojiChange}
          />

          {/* 标题编辑 */}
          <div style={{ flex: 1 }}>
            {isEditingTitle ? (
              <Space.Compact style={{ width: '100%' }}>
                <Input
                  value={titleValue}
                  onChange={(e) => setTitleValue(e.target.value)}
                  onPressEnter={handleSaveTitle}
                  onBlur={handleSaveTitle}
                  autoFocus
                  placeholder="请输入标题"
                />
              </Space.Compact>
            ) : (
              <Space align="center">
                <Title 
                  level={4} 
                  style={{ 
                    margin: 0, 
                    cursor: 'pointer',
                    color: '#262626'
                  }}
                  onClick={() => setIsEditingTitle(true)}
                >
                  {noteTab.title || '未命名笔记'}
                </Title>
                <Button
                  type="text"
                  size="small"
                  icon={<EditOutlined />}
                  onClick={() => setIsEditingTitle(true)}
                  style={{ opacity: 0.6 }}
                />
              </Space>
            )}
          </div>

          {/* 保存按钮 */}
          <Button
            type={hasUnsavedChanges ? 'primary' : 'default'}
            size="small"
            icon={<SaveOutlined />}
            onClick={handleManualSave}
            disabled={!hasUnsavedChanges}
          >
            {hasUnsavedChanges ? '保存' : '已保存'}
          </Button>
        </Space>
      </Card>

      {/* 编辑器区域 */}
      <div style={{ 
        flex: 1,
        padding: '16px',
        overflow: 'hidden'
      }}>
        <div 
          ref={editorRef}
          id={`cherry-${noteTab.key}`}
          style={{ 
            height: '100%',
            width: '100%',
            border: '1px solid #d9d9d9',
            borderRadius: '6px',
            overflow: 'hidden'
          }}
        />
      </div>
    </div>
  );
} 