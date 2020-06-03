using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForcast.Services
{
    public interface IFbBotService
    {
        Task Reply(string SenderId, string textInput);
    }
}