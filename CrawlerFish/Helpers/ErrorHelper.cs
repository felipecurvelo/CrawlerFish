using CrawlerFish.Resources;
using System.Resources;
using System.Net;
using CrawlerFish.Models;

namespace CrawlerFish.Helpers {
	public class ErrorHelper {

		/// <summary>
		/// Get error message based on application errorCode 
		/// </summary>
		public static string GetMessage(ErrorCode errorCode) {
			var rm = new ResourceManager(typeof(ErrorMessage));
			return rm.GetString(errorCode.ToString());
		}

		/// <summary>
		/// Get HttpStatusCode based on application errorCode
		/// </summary>
		public static HttpStatusCode GetStatusCode(ErrorCode errorCode) {
			switch (errorCode) {
				case ErrorCode.UnhandledError:
					return HttpStatusCode.NotAcceptable;
				case ErrorCode.CannotReachWebSite:
				case ErrorCode.InvalidUrl:
					return HttpStatusCode.BadRequest;
				default:
					return HttpStatusCode.NotAcceptable;
			}
		}

		/// <summary>
		/// Create a new ApiException
		/// </summary>
		public static ApiError CreateError(ErrorCode errorCode) {
			return new ApiError(errorCode);
		}
	}
}