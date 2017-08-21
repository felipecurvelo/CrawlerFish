using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerFish.Models {
	public class SiteMap {
		public string MainUrl { get; set; }
		public List<SiteMapItem> Items { get; set; }

		public SiteMap() {
			Items = new List<SiteMapItem>();
		}

		/// <summary>
		/// Add items from another siteMap
		/// </summary>
		public void Join(SiteMap siteMap) {
			Items.AddRange(siteMap.Items);
		}
	}
}