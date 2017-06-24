using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;


namespace KindleWorker.Models.XmlDb {
    public class XmlTableRssItem :InterfaceXmlTable{
		private string _rootPath;
		private string _xmlFileName;
        private string _TableName = "RssItem_";
		private XDocument _doc;

        private int _maxId = 0;

		public XmlTableRssItem() {
		}

		public void CheckAndInit(string rootPath,int rssId) {
			_rootPath = rootPath;
            _xmlFileName = System.IO.Path.Combine(rootPath, $"{_TableName}{rssId}.xml");

			if (System.IO.File.Exists(_xmlFileName)) {
				_doc = XDocument.Load(_xmlFileName);

                _maxId = _doc.Descendants().OfType<XElement>()
                                .Where(n => n.Name == "rssitem" && n.Attribute("id") != null)
                                .Select(n => n.Attribute("id").Value).ToList()
                                .ConvertAll(n => int.Parse(n)).Max();
			} else {
				_doc = new XDocument();
                _doc.Add(new XElement("root"));
				_doc.Save(_xmlFileName);
			}

		}

		public List<RssItem> GetAll() {
			var items = _doc.DescendantNodes().OfType<XElement>().Where(n => n.Name == "rssitem").ToArray();

            var list = new List<RssItem>();

			foreach (var i in items) {
                list.Add(new RssItem() {
                    Id = int.Parse(i.Attribute("id").Value),
                    RssId = int.Parse(i.Attribute("rssid").Value),
                    PubTime = i.Attribute("pubtime").Value,
                    Url = i.Attribute("url").Value,
                    Title = i.Attribute("title").Value,
                    Guid = i.Attribute("guid").Value
				});
			}

			return list;
		}

		public void Add(RssItem item) {

            var guids = _doc.Descendants().OfType<XElement>()
                            .Where(n => n.Name.Equals("rssitem") && n.Attribute("guid") != null)
                            .Select(n => n.Attribute("guid").Value).ToList();

            if (guids.Contains(item.Guid)) {
                return;
            }

            if (item.Id == 0 || item.Id < _maxId) {
                item.Id = ++_maxId;
            }

            _doc.Root.Add(new XElement("rssitem",
						   new XAttribute("id", item.Id),
						   new XAttribute("rssid", item.RssId),
						   new XAttribute("url", item.Url),
						   new XAttribute("pubtime", item.PubTime),
                           new XAttribute("title",item.Title),
                           new XAttribute("guid",item.Guid)
						  ));

			_doc.Save(_xmlFileName);
		}

		public void Add(List<RssItem> items) {

            var guids = _doc.Descendants().OfType<XElement>()
                            .Where(n => n.Name.Equals("rssitem") && n.Attribute("guid") != null)
                            .Select(n => n.Attribute("guid").Value).ToList();

            foreach (var item in items) {
                if (item.Id == 0 || item.Id < _maxId) {
                    item.Id = ++_maxId;
                }

                if (guids.Contains(item.Guid)) {
                    continue;
                }

                _doc.Root.Add(new XElement("rssitem",
							   new XAttribute("id", item.Id),
							   new XAttribute("rssid", item.RssId),
							   new XAttribute("url", item.Url),
							   new XAttribute("pubtime", item.PubTime),
							   new XAttribute("title", item.Title),
							   new XAttribute("guid", item.Guid)
							  ));
            }

			_doc.Save(_xmlFileName);
		}

	}
}
