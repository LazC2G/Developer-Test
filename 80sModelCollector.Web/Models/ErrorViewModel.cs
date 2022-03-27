using System;

namespace _80sModelCollector.Models
{
    /// <summary>
    /// An out of the box class provided for error handling
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
