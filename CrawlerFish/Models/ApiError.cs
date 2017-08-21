using CrawlerFish.Helpers;
using System.Net;

namespace CrawlerFish.Models {
	public class ApiError {
		public HttpStatusCode Status { get; set; }
		public string Message { get; set; }

		public ApiError(ErrorCode errorCode) {
			Status = ErrorHelper.GetStatusCode(errorCode);
			Message = ErrorHelper.GetMessage(errorCode);
		}
	}
}