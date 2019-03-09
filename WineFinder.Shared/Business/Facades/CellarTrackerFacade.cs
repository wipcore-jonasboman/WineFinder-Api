using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
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
                throw new SecurityException("incorrect_credentials");
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
            try
            {
                XmlDocument urlData = new XmlDocument();
                HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(url);

                rq.Timeout = 60000;

                HttpWebResponse response = rq.GetResponse() as HttpWebResponse;

                // New check added to dash's answer.
                if (response.ContentType.Contains("text/xml"))
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        XmlTextReader reader = new XmlTextReader(responseStream);
                        urlData.Load(reader);
                    }
                }

                return urlData;
            }
            catch (Exception)
            {
                throw new Exception("service_down");
            }

        }
    }
}
