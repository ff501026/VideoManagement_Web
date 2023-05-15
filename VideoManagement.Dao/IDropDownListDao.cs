using VideoManagement.Model;
using System.Collections.Generic;

namespace VideoManagement.Dao
{
    public interface IDropDownListDao
    {
        List<DropDownList> GetVideoClassId();
        List<DropDownList> GetVideoStatus(string type);
        List<DropDownList> GetMemberMId();
    }
}