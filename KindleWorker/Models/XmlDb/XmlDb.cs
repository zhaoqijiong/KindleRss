﻿using System;
using log4net;


namespace KindleWorker.Models
{
    public class XmlDb : InterfaceKindleDb
    {
        private string _rootPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xmldb");
        private static ILog Logger = log4net.LogManager.GetLogger(typeof(XmlDb));

        public XmlDb()
        {
        }


        public void Init(){
            Logger.DebugFormat("xmldb init");
        }
    }
}
