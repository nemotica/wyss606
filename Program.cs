using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.IO;
using System.Xml;

namespace ConsoleApplication1
{
    class Program
    {
        static string xmlfilename = "catelog.xml";         //define of filename
        static bool xmlfileexist = false;

        static void Main(string[] args)
        {
            Program p = new Program();

            //string path = Console.ReadLine();
            string path = "D:\\wy\\video_clip";
            p.createXMLFile(path);

            Console.Read();
        }

        public XmlDocument xmldoc;
        public XmlNode xmlnode;
        public XmlElement xmlelem;
        public XmlDeclaration xmldecl;

        private void createXMLFile(string path)
        {
            string[] aformatList = {"avi", "mp4", "mpeg", "mpg", "divx", "mov", "wmv"};
            try
            {
                //////////////判断xml文件是否存在，如果不存在则创建新的xml文件
                if (System.IO.File.Exists(xmlfilename)) {                               //如果a.xml已经存在，则载入
                    xmldoc.Load(xmlfilename);
                    xmlfileexist = true;
                }
                else 
                {                                                                       //如果a.xml不存在，则创建新的xml文件。
                    xmldoc = new XmlDocument();
                    xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
                    xmldoc.AppendChild(xmldecl);                                        //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>

                    xmlelem = xmldoc.CreateElement("", "catalog", "");                  //加入一个根元素
                    xmldoc.AppendChild(xmlelem);
                }
                ///////////////

                XmlNode root = xmldoc.SelectSingleNode("catalog");                    //查找<Catelog> 


                ///////////////遍历文件夹和文件，对应每个视频文件建立相应的vnode结点
                string[] dir = Directory.GetDirectories(path);                          //当前文件夹列表   
                DirectoryInfo fdir = new DirectoryInfo(path);
                FileInfo[] file = fdir.GetFiles();
                //FileInfo[] file = Directory.GetFiles(path);                           //文件列表   

                XmlElement xeTemp, xeTempSub, xeTempSub1;

                if (file.Length != 0 || dir.Length != 0)                                //当前目录文件或文件夹不为空
                {
                    foreach (FileInfo f in file)                                        //显示当前目录所有文件，各种格式的都有，没有文件夹   
                    {
                        string sfullName = f.FullName.ToString();                       //取得文件的完整路径字符串

                        int iLastIndexOfSlash = sfullName.LastIndexOf('\\');
                        int iLastIndexOfDot = sfullName.LastIndexOf('.');

                        string sformat = sfullName.Substring(iLastIndexOfDot + 1);      //取出了文件类型
                        string sname = sfullName.Substring(iLastIndexOfSlash + 1, iLastIndexOfDot - iLastIndexOfSlash - 1);   //取出了文件名
                        string sformatLow = sformat.ToLower();
                        long sLength = f.Length;

                        foreach (string format in aformatList)                          //
                        {
                            if (sformatLow.CompareTo(format) == 0){                     //如果找到了视频格式文件，则进行结点的创建
                                
                                xeTemp = xmldoc.CreateElement("", "vnode", "");        //创建视频结点
                                xeTemp.SetAttribute("name", sname);                    
                                xeTemp.SetAttribute("format", sformatLow);

                                xeTempSub = xmldoc.CreateElement("", "vsize", "");
                                xeTempSub.InnerText = sLength.ToString();
                                xeTemp.AppendChild(xeTempSub);

                                xeTempSub = xmldoc.CreateElement("", "vdir", "");
                                xeTempSub.InnerText = sfullName;
                                xeTemp.AppendChild(xeTempSub);

                                xeTempSub = xmldoc.CreateElement("", "vdataset", "");
                                xeTempSub.InnerText = "vdataset";                       //TODO
                                xeTemp.AppendChild(xeTempSub);

                                xeTempSub = xmldoc.CreateElement("", "features", "");
                                xeTempSub.SetAttribute("fNumber", "0");                    //default:0
                                xeTempSub1 = xmldoc.CreateElement("", "");


                                xeTemp.AppendChild(xeTempSub);

                                root.AppendChild(xeTemp);
                            }
                        }

                        Console.Write(sfullName + "\n sname: " + sname + "\n sformat: " + sformat); break;
                        
                        

                        //Console.Write("   -- " + filename + "\n");
                    }
                    foreach (string d in dir)
                    {
                        //Console.Write("d: " + d + "\n");
                        int startIndex = d.LastIndexOf("\\");
                        string fileName = d.Substring(startIndex + 1);      //文件夹名称
                        //Console.Write("" + fileName + "\n");

                        createXMLFile(d);  //递归  
                    }
                }
                else
                    return;

                xmldoc.Save("a.xml");                                   //保存a.xml文件

            }
            catch { };
        }

        
    }

}
