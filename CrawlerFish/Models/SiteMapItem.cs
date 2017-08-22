using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerFish.Models {
	public class SiteMapItem {
		public string ParentUrl { get; set; }
		public string Url { get; set; }
		public List<string> Links { get; set; }
		public List<string> Assets { get; set; }
		public ApiError Error { get; set; }
	}
}