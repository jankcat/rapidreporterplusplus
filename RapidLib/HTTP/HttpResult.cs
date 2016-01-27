namespace RapidLib.HTTP
{
    public class HttpResult
    {
        // Response
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }

        public HttpResult()
        {
            Status = "";
            StatusCode = 0;
            Message = "";
        }
    }
}
