using VideoManagement.Dao;
using VideoManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManagement.Service
{
    public class DropDownListService : IDropDownListService
    {
        private IDropDownListDao dropDownListDao { get; set; }
        /// <summary>
        /// 取得影片類別的部分資料
        /// </summary>
        /// <returns>影片類別下拉選單</returns>
        public List<DropDownList> GetVideoClassId()
        {
            return dropDownListDao.GetVideoClassId();
        }

        /// <summary>
        /// 取得影片狀態的部分資料
        /// </summary>
        /// <returns>影片狀態下拉選單</returns>
        public List<DropDownList> GetVideoStatus(string type)
        {
            return dropDownListDao.GetVideoStatus(type);
        }

        /// <summary>
        /// 取得借閱人的部分資料
        /// </summary>
        /// <returns>借閱人下拉選單</returns>
        public List<DropDownList> GetMemberMId()
        {
            return dropDownListDao.GetMemberMId();
        }
    }
}
