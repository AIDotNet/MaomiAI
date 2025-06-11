import React, { createContext, useContext, useState, useCallback, useRef } from 'react';
import { message } from 'antd';
import type { DataNode } from 'antd/es/tree';
import { GetApiClient } from '../../../ServiceClient';
import type { 
  NoteTreeItem, 
  QueryNoteTreeCommand, 
  CreateNoteCommand, 
  QueryNoteCommandResponse, 
  UpdateNoteCommand 
} from '../../../../apiClient/models';

// 扩展的树节点类型
export interface TreeData extends DataNode {
  id: string;
  isNote: boolean;
  content?: string;
  noteId?: string;
  parentNoteId?: string | null;
}

// 笔记标签页类型
export interface NoteTab {
  key: string;
  title: string;
  content: string;
  noteId: string;
  titleEmoji?: string;
  isNew?: boolean;
  id?: string;
}

// Context 类型定义
interface NoteContextType {
  // 树相关状态
  treeData: TreeData[];
  setTreeData: React.Dispatch<React.SetStateAction<TreeData[]>>;
  selectedKeys: string[];
  setSelectedKeys: React.Dispatch<React.SetStateAction<string[]>>;
  expandedKeys: string[];
  setExpandedKeys: React.Dispatch<React.SetStateAction<string[]>>;
  
  // 标签页相关状态
  openTabs: NoteTab[];
  setOpenTabs: React.Dispatch<React.SetStateAction<NoteTab[]>>;
  activeTabKey: string;
  setActiveTabKey: React.Dispatch<React.SetStateAction<string>>;
  
  // 加载和错误状态
  loading: boolean;
  setLoading: React.Dispatch<React.SetStateAction<boolean>>;
  error: string | null;
  setError: React.Dispatch<React.SetStateAction<string | null>>;
  
  // Cherry编辑器实例管理
  cherryInstances: React.MutableRefObject<{[key: string]: any}>;
  
  // API 方法
  fetchCatalog: () => Promise<void>;
  createNote: (parentNoteId: string | null) => Promise<void>;
  fetchNoteContent: (noteId: string) => Promise<QueryNoteCommandResponse | null>;
  updateNote: (noteId: string, updates: Partial<UpdateNoteCommand>) => Promise<void>;
  saveNoteContent: (tabKey: string, content: string) => Promise<void>;
  
  // 树操作方法
  findNodeById: (nodes: TreeData[], id: string) => TreeData | null;
  convertToTreeData: (items: NoteTreeItem[], parentId?: string | null) => TreeData[];
  
  // 标签页操作方法
  openNote: (noteId: string) => Promise<void>;
  closeTab: (tabKey: string) => void;
  switchTab: (tabKey: string) => void;
}

const NoteContext = createContext<NoteContextType | undefined>(undefined);

export const useNoteContext = () => {
  const context = useContext(NoteContext);
  if (!context) {
    throw new Error('useNoteContext must be used within a NoteProvider');
  }
  return context;
};

