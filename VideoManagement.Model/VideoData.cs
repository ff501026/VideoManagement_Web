using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManagement.Model
{
    public class VideoData
    {
        /// <summary>
        /// 影片編號
        /// </summary>
        [DisplayName("影片編號")]
        public int VideoId { get; set; }

        /// <summary>
        /// 影片名稱
        /// </summary>
        [DisplayName("影片名稱")]
        [MaxLength(200, ErrorMessage = "{0} 不得高於 {1} 個字元")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string VideoName { get; set; }

        /// <summary>
        /// 類別代號
        /// </summary>
        [DisplayName("影片類別名稱")]
        public string VideoClassName { get; set; }

        [DisplayName("影片類別")]
        [MaxLength(4, ErrorMessage = "{0} 不得高於 {1} 個字元")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string VideoClassId { get; set; }

        /// <summary>
        /// 影片作者
        /// </summary>
        [DisplayName("作者")]
        [MaxLength(30, ErrorMessage = "{0} 不得高於 {1} 個字元")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string VideoAuthor { get; set; }

        /// <summary>
        /// 影片購買日期
        /// </summary>
        [DisplayName("影片購買日期")]
        [Required(ErrorMessage = "此欄位為必填")]
        [DateRange("01/01/1753", ErrorMessage = "{0} 必須介於 1753/01/01 和當前日期之間")]
        [DataType(DataType.Date, ErrorMessage = "請輸入正確的日期格式")]
        public DateTime VideoBoughtDate { get; set; }

        /// <summary>
        /// 出版商
        /// </summary>
        [DisplayName("出版商")]
        [MaxLength(20, ErrorMessage = "{0} 不得高於 {1} 個字元")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string VideoPublisher { get; set; }

        /// <summary>
        /// 內容簡介
        /// </summary>
        [DisplayName("內容簡介")]
        [MaxLength(1200, ErrorMessage = "{0} 不得高於 {1} 個字元")]
        [Required(ErrorMessage = "此欄位為必填")]
        public string VideoNote { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        [DisplayName("借閱狀態名稱")]
        public string VideoStatusName { get; set; }

        [DisplayName("借閱狀態")]
        [MaxLength(1, ErrorMessage = "{0} 不得高於 {1} 個字元")]

        public string VideoStatusId { get; set; }

        /// <summary>
        /// 影片保管人
        /// </summary>
        [DisplayName("借閱人")]
        public string VideoKeeperName { get; set; }

        [DisplayName("借閱人")]
        [MaxLength(12, ErrorMessage = "{0} 不得高於 {1} 個字元")]

        public string VideoKeeperId { get; set; }
    }
}
