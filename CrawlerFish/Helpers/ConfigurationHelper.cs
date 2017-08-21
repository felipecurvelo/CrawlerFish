using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CrawlerFish.Helpers {
	public class ConfigurationHelper {

		/// <summary>
		/// Get invalid extensions from config file
		/// </summary>
		public static List<string> NotValidExtensionsToNavigate {
			get {
				return ConfigurationManager.AppSettings["NotValidExtensionsToNavigate"].Split('|').ToList();
			}
		}
	}
}