using DataManagementServer.Common.Models;
using DataManagementServer.Sdk.Channels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace DataManagementServer.AppServer.Controllers
{
    /// <summary>
    /// Контроллер каналов
    /// </summary>
    public class ChannelController : ControllerBase
    {
        /// <summary>
        /// Сервис каналов
        /// </summary>
        private readonly IChannelService _ChannelService;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="channelService">Сервис каналов</param>
        /// <exception cref="ArgumentNullException">Ошибка при Null аргументах</exception>
        public ChannelController(IChannelService channelService)
        {
            _ChannelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
        }

        [HttpGet]
        public ActionResult<ChannelModel> GetSample()
        {
            try
            {
                var sampleModel = new ChannelModel()
                {
                    Id = Guid.NewGuid(),
                    Description = "Test model",
                    Status = ChannelStatus.Good,
                    ValueType = TypeCode.Boolean,
                    Value = true
                };

                return Ok(sampleModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
