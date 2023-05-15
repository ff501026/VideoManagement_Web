using System;
using System.ComponentModel.DataAnnotations; //提供定義數據控件的類的特性，包含[Required]等數據驗證常用的特性

namespace VideoManagement.Models
{
    public class DateRangeAttribute : RangeAttribute //為數據字段的值指定數值範圍約束。
    {
        public DateRangeAttribute(string minimumValue)
            : base(typeof(DateTime), minimumValue, DateTime.Now.ToShortDateString())
        {
        }
    }
}