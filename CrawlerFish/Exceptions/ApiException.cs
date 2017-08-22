using CrawlerFish.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace CrawlerFish.Exceptions {
	public class ApiException : Exception {
		public ApiException(ErrorCode errorCode) {
			ErrorCode = errorCode;
		}

		public ErrorCode ErrorCode { get; set; }
	}
}