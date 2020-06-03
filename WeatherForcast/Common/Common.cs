using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherForcast.Common
{
    public class CommonHelper
    {
        public static string ConvertToUnSign3(string s)
        {
            if (s == null)
            {
                return null;
            }
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }

        public static string getIpAccess()
        {
            try
            {
                IPHostEntry host;
                string localIP = "?";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        localIP = ip.ToString();
                    }
                }
                return localIP;
            }
            catch (Exception)
            {
                return "Không tìm thấy địa chỉ IP";
            }
        }

        public static string ConvertUtf8Convert(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(" ", "-").Replace("/", "-").ToLower();
        }

        public static string ConvertUtf8ConvertNotReplcae(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(" ", string.Empty).ToLower();
        }

        public static string GenerateId()
        {
            var code = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            return code.ToString();
        }
    }
}