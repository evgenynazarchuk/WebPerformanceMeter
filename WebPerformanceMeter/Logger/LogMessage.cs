using System;

namespace WebPerformanceMeter.Logger
{
    public class LogMessage
    {
        public string User { get; set; }

        public string Request { get; set; }

        public int StatusCode { get; set; }

        public DateTime StartSendRequest { get; set; }

        public DateTime StartWaitResponse { get; set; }

        public DateTime StartResponse { get; set; }

        public DateTime EndResponse { get; set; }

        public int SendBytes { get; set; }

        public int ReceiveBytes { get; set; }

        public LogMessage(
            string user,
            string request,
            int statusCode,
            DateTime startSendRequest,
            DateTime startWaitResponse,
            DateTime startResponse,
            DateTime endResponse,
            int sendBytes,
            int receiveBytes)
        {
            User = user;
            Request = request;
            StatusCode = statusCode;
            StartSendRequest = startSendRequest;
            StartWaitResponse = startWaitResponse;
            StartResponse = startResponse;
            EndResponse = endResponse;
            SendBytes = sendBytes;
            ReceiveBytes = receiveBytes;
        }
    }
}