export const NoteProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  // 状态定义
  const [treeData, setTreeData] = useState<TreeData[]>([]);
  const [selectedKeys, setSelectedKeys] = useState<string[]>([]);
  const [expandedKeys, setExpandedKeys] = useState<string[]>(['root']);
  const [openTabs, setOpenTabs] = useState<NoteTab[]>([]);
  const [activeTabKey, setActiveTabKey] = useState<string>('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const cherryInstances = useRef<{[key: string]: any}>({});

  // API 客户端
  const getApiClient = () => GetApiClient();

  // 将 API 数据转换为树形数据
  const convertToTreeData = useCallback((items: NoteTreeItem[], parentId?: string | null): TreeData[] => {
    return items.map(item => ({
      key: item.noteId?.toString() || '',
      title: `${item.titleEmoji || '📝'} ${item.title || '未命名'}`,
      id: item.noteId?.toString() || '',
      isNote: true,
      content: item.summary || undefined,
      noteId: item.noteId?.toString(),
      parentNoteId: parentId,
      children: item.children ? convertToTreeData(item.children, item.noteId?.toString()) : undefined,
    }));
  }, []);

  // 递归查找树节点
  const findNodeById = useCallback((nodes: TreeData[], id: string): TreeData | null => {
    for (const node of nodes) {
      if (node.id === id) {
        return node;
      }
      if (node.children) {
        const found = findNodeById(node.children as TreeData[], id);
        if (found) return found;
      }
    }
    return null;
  }, []);

  // 获取目录树数据
  const fetchCatalog = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const apiClient = getApiClient();
      
      const queryCommand: QueryNoteTreeCommand = {
        includeSummary: true,
        parantId: undefined,
        search: undefined,
        searchContent: false
      };
      
      const response = await apiClient.api.note.catalog.post(queryCommand);
      
      const rootTreeData: TreeData[] = [{
        key: 'root',
        title: '🗂️ 我的笔记',
        id: 'root',
        isNote: false,
        parentNoteId: null,
        children: response?.notes ? convertToTreeData(response.notes, null) : []
      }];
      
      setTreeData(rootTreeData);
    } catch (err) {
      setError(err instanceof Error ? err.message : '获取目录失败');
    } finally {
      setLoading(false);
    }
  }, [convertToTreeData]);

  // 创建新笔记
  const createNote = useCallback(async (parentNoteId: string | null) => {
    try {
      const apiClient = getApiClient();
      
      const createCommand: CreateNoteCommand = {
        title: '新建笔记',
        content: '',
        parentNoteId: parentNoteId === 'root' ? null : (parentNoteId as any),
        titleEmoji: '📝'
      };
      
      const response = await apiClient.api.note.create.post(createCommand);
      
      if (response?.id) {
        // 创建新的tab
        const newTab: NoteTab = {
          key: response.id.toString(),
          title: '新建笔记',
          content: '',
          noteId: response.id.toString(),
          titleEmoji: '📝',
          isNew: true,
          id: response.id.toString()
        };
        
        setOpenTabs(prev => [...prev, newTab]);
        setActiveTabKey(response.id.toString());
        
        // 刷新目录树
        await fetchCatalog();
        
        message.success('笔记创建成功');
      }
    } catch (err) {
      console.error('创建笔记失败:', err);
      message.error('创建笔记失败');
    }
  }, [fetchCatalog]);

  // 获取笔记详细内容
  const fetchNoteContent = useCallback(async (noteId: string): Promise<QueryNoteCommandResponse | null> => {
    try {
      const apiClient = getApiClient();
      const response = await apiClient.api.note.byNoteId(noteId).get();
      return response || null;
    } catch (err) {
      console.error('获取笔记内容失败:', err);
      message.error('获取笔记内容失败');
      return null;
    }
  }, []);

  // 更新笔记
  const updateNote = useCallback(async (noteId: string, updates: Partial<UpdateNoteCommand>) => {
    try {
      const apiClient = getApiClient();
      const updateCommand: UpdateNoteCommand = {
        noteId: noteId as any,
        ...updates
      };
      
      await apiClient.api.note.update.post(updateCommand);
      
      // 刷新目录树以显示最新的标题变化
      await fetchCatalog();
    } catch (err) {
      console.error('更新笔记失败:', err);
      message.error('更新笔记失败');
    }
  }, [fetchCatalog]);

  // 保存笔记内容
  const saveNoteContent = useCallback(async (tabKey: string, content: string) => {
    const tab = openTabs.find(t => t.key === tabKey);
    if (!tab || !tab.id) {
      message.warning('找不到要保存的笔记');
      return;
    }

    // 检查内容是否有变化
    if (content === tab.content) {
      message.info('内容未发生变化');
      return;
    }

    try {
      await updateNote(tab.id, { content });
      
      // 更新本地状态
      setOpenTabs(prev => prev.map(t => 
        t.key === tabKey 
          ? { ...t, content }
          : t
      ));
      
      message.success(`${tab.title} 已保存`);
    } catch (err) {
      console.error('保存笔记内容失败:', err);
      message.error('保存失败，请重试');
    }
  }, [openTabs, updateNote]);

  // 打开笔记
  const openNote = useCallback(async (noteId: string) => {
    // 检查是否已经打开了这个tab
    const existingTab = openTabs.find(tab => tab.noteId === noteId);
    if (existingTab) {
      setActiveTabKey(existingTab.key);
      return;
    }
    
    // 获取详细内容
    const noteDetail = await fetchNoteContent(noteId);
    if (noteDetail) {
      const newTab: NoteTab = {
        key: noteDetail.id?.toString() || noteId,
        title: noteDetail.title || '未命名笔记',
        content: noteDetail.content || '',
        noteId: noteId,
        titleEmoji: noteDetail.titleEmoji || '📝',
        id: noteDetail.id?.toString()
      };
      
      setOpenTabs(prev => [...prev, newTab]);
      setActiveTabKey(newTab.key);
    }
  }, [openTabs, fetchNoteContent]);

  // 关闭标签页
  const closeTab = useCallback((tabKey: string) => {
    const newTabs = openTabs.filter(tab => tab.key !== tabKey);
    setOpenTabs(newTabs);
    
    // 清理Cherry编辑器实例
    if (cherryInstances.current[tabKey]) {
      try {
        cherryInstances.current[tabKey].destroy();
        delete cherryInstances.current[tabKey];
      } catch (error) {
        console.error('清理编辑器实例失败:', error);
      }
    }
    
    if (activeTabKey === tabKey) {
      const newActiveKey = newTabs.length > 0 ? newTabs[newTabs.length - 1].key : '';
      setActiveTabKey(newActiveKey);
    }
  }, [openTabs, activeTabKey]);

  // 切换标签页
  const switchTab = useCallback((tabKey: string) => {
    setActiveTabKey(tabKey);
  }, []);

  const contextValue: NoteContextType = {
    // 状态
    treeData,
    setTreeData,
    selectedKeys,
    setSelectedKeys,
    expandedKeys,
    setExpandedKeys,
    openTabs,
    setOpenTabs,
    activeTabKey,
    setActiveTabKey,
    loading,
    setLoading,
    error,
    setError,
    cherryInstances,
    
    // 方法
    fetchCatalog,
    createNote,
    fetchNoteContent,
    updateNote,
    saveNoteContent,
    findNodeById,
    convertToTreeData,
    openNote,
    closeTab,
    switchTab,
  };

  return (
    <NoteContext.Provider value={contextValue}>
      {children}
    </NoteContext.Provider>
  );
}; 