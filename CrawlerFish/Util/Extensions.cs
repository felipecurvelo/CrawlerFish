using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CrawlerFish {
	public static class XmlAttributeCollectionExtensions {
		public static List<XmlAttribute> GetAttributesDictionary(this XmlAttributeCollection attributeCollection) {
			var attributeList = new List<XmlAttribute>();
			foreach (XmlAttribute a in attributeCollection) {
				attributeList.Add(a);
			}
			return attributeList;
		}
	}
}