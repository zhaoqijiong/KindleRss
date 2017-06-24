using System;
using System.Collections.Generic;
using log4net;
using KindleWorker.Models.XmlDb;

namespace KindleWorker.Models
{
    public class XmlDatabase : InterfaceKindleDb {
        private string _rootPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xmldb");
        private static ILog Logger = log4net.LogManager.GetLogger(typeof(XmlDatabase));

        private Dictionary<string, InterfaceXmlTable> _tables = new Dictionary<string, InterfaceXmlTable>();

        public XmlDatabase() {
        }


        public void Init() {
            Logger.DebugFormat("xmldb init");


            if (!System.IO.Directory.Exists(_rootPath)) {
                Logger.Debug("xml 文件夹不存在, 创建一个文件夹");

                System.IO.Directory.CreateDirectory(_rootPath);
            }

            var tRss = new XmlTableRss();
            _tables.Add("Rss", tRss);

            tRss.CheckAndInit(_rootPath);

            foreach (var i in tRss.GetAll()) {
                var name = $"RssItem_{i.Id}";
                var t = new XmlTableRssItem();
                t.CheckAndInit(_rootPath, i.Id);
                _tables.Add(name,t);
            }


        }

        #region rss
        public List<Rss> GetRssList() {
            var t = _tables["Rss"];

            return (t as XmlTableRss).GetAll();
        }

        public void AddRss(Rss rss) {
            var t = _tables["Rss"];

            (t as XmlTableRss).Add(rss);
        }



		#endregion

		#region rssitem

		public void AddRssItems(int rssId, List<RssItem> items) {
            var tname = $"RssItem_{rssId}";
            if (!_tables.ContainsKey(tname)) {
                return;
            }

            var t = _tables[tname] as XmlTableRssItem;
            t.Add(items);
		}

        public List<RssItem> GetItems(int rssId, DateTime afterTime) {
            var tname = $"RssItem_{rssId}";
            if (!_tables.ContainsKey(tname)) {
                return new List<RssItem>();
            }

            var t = _tables[tname] as XmlTableRssItem;

            return t.GetItem(afterTime);
        }

        #endregion
    }
}
