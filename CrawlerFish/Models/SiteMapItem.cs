using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerFish.Models {
	public class SiteMapItem {
		public string Url { get; set; }
		public List<string> Links { get; set; }
		public List<string> Assets { get; set; }
	}
}