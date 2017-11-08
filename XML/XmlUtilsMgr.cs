using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//xml管理类
namespace MonitorTool.src.core
{
    using UnityEngine; //所有表的缓存
    using DicMapTable = Dictionary<String, Dictionary<String, String>>;  //一级枚举,一张表的数据
    using DicMapTableAll = Dictionary<String, Dictionary<String, Dictionary<String, String>>>;

    class XmlUtilsMgr
    {
        private static XmlUtilsMgr xmlUtilsMgr;
        private static bool m_init = false;
        //获得管理类
        public static XmlUtilsMgr newInstance(){
            if (xmlUtilsMgr == null)
            {
                xmlUtilsMgr = new XmlUtilsMgr();
                xmlUtilsMgr.Init();
            }
            return xmlUtilsMgr;
        }

        private DicMapTableAll configCache;
        private string defPath = null;
        //缓存所有表数据
        public void Init()
        {
            if (m_init)
                return;
            // 初始化表格
            configCache = new DicMapTableAll();

            defPath = GlobalConfig.Instance.ConfigPath.Replace("\\", "/");
            String[] fileList = XmlUtils.getXmlFileList(defPath);
            foreach (String fileName in fileList)
            {
               string fname = fileName.Replace("\\", "/");
               DicMapTable map = XmlUtils.getXmlList(fname);
               configCache.Add(fname, map);
            }
            m_init = true;
        }

        //直接获取单行数据某个key的值
        public String getItemKey(String fileName, String key, String keys)
        {
            if (configCache == null)
            {
                return null;
            }
            Dictionary<String, String> map = getItem(fileName, key);
            String value = null;
            Boolean isRead = map.TryGetValue(keys, out value);
            return value;
        }

        //获得单行数据
        public Dictionary<String, String> getItem(String fileName, String key)
        {
            if (fileName.Length < defPath.Length)
                fileName = string.Format("{0}{1}", defPath, fileName + ".xml");
            Dictionary<String, String> map = new Dictionary<String, String>();
            if (configCache == null)
            {
                return null;
            }
            DicMapTable dicMap;
            Boolean isRead = configCache.TryGetValue(fileName, out dicMap);
            if (isRead)
            {
                dicMap.TryGetValue(key, out map);
            }
            else
            {
                return null;
            }

            return map;
        }

        //获得整个表的数据
        public DicMapTable getAll(String fileName)
        {
            if (fileName.Length < defPath.Length)
                fileName = string.Format("{0}{1}", defPath, fileName + ".xml");
            if (configCache == null)
            {
                return null;
            }
            DicMapTable dicMap;
            Boolean isRead = configCache.TryGetValue(fileName, out dicMap);
            if (isRead)
            {
                return dicMap;
            }
            else
            {
                return null;
            }
        }
        
    }
}
