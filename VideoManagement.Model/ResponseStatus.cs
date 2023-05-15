using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManagement.Model
{
    public class ResponseStatus
    {
        /// <summary>
        /// 狀態碼
        /// </summary>
        public bool StatusCode { get; set; }
        /// <summary>
        /// 狀態訊息
        /// </summary>
        public string StatusMessage { get; set; }
    }
}
