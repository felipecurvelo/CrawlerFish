using CrawlerFish.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerFish.Tests.UnitTest.Services {
	[TestClass]
	public class UrlFectherServiceTest {
		[TestMethod]
		public void TestBasicPageRetrieve_ReturnNonEmptyString() {
			var url = "http://www.google.com";
			var service = new UrlFetcherService();
			var actual = service.RetrieveUrlAsPlainText(url);
			Assert.IsTrue(!String.IsNullOrEmpty(actual));
		}

		[TestMethod]
		public void TestLinkExtract_ReturnOneLink() {
			string htmlSample = "<html><body><a href=\"OneLinkTest\" /></body></html>";
			var fetcherService = new UrlFetcherService();
			var actual = fetcherService.ExtractLinks(htmlSample).Count;
			Assert.AreEqual(1, actual);
		}

		[TestMethod]
		public void TestLinkExtract_ReturnTwoLinks() {
			string htmlSample = "<html><body><a href=\"TwoLinkTest\" /><a href=\"TwoLinkTest\" /></body></html>";
			var fetcherService = new UrlFetcherService();
			var actual = fetcherService.ExtractLinks(htmlSample).Count;
			Assert.AreEqual(2, actual);
		}

		[TestMethod]
		public void TestCssAssetExtract_ReturnOneAsset() {
			string htmlSample = "<html><body><link rel=\"stylesheet\" type=\"text / css\" href=\"mystyle.css\" /></body></html>";
			var fetcherService = new UrlFetcherService();
			var actual = fetcherService.ExtractAssets(htmlSample).Count;
			Assert.AreEqual(1, actual);
		}

		[TestMethod]
		public void TestCssAssetExtract_ReturnTwoAsset() {
			string htmlSample = "<html><body><link rel=\"stylesheet\" type=\"text / css\" href=\"mystyle.css\" /><link rel=\"stylesheet\" type=\"text / css\" href=\"mystyle2.css\" /></body></html>";
			var fetcherService = new UrlFetcherService();
			var actual = fetcherService.ExtractAssets(htmlSample).Count;
			Assert.AreEqual(2, actual);
		}
	}
}
