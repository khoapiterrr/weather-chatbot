using DataMigration;
using Hangfire;
using Messenger.Client.Objects;
using Messenger.Client.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherForcast.Common;
using WeatherForcast.Model;
using WeatherForcast.Services;

namespace WeatherForcast.api
{
    [Route("webhook")]
    public class FbChatBotController : Controller
    {
        private readonly IMessengerMessageSender messageSender;
        private readonly IFbBotService _apibot;
        private readonly IMessengerProfileProvider _messengerProfile;
        private readonly DataMigrationContext _context;
        private readonly IWeatherService _weatherService;

        public FbChatBotController(
            IMessengerMessageSender messageSender,
            IFbBotService apibot, IWeatherService weatherService,
            DataMigrationContext context,
            IMessengerProfileProvider messengerProfile)

        {
            this.messageSender = messageSender;
            _apibot = apibot;
            _weatherService = weatherService;
            _context = context;
            _messengerProfile = messengerProfile;
        }

        [HttpGet]
        public IActionResult Validate()
        {
            var challenge = Request.Query["hub.challenge"];

            if (Request.Query["hub.verify_token"].Equals("linhcutehotmexinchao"))
            {
                if (challenge.Any())
                {
                    return Ok(challenge.First());
                }
            }
            return Ok();
        }

        // Sends echo message back to sender.
        [HttpPost]
        public async Task HandleMessage([FromBody] MessengerObject obj)
        {
            foreach (var entry in obj.Entries)
            {
                var mesaging = entry.Messaging;

                foreach (var message in mesaging)
                {
                    var senderId = message.Sender.Id;
                    if (message.Message != null)
                    {
                        if (!string.IsNullOrEmpty(message.Message.Text))
                        {
                            try
                            {
                                var checkSend = await SendDBTT(message);
                                RecurringJob.AddOrUpdate(senderId,
                                                   () => SendDBTT(message),
                                                   Cron.Hourly);
                                var jobId = BackgroundJob.Schedule(
                                        () => RecurringJob.RemoveIfExists(senderId),
                                        TimeSpan.FromDays(1));
                            }
                            catch (Exception ex)
                            {
                                var response = new MessengerMessage { Text = "Xin lỗi page mình có vấn đề rồi :'( ... " + ex.Message };
                                await messageSender.SendAsync(response, message.Sender);
                            }
                        }
                        else
                        {
                            var response = new MessengerMessage { Text = "Hi chào cậu , mình đợi cậu từ chiều ơ...mà cậu đừng gửi file, ảnh linh tinh cho tớ spam tớ ghét  đấy <3" };
                            await messageSender.SendAsync(response, message.Sender);
                        }
                    }
                }
            }
        }

        private bool CheckChat(string input, string ex)
        {
            var lst = ex.Split(',');
            var check = lst.FirstOrDefault(x => CommonHelper.ConvertUtf8ConvertNotReplcae(x).Equals(CommonHelper.ConvertUtf8ConvertNotReplcae(input)));
            return check != null;
        }

        public async Task<bool> SendDBTT(MessengerMessaging mes)
        {
            var weather = await _weatherService.GetWeatherFromCity(CommonHelper.ConvertUtf8ConvertNotReplcae(mes.Message.Text));
            if (weather != null)
            {
                var response = new MessengerMessage { Text = $"DBTT {DateTime.UtcNow.ToString("dd.MM.yyyy")}, tại {weather.name} NĐ là: {weather.main.temp}, NĐ cao nhất: {weather.main.temp_max}, thấp nhất: {weather.main.temp_max} có {weather.weather[0].description} <3 From Khoapiterrr with love" };
                _ = messageSender.SendAsync(response, new MessengerUser { Id = mes.Sender.Id });
                try
                {
                    var Sender = await _messengerProfile.GetUserProfileAsync(mes.Sender.Id);

                    _context.ViewWeatherHistoryApi.Add(new Entity.ViewWeatherHistoryApi
                    {
                        City = weather.name,
                        CreatedDate = DateTime.UtcNow,
                        FacebookId = mes.Sender.Id,
                        Name = Sender.Result.FirstName + " " + Sender.Result.LastName,
                        Temperature = weather.main.temp.ToString()
                    });
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
            _ = messageSender.SendAsync(new MessengerMessage { Text = "Xin lỗi bạn, mình không tìm thấy thành phố của bạn. Vui lòng gõ chính xác" }, new MessengerUser { Id = mes.Sender.Id });
            return false;
        }

        private void SaveHistory(WeatherResponseApi obj)
        {
        }

        public void xxx()
        {
            //var Sender = await _messengerProfile.GetUserProfileAsync(senderId);
            //var checkInput = _context.MesBotEntities.FirstOrDefault(x => CheckChat(message.Message.Text, x.Exception));
            //if (checkInput == null)
            //{
            //    Random rand = new Random();
            //    int toSkip = rand.Next(0, _context.TextRandomEntities.Count() - 1);
            //    var res = _context.TextRandomEntities.Skip(toSkip).Take(1).First();

            //    //var text = "Cậu nói gì vậy mình hông hiểu gì hớt -.-";
            //    var response = new MessengerMessage { Text = res.ReplyText };
            //    await messageSender.SendAsync(response, new MessengerUser { Id = senderId });
            //}
            //else
            //{
            //    if (checkInput.Type == 1)
            //    {
            //        var weather = await _weatherService.GetWeatherFromCity(checkInput.ReplyText);
            //        if (weather != null)
            //        {
            //            var response = new MessengerMessage { Text = $"Dự báo thời tiết hôm nay {DateTime.UtcNow.ToString("dd.MM.yyyy")}, khu vực {weather.name} NĐ là: {weather.main.temp}, NĐ cao nhất: {weather.main.temp_max}, thấp nhất: {weather.main.temp_max} có{weather.weather[0].description} <3 From Khoapiterrr with love" };
            //            await messageSender.SendAsync(response, new MessengerUser { Id = senderId });
            //            RecurringJob.AddOrUpdate(senderId,
            //                            () => messageSender.SendAsync(response, new MessengerUser { Id = senderId }),
            //                            Cron.Hourly);
            //            var jobId = BackgroundJob.Schedule(
            //                    () => RecurringJob.RemoveIfExists(senderId),
            //                    TimeSpan.FromDays(1));
            //        }
            //        else
            //        {
            //            var text = "Ơ sao mình k thấy thành phố đó nhỉ :( xin loi nha";
            //            var response = new MessengerMessage { Text = text };
            //            await messageSender.SendAsync(response, new MessengerUser { Id = senderId });
            //        }
            //    }
            //    else if (checkInput.Type == 2)
            //    {
            //        var text = checkInput.ReplyText.Replace("@firstname", Sender.Result.FirstName);
            //        var response = new MessengerMessage { Text = text };
            //        await messageSender.SendAsync(response, new MessengerUser { Id = senderId });
            //    }
            //    else
            //    {
            //        var response = new MessengerMessage { Text = checkInput.ReplyText };
            //        await messageSender.SendAsync(response, new MessengerUser { Id = senderId });
            //    }
            //}
        }
    }
}