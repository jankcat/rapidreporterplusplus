namespace Rapid_Reporter.HTML
{
    class HttpResult
    {
        // Response
        public int StatusCode { get; internal set; }
        public string Status { get; internal set; }
        public string Message { get; internal set; }

        public HttpResult()
        {
            Status = "";
            StatusCode = 0;
            Message = "";
        }
    }
}
