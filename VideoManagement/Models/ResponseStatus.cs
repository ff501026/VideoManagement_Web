using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoManagement.Models
{
    public class ResponseStatus
    {
        /// <summary>
        /// 狀態碼
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 狀態訊息
        /// </summary>
        public string StatusMessage { get; set; }
    }
}