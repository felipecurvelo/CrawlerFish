using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerFish.Helpers {
	public class LinkHelper {
		public static string GetUrlHost(string url) {
			var normalizedUrl = NormalizeUrl(url);
			var uri = new UriBuilder(normalizedUrl);
			return uri != null ? uri.Host : string.Empty;
		}

		public static bool IsUrlValidToNavigate(string url) {
			foreach (string notValidExtension in ConfigurationHelper.NotValidExtensionsToNavigate) {
				if (url.Contains(notValidExtension)) {
					return false;
				}
			}
			return true;
		}

		private static string removeInvalidStartChars(string url) {
			var notValidStart = new string[] { "//", "/", "://", "#" };
			foreach (var s in notValidStart) {
				if (url.StartsWith(s)) {
					return url.Substring(s.Length, url.Length - s.Length);
				}
			}
			return url;
		}

		public static string NormalizeUrl(string url) {
			if (!IsUrlValidToNavigate(url)) {
				return url;
			}

			var normalizedUrl = removeInvalidStartChars(url);
			if (!normalizedUrl.Contains("://")) {
				normalizedUrl = "http://" + normalizedUrl;
			}

			var splitedUrl = normalizedUrl.Split(new string[] { "://" }, StringSplitOptions.None);
			if (!splitedUrl[1].StartsWith("www.")) {
				normalizedUrl = splitedUrl[0] + "://www." + splitedUrl[1];
			}

			while (normalizedUrl.EndsWith("/")) {
				normalizedUrl = normalizedUrl.Substring(0, normalizedUrl.Length - 1);
			}

			return normalizedUrl;
		}
	}
}