using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using UnityEngine;

namespace MonitorTool.src.core
{
    class XmlUtils
    {

        ///<summary>
        /// 获的路径下的所有.xml文件
        ///</summary>
        ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        ///<returns>返回Dictionary</returns>
        public static string[] getXmlFileList(string path)
        {
            return Directory.GetFiles(path, "*.xml");
        }

        ///<summary>
        /// 获得解析好的XML
        ///</summary>
        ///<param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        ///<returns>返回Dictionary</returns>
        public static Dictionary<String, Dictionary<String, String>> getXmlList(string xmlFileName)
        {
            Dictionary<String, Dictionary<String, String>> xmlList = new Dictionary<String, Dictionary<String, String>>();
         Dictionary<string,string> dic=new Dictionary<string, string>();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //加载XML文档
                //取到所有的xml结点
                XmlNodeList nodelist = xmlDoc.GetElementsByTagName("*");
                for (int i = 1; i < nodelist.Count; ++i)
                {
                    XmlNode xmlNode = nodelist.Item(i);
                    string a = xmlNode.OuterXml;
                    XmlAttributeCollection xmlListstr = xmlNode.Attributes;
                    Dictionary<String, String> xmlListline = new Dictionary<String, String>();
                    foreach (XmlAttribute item in xmlListstr)
                    {
                        string Name = item.Name;
                        string InnerText = item.InnerText;
                        xmlListline.Add(Name, InnerText);
                    }
                    //当前Map存在数据
                    if (xmlListline.Count > 0)
                    {
                        String value = "";
                        xmlListline.TryGetValue(xmlListline.Keys.First(), out value);
                        if (!"".Equals(value))
                        {
                            xmlList.Add(value, xmlListline);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Debug.Log(xmlFileName);
                return null;
                //throw ex; //这里可以定义你自己的异常处理
            }
            return xmlList;
        }

        /// <summary>
        /// 修改单行数据
        /// </summary>
        /// <param name="xmlFileName">XML地址</param>
        /// <param name="MapTable">单行数据。第一项必须是唯一键值对</param>
        public static void Modify(string xmlFileName, Dictionary<string, string> MapTable)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //加载XML文档
                XmlElement root = xmlDoc.DocumentElement;   //获取根节点 
                //取到所有的xml结点
                XmlNodeList nodelist = xmlDoc.GetElementsByTagName("*");
                foreach (XmlNode node in nodelist)
                {
                    XmlNode xmlNode = node;
                    XmlElement ele = (XmlElement)node;
                    if (ele.GetAttribute(MapTable.Keys.First()) == MapTable[MapTable.Keys.First()])
                    {
                        foreach (string item in MapTable.Keys)
                        {
                            if (ele.GetAttribute(item) != MapTable[item])
                            {
                                ele.SetAttribute(item, MapTable[item]);
                            }
                        }
                    }
                }
               Debug.Log("节点修改成功");
               xmlDoc.Save(xmlFileName);
            }
            catch (System.Exception ex)
            {
                Debug.Log(xmlFileName + ex);
                return;
            }
        }
        /// <summary>
        /// 修改单行单列上的一个数据
        /// </summary>
        /// <param name="xmlFileName">要修改xml地址</param>
        /// <param name="CheckKey">唯一key</param>
        /// <param name="CheckValue">唯一值</param>
        /// <param name="key">要修改的key</param>
        /// <param name="value">要修改的值</param>
        public static void Modify(string xmlFileName, string CheckKey, string CheckValue, string key, string value)
        {
             XmlDocument xmlDoc = new XmlDocument();
             try
             {
                 xmlDoc.Load(xmlFileName); //加载XML文档
                 XmlElement root = xmlDoc.DocumentElement;   //获取根节点 
                 //取到所有的xml结点
                 XmlNodeList nodelist = xmlDoc.GetElementsByTagName("*");
                 foreach (XmlNode node in nodelist)
                 {
                    XmlElement ele = (XmlElement)node;
                    if (ele.GetAttribute(CheckKey) == CheckValue)
                    {
                        if (ele.GetAttribute(key) != value)
                        {
                            ele.SetAttribute(key, value);
                        }
                    }
                 }
             }
             catch (System.Exception ex)
             {
                 Debug.Log(xmlFileName);
                 return;
             }
        }


    }
}
