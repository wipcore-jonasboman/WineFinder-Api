using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using WineFinder.Shared.Extensions;
using WineFinder.Shared.Models;

namespace WineFinder.Shared.Business.Facades
{
    public class CellarTrackerFacade : IWineFacade
    {
        private string _handle;
        private string _key;

        public CellarTrackerFacade(string handle, string key)
        {
            _handle = handle;
            _key = key;
        }

        public List<WineItem> LoadWines(bool includeLocation)
        {
            var fileFormat = "xml";
            var table = "List";
            
            var url = $"https://www.cellartracker.com/xlquery.asp?User={_handle}&Password={_key}&Format={fileFormat}&Table={table}";
            if (includeLocation) url += "&Location=1";

            var xml = GetXmlDataFromUrl(url);
            if (xml.DoesNotValidate())
            {
                throw new UnauthorizedAccessException("Incorrect credentials");
            }

            return MapToWineListItems(xml);
        }

        private List<WineItem> MapToWineListItems(XmlDocument xmlDocument)
        {
            var wineListItems = new List<WineItem>();

            XmlElement root = xmlDocument.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("/cellartracker/list/row");

            foreach (XmlNode node in nodes)
            {
                if (node.Validates())
                {
                    var item = new WineItem(node);
                    if (wineListItems.FirstOrDefault(i => i.Id == item.Id) == null)
                    {
                        wineListItems.Add(item);
                    }
                }
            }
            return wineListItems;
        }

        /// <summary>
        ///  Get Data in xml format by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private XmlDocument GetXmlDataFromUrl(string url)
        {
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(url);
            return doc1;
        }
    }
}
