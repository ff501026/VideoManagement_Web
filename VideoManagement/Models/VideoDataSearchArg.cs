using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VideoManagement.Models
{
    public class VideoDataSearchArg
    {
        /// <summary>
        /// 書籍名稱
        /// </summary>
        [DisplayName("書名")]
        [MaxLength(200, ErrorMessage = "{0} 不得高於 {1} 個字元")]
        public string VideoName { get; set; }

        /// <summary>
        /// 類別代號
        /// </summary>
        [DisplayName("圖書類別")]
        [MaxLength(4, ErrorMessage = "{0} 不得高於 {1} 個字元")]
        public string VideoClassId { get; set; }

        /// <summary>
        /// 書籍保管人
        /// </summary>
        [DisplayName("借閱人")]
        [MaxLength(12, ErrorMessage = "{0} 不得高於 {1} 個字元")]
        public string VideoKeeperId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        [DisplayName("借閱狀態")]
        [MaxLength(1, ErrorMessage = "{0} 不得高於 {1} 個字元")]
        public string VideoStatusId { get; set; }


    }
}