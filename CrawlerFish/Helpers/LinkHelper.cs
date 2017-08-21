using System;

namespace CrawlerFish.Helpers {
	public class LinkHelper {

		/// <summary>
		/// Normalize, parse url and get its host
		/// </summary>
		public static string GetUrlHost(string url) {
			var normalizedUrl = NormalizeUrl(url);
			var uri = new UriBuilder(normalizedUrl);
			return uri != null ? uri.Host : string.Empty;
		}

		/// <summary>
		/// Check if url has one of the invalid extensions from config file
		/// </summary>
		public static bool UrlHasInvalidExtension(string url) {
			foreach (string notValidExtension in ConfigurationHelper.NotValidExtensionsToNavigate) {
				if (url.Contains(notValidExtension)) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Remove invalid url start chars
		/// </summary>
		private static string removeInvalidStartChars(string url) {
			var notValidStart = new string[] { "//", "/", "://", "#" };
			foreach (var s in notValidStart) {
				if (url.StartsWith(s)) {
					return url.Substring(s.Length, url.Length - s.Length);
				}
			}
			return url;
		}

		/// <summary>
		/// Normalize url to prevent errors and wrong comparison
		/// </summary>
		public static string NormalizeUrl(string url) {
			if (!UrlHasInvalidExtension(url)) {
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