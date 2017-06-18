using System;
using System.Collections.Generic;



namespace KindleWorker.Models {
    public interface InterfaceKindleDb {
        void Init();
        List<Rss> GetRssList();
        void AddRss(Rss rss);
        void AddRssItems(int rssId,List<RssItem> items);
    }
}
