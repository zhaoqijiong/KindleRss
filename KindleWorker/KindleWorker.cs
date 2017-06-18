using System;
using KindleWorker.Models;


namespace KindleWorker {
    public class KindleWorker {

        private InterfaceKindleDb db;

        public KindleWorker() {
            var logConfigPath = new System.IO.FileInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/log4net.xml"));
            log4net.Config.XmlConfigurator.Configure(logConfigPath);
        }

        public void Init() {
            var dbtype = System.Configuration.ConfigurationManager.AppSettings["dbtype"];

            switch (dbtype) {
                case "xml":
                    db = new XmlDb();

                    break;
                default:
                    break;
            }

            db.Init();

        }
    }
}
