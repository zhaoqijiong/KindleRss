using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace KindleWorker.Models.XmlDb {
    public class XmlTableRss : InterfaceXmlTable{
        private string _rootPath;
        private string _xmlFileName;
        private string _TableName = "Rss.xml";
        private XDocument _doc;

        //TODO: 自增长id
        public XmlTableRss() {
        }

        public void CheckAndInit(string rootPath) {
            _rootPath = rootPath;
            _xmlFileName = System.IO.Path.Combine(rootPath, _TableName);

            if (System.IO.File.Exists(_xmlFileName)) {
                _doc = XDocument.Load(_xmlFileName);

            }else {
                _doc = new XDocument();
                _doc.Add(new XElement("root"));
                _doc.Save(_xmlFileName);
            }

        }

        public List<Rss> GetAll(){
            var items = _doc.DescendantNodes().OfType<XElement>().Where(n=>n.Name == "rss").ToArray();

            var list = new List<Rss>();

            foreach (var i in items) {
                list.Add(new Rss() {
                    Id = int.Parse( i.Attribute("id").Value ),
                    Name = i.Attribute("name").Value,
                    CreateTime = DateTime.Parse(i.Attribute("createtime").Value),
                    Url = i.Attribute("url").Value
                });
            }

            return list;
        }

        public void Add(Rss item){
            _doc.Root.Add(new XElement("rss",
                                       new XAttribute("id",item.Id),
                                       new XAttribute("name",item.Name),
                                       new XAttribute("url",item.Url),
                                       new XAttribute("createtime",item.CreateTime)
                                      ));

            _doc.Save(_xmlFileName);
        }
    }
}
