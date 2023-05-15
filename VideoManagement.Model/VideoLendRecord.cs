using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManagement.Model
{
    public class VideoLendRecord
    {
        /// <summary>
        /// 借閱日期
        /// </summary>
        [DisplayName("借閱日期")]
        public DateTime VideoLendDate { get; set; }

        /// <summary>
        /// 借閱人英文姓名
        /// </summary>
        [DisplayName("英文姓名")]
        public string UserEname { get; set; }

        /// <summary>
        /// 借閱人中文姓名
        /// </summary>
        [DisplayName("中文姓名")]
        public string UserCname { get; set; }

        /// <summary>
        /// 借閱人編號
        /// </summary>
        [DisplayName("借閱人員編號")]
        public string KeeperId { get; set; }
    }
}
