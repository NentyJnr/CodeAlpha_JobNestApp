using System.Net;

namespace JobNest.Commons.Responses
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse<T> SetError(ApiResponse<T> response, string message = null, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            response.Status = false;
            response.Message = message ?? "An error occurred.";
            response.StatusCode = (int)httpStatusCode;
            response.Errors = new List<string>();
            return response;
        }

        public static ApiResponse<T> SetSuccess(ApiResponse<T> response, T data, string message = null)
        {
            response.Status = true;
            response.Message = message ?? "Request successful.";
            response.Data = data;
            response.Errors = new List<string>();
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }
    }
}
