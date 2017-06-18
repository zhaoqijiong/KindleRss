using System;
using KindleWorker.Models;
using System.Collections.Generic;
using log4net;

namespace KindleWorker {
    public class KindleWorker {

        private InterfaceKindleDb db;
        private List<Rss> _rssList = new List<Rss>();
        private static ILog Logger = log4net.LogManager.GetLogger(typeof(KindleWorker));

        public KindleWorker() {
            var logConfigPath = new System.IO.FileInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/log4net.xml"));
            log4net.Config.XmlConfigurator.Configure(logConfigPath);
        }

        public void Init() {
            var dbtype = System.Configuration.ConfigurationManager.AppSettings["dbtype"];

            switch (dbtype) {
                case "xml":
                    db = new XmlDatabase();

                    break;
                default:
                    break;
            }

            db.Init();



        }

        /// <summary>
        /// 更新一次内容
        /// </summary>
        public void DoUpdate(){
			GetDbRssList();

			if (_rssList.Count > 0) {

                CheckRssUpdate();
			}
        }

        /// <summary>
        /// 获取Rss 列表
        /// </summary>
        public void CheckRssUpdate(){
            foreach (var r in _rssList) {

            }
        }

        /// <summary>
        /// 从数据库中获取要更新的RSS列表
        /// </summary>
        private void GetDbRssList(){
            _rssList = db.GetRssList();

            Logger.DebugFormat("获取 rss 列表, 获取到 {0}" , _rssList.Count);
        }
    }
}
