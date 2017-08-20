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
	}
}
