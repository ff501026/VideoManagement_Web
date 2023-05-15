using VideoManagement.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManagement.Dao
{
    public class DropDownListTestDao : IDropDownListDao
    {
        private readonly string rootCodeDataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\VideoManagement\File\"); // 路径


        public List<DropDownList> GetVideoClassId()
        {
            string videoClassFilePath = rootCodeDataFilePath + "VIDEO_CLASS.txt";

            var lines = File.ReadAllLines(videoClassFilePath);
            List<DropDownList> result = new List<DropDownList>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                result.Add(new DropDownList()
                {
                    text = item.Split(splitChar.ToCharArray())[1],
                    value = item.Split(splitChar.ToCharArray())[0]
                });
            }
            return result;
        }

        public List<DropDownList> GetMemberMId()
        {
            string memborFilePath = rootCodeDataFilePath + "MEMBER_M.txt";

            var lines = File.ReadAllLines(memborFilePath);
            List<DropDownList> result = new List<DropDownList>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                result.Add(new DropDownList()
                {
                    text = item.Split(splitChar.ToCharArray())[1],
                    value = item.Split(splitChar.ToCharArray())[0]
                });
            }
            return result;
        }

        public List<DropDownList> GetVideoStatus(string type)
        {
            string videoCodeFilePath = rootCodeDataFilePath + "VIDEO_CODE.txt";

            var lines = File.ReadAllLines(videoCodeFilePath);
            List<DropDownList> result = new List<DropDownList>();
            string splitChar = "\t";

            foreach (var item in lines)
            {
                if (item.Split(splitChar.ToCharArray())[0] == type)
                {
                    result.Add(new DropDownList()
                    {
                        text = item.Split(splitChar.ToCharArray())[3],
                        value = item.Split(splitChar.ToCharArray())[1]
                    });
                }

            }
            return result;
        }
    }
}
