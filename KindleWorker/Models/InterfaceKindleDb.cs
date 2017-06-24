using System;
using System.Collections.Generic;



namespace KindleWorker.Models {
    public interface InterfaceKindleDb {
        void Init();
        List<Rss> GetRssList();
        void AddRss(Rss rss);
        void AddRssItems(int rssId,List<RssItem> items);
        /// <summary>
        /// 获取指定日期后面的数据
        /// </summary>
        /// <param name="rssId"></param>
        /// <param name="afterTime"></param>
        /// <returns></returns>
        List<RssItem> GetItems(int rssId, DateTime afterTime);
    }
}
