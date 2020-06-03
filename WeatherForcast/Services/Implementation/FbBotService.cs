using DataMigration;
using Messenger.Client.Objects;
using Messenger.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForcast.Common;

namespace WeatherForcast.Services.Implementation
{
    public class FbBotService : IFbBotService
    {
        private readonly IMessengerProfileProvider _messengerProfile;
        private readonly DataMigrationContext _context;
        private readonly IWeatherService _weatherService;
        private readonly IMessengerMessageSender messageSender;

        public FbBotService(IMessengerProfileProvider messengerProfile,
            DataMigrationContext contexts,
            IWeatherService weatherService,
            IMessengerMessageSender messageSender)
        {
            _messengerProfile = messengerProfile;
            _context = contexts;
            _weatherService = weatherService;
            this.messageSender = messageSender;
        }

        public async Task Reply(string SenderId, string textInput)
        {
            var Sender = await _messengerProfile.GetUserProfileAsync(SenderId);
            var checkInput = _context.MesBotEntities.FirstOrDefault(x => CheckChat(textInput, x.Exception));
            if (checkInput == null)
            {
                var text = "Cậu nói gì vậy mình hông hiểu gì hớt -.-";
                var response = new MessengerMessage { Text = text };
                await messageSender.SendAsync(response, new MessengerUser { Id = SenderId });
            }
            else
            {
                if (checkInput.Type == 1)
                {
                    var weather = await _weatherService.GetWeatherFromCity(checkInput.ReplyText);
                    if (weather != null)
                    {
                        var response = new MessengerMessage { Text = $"Dự báo thời tiết hôm nay {DateTime.UtcNow.ToString("dd.MM.yyyy H:mm:ss")}, khu vực {weather.name} NĐ là: {weather.main.temp}, NĐ cao nhất: {weather.main.temp_max}, thấp nhất: {weather.main.temp_max} có{weather.weather[0].description} <3 From Khoapiterrr with love :* " };
                        await messageSender.SendAsync(response, new MessengerUser { Id = SenderId });
                    }
                    else
                    {
                        var text = "Ơ sao mình k thấy thành phố đó nhỉ :( xin loi nha";
                        var response = new MessengerMessage { Text = text };
                        await messageSender.SendAsync(response, new MessengerUser { Id = SenderId });
                    }
                }
                else if (checkInput.Type == 2)
                {
                    var text = checkInput.ReplyText.Replace("@firstname", Sender.Result.FirstName);
                    var response = new MessengerMessage { Text = text };
                    await messageSender.SendAsync(response, new MessengerUser { Id = SenderId });
                }
            }
        }

        /// <summary>
        /// Kiểm tra text người chat
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool CheckChat(string input, string ex)
        {
            var lst = ex.Split(',');
            var check = lst.FirstOrDefault(x => CommonHelper.ConvertUtf8ConvertNotReplcae(x).Equals(CommonHelper.ConvertUtf8ConvertNotReplcae(input)));
            return check != null;
        }
    }
}