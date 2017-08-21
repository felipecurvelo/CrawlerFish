using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CrawlerFish.Helpers {
	public class ConfigurationHelper {
		public static List<string> NotValidExtensionsToNavigate {
			get {
				return ConfigurationManager.AppSettings["NotValidExtensionsToNavigate"].Split('|').ToList();
			}
		}
	}
}