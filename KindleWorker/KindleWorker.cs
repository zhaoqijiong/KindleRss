using System;
using KindleWorker.Models;
using System.Collections.Generic;
using log4net;
using System.Linq;
using System.Threading.Tasks;
using KindleWorker.WebParser;
using TNX.RssReader;
using RssItem = KindleWorker.Models.RssItem;

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
        private void CheckRssUpdate(){
            foreach (var r in _rssList) {
                RssFeed feed = null;
                try {
                    feed = TNX.RssReader.RssHelper.ReadFeed(r.Url);
                } catch (Exception e) {
                    Logger.Error(e.Message);
                    continue;
                }
                
                var items = feed.Items.ToArray();

                Logger.DebugFormat("rss update url : {0} count :{1}", r.Url, items.Length);

                var list = new List<RssItem>();

                foreach (var item in items) {
                    list.Add(new RssItem(){
                        Id = 0,
                        Url = item.Link,
                        Guid = item.UniqueLinkOrName,
                        PubTime = item.PublicationUtcTime.ToLocalTime().ToString(),
                        RssId = r.Id,
                        Title = item.Title,
                    });
                }


                db.AddRssItems(r.Id,list);
            }
        }

        private async Task DownloadHtmlContent() {
            foreach (var rss in _rssList) {
                var items = db.GetItems(rss.Id, DateTime.Today.AddDays(-1));

                foreach (var rssItem in items) {
                    var http = new HttpDownloadHelper(new HttpWorkDefine() {
                        RssId = rss.Id.ToString(),
                        RssItemId = rssItem.Id.ToString(),
                        Url = rssItem.Url
                    });

                    if (http.CheckFileDownloaded()) {
                        continue;
                    }

                    await http.Do();
                }
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
