using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.BD.Services
{
    public class BdService
    {
        public static string ConvertDatetimeToString(DateTime dateTime)
        {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}";
            return string.Format(dateTimeFormat, dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static DateTime? ConvertStringToDateTime(string strDateTime)
        {
            if (string.IsNullOrEmpty(strDateTime)) return null;

            string[] partesData = strDateTime.Trim().Replace("-", ":").Replace(".", ":").Replace(" ", ":").Split(':');
            if (partesData.Count() == 0) return null;

            DateTime dateTime = new DateTime(Convert.ToInt32(partesData[0]), Convert.ToInt32(partesData[1]), Convert.ToInt32(partesData[2]), Convert.ToInt32(partesData[3]), Convert.ToInt32(partesData[4]), Convert.ToInt32(partesData[5]));
            return dateTime;
        }


    }
}
