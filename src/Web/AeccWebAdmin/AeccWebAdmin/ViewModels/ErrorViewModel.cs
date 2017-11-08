using System;

namespace AeccApi.WebAdmin.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);
    }
}