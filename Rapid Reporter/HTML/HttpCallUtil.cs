using System;
using System.IO;
using System.Net;
using System.Text;

namespace Rapid_Reporter.HTML
{
    internal static class HttpCallUtil
    {
        internal const int DefaultTimeout = 300000;
        internal const string DefaultAcceptType = "text/plain";

        internal static HttpResult HttpGetCall(string url)
        {
            var result = new HttpResult();

            try
            {
                var httpReq = (HttpWebRequest)WebRequest.Create(url);
                httpReq.Accept = DefaultAcceptType;
                httpReq.Method = @"GET";
                httpReq.Timeout = DefaultTimeout;
                httpReq.AllowAutoRedirect = false;
                httpReq.ContentLength = 0;

                var httpResp = (HttpWebResponse)httpReq.GetResponse();
                result.Status = httpResp.StatusDescription;
                result.StatusCode = (int)httpResp.StatusCode;

                var responseEncoding = (string.IsNullOrWhiteSpace(httpResp.CharacterSet))
                                           ? Encoding.Default
                                           : Encoding.GetEncoding(httpResp.CharacterSet);
                var respStream = httpResp.GetResponseStream();
                if (respStream == null) return result;
                using (var sr = new StreamReader(respStream, responseEncoding))
                {
                    result.Message = sr.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                switch (e.Status)
                {
                    case WebExceptionStatus.Timeout:
                    case WebExceptionStatus.SecureChannelFailure:
                    case WebExceptionStatus.ConnectFailure:
                        result.Status = e.Status.ToString();
                        result.Message = e.Message;
                        return result;
                }

                try
                {
                    result.Status = ((HttpWebResponse) e.Response).StatusDescription;
                    result.StatusCode = (int) ((HttpWebResponse) e.Response).StatusCode;
                    result.Message = e.Message;
                }
                catch
                {
                    result.Status = e.Status.ToString();
                    result.Message = e.Message;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }
            return result;
        }
    }


}
