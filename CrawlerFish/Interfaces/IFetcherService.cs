using CrawlerFish.Models;
using System.Collections.Generic;

namespace CrawlerFish.Interfaces {
	public interface IFetcherService {
		string RetrieveUrlAsPlainText(string url, out ApiError error);
		List<string> ExtractLinks(string pageText, string mainUrl);
		List<string> ExtractAssets(string pageText);
	}
}