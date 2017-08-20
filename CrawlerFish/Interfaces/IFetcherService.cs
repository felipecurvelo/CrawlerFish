using System.Collections.Generic;

namespace CrawlerFish.Interfaces {
	public interface IFetcherService {
		string RetrieveUrlAsPlainText(string url);

		List<string> ExtractLinks(string pageText);
		List<string> ExtractAssets(string pageText);
	}
}