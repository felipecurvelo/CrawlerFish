using System;

namespace CrawlerFish.Exceptions {
	public class ApiException : Exception {
		public ApiException(ErrorCode errorCode) {
			ErrorCode = errorCode;
		}

		public ErrorCode ErrorCode { get; set; }
	}
}