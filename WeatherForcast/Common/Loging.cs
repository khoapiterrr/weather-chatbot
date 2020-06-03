using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForcast.Common
{
    public class Logging
    {
        public static void CreateLogging(SaveLogSystem saveLogSystem, string nameFile)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "wwwroot/log_system"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "wwwroot/log_system");
            }
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "wwwroot/log_system/" + nameFile + ".txt", true);
            sw.WriteLine(DateTime.Now.ToString("g") + ": " + saveLogSystem.Method + saveLogSystem.Value);
            sw.Flush();
            sw.Close();
        }

        public static void CreateLogSystem(string nameFile, string content)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "wwwroot"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "wwwroot");
            }
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "wwwroot/" + nameFile + ".txt", true);
            sw.WriteLine($"{DateTime.Now}: {content}");
            sw.Flush();
            sw.Close();
        }
    }

    public class SaveLogSystem
    {
        public string Method { get; set; }
        public string Value { get; set; }
    }
}