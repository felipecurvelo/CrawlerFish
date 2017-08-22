using System;

namespace CrawlerFish.Helpers {
	public class LinkHelper {

		/// <summary>
		/// Normalize, parse url and get its host
		/// </summary>
		public static string GetUrlHost(string url) {
			var uri = new UriBuilder(url);
			return uri != null ? uri.Host : string.Empty;
		}

		/// <summary>
		/// Check if url has one of the invalid extensions from config file
		/// </summary>
		public static bool UrlHasOneOfInvalidExtensions(string url) {
			foreach (string notValidExtension in ConfigurationHelper.NotValidExtensionsToNavigate) {
				if (url.Contains(notValidExtension)) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Normalize url to prevent errors and wrong comparison
		/// </summary>
		public static string NormalizeUrl(string url, string mainUrl = null) {
			if (UrlHasOneOfInvalidExtensions(url)) {
				return url;
			}

			var normalizedUrl = removeInvalidStartChars(url);

			while (normalizedUrl.EndsWith("/")) {
				normalizedUrl = normalizedUrl.Substring(0, normalizedUrl.Length - 1);
			}

			if (!String.IsNullOrEmpty(mainUrl) && !url.Contains(".")) {
				var relativeUrlPart = !String.IsNullOrWhiteSpace(normalizedUrl) ? "/" + normalizedUrl : string.Empty;
				normalizedUrl = mainUrl + relativeUrlPart;
			}
			
			if (!normalizedUrl.Contains("://")) {
				normalizedUrl = "http://" + normalizedUrl;
			}

			return normalizedUrl;
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
	}
}