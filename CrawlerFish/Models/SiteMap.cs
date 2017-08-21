using System.Collections.Generic;

namespace CrawlerFish.Models {
	public class SiteMap {
		public string MainUrl { get; set; }
		public List<SiteMapItem> Items { get; set; }

		public SiteMap() {
			Items = new List<SiteMapItem>();
		}
	}
}