using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VideoManagement.Common
{
    public class CodeTool
    {
        public static string DecodeStr(string str_encode)
        {
            StringWriter writer = new StringWriter();
            HttpUtility.HtmlDecode(str_encode, writer);
            String DecodedString = writer.ToString();
            return DecodedString;
        }
    }
}
