using VideoManagement.Model;
using System.Collections.Generic;

namespace VideoManagement.Service
{
    public interface IDropDownListService
    {
        List<DropDownList> GetVideoClassId();
        List<DropDownList> GetVideoStatus(string type);
        List<DropDownList> GetMemberMId();
    }
}