using System;
namespace KindleWorker.Models {
    public class Rss{
        public int Id { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

    }

    public class RssItem {

        public int Id { get; set; }

        public int RssId { get; set; }

        public string Title { get; set; }

        public string PubTime { get; set; }

        public string Guid { get; set; }

        public string Url { get; set; }
    }
}
