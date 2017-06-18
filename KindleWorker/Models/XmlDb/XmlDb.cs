﻿using System;
using log4net;


namespace KindleWorker.Models
{
    public class XmlDatabase : InterfaceKindleDb
    {
        private string _rootPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xmldb");
        private static ILog Logger = log4net.LogManager.GetLogger(typeof(XmlDatabase));

        public XmlDatabase()
        {
        }


        public void Init(){
            Logger.DebugFormat("xmldb init");


            if (!System.IO.Directory.Exists(_rootPath)) {
                Logger.Debug("xml 文件夹不存在, 创建一个文件夹");

                System.IO.Directory.CreateDirectory(_rootPath);
            }




        }
    }
}
