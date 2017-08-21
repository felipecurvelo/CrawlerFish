using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace CrawlerFish {
	public static class HtmlAttributeCollectionExtensions {

		/// <summary>
		/// Extends HtmlAttributeCollection to transform it in a list of HtmlAttribute
		/// </summary>
		public static List<HtmlAttribute> GetAttributesDictionary(this HtmlAttributeCollection attributeCollection) {
			var attributeList = new List<HtmlAttribute>();
			foreach (HtmlAttribute a in attributeCollection) {
				attributeList.Add(a);
			}
			return attributeList;
		}
	}
}