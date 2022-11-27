using DataManagementServer.AppServer.Responses;
using DataManagementServer.AppServer.Utils;
using DataManagementServer.Common.Models;
using DataManagementServer.Sdk.Channels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [HttpPost]
        public ActionResult<ChannelModel> CreateChannel()
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                Guid id = _ChannelService.Create();
                ChannelModel channel = _ChannelService.Retrieve(id, true);
                return channel;
            });

        }

        [HttpPost]
        public ActionResult<ChannelModel> CreateChannelInGroup([FromQuery(Name = "groupId")][NotEmpty] Guid groupId)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                Guid channelId = _ChannelService.Create(groupId);
                ChannelModel channel = _ChannelService.Retrieve(channelId, true);
                return channel;
            });
        }

        [HttpPost]
        public ActionResult<ChannelModel> CreateChannelByModel([FromBody] ChannelModel model)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                Guid channelId = _ChannelService.Create(model);
                ChannelModel channel = _ChannelService.Retrieve(channelId, true);
                return channel;
            });
        }

        [HttpGet]
        public ActionResult<ChannelModel> GetChannel([FromQuery(Name = "channelId")][NotEmpty] Guid channelId)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                ChannelModel channel = _ChannelService.Retrieve(channelId, true);
                return channel;
            });
        }

        [HttpGet]
        public ActionResult<ChannelModel> GetChannelFields([FromQuery(Name = "channelId")][NotEmpty] Guid channelId, [FromQuery(Name = "fields")] IList<string> fields)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                ChannelModel channel = _ChannelService.Retrieve(channelId, fields.ToArray());
                return channel;
            });
        }

        [HttpGet]
        public ActionResult<IList<ChannelModel>> RetrieveByGroup([FromQuery(Name = "groupId")][NotEmpty] Guid groupId)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<ChannelModel> channel = _ChannelService.RetrieveByGroup(groupId, true);
                return channel;
            });
        }

        [HttpGet]
        public ActionResult<IList<ChannelModel>> RetrieveByGroupFields([FromQuery(Name = "groupId")][NotEmpty] Guid groupId, [FromQuery(Name = "fields")] IList<string> fields)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<ChannelModel> channel = _ChannelService.RetrieveByGroup(groupId, fields.ToArray());
                return channel;
            });
        }

        [HttpGet]
        public ActionResult<IList<ChannelModel>> RetrieveAll()
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<ChannelModel> channel = _ChannelService.RetrieveAll(true);
                return channel;
            });
        }

        [HttpGet]
        public ActionResult<IList<ChannelModel>> RetrieveAllFields([FromQuery(Name = "fields")] IList<string> fields)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<ChannelModel> channel = _ChannelService.RetrieveAll(fields.ToArray());
                return channel;
            });
        }

        private ActionResult<T> ExecuteWithValidateAndHandleErrors<T>(Func<T> func)
        {
            try
            {
                RequestIsValidOrThrown();
                return Ok(func.Invoke());
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse() { Message = ex.Message });
            }
        }


        private void RequestIsValidOrThrown()
        {
            if (!ModelState.IsValid)
            {
                throw new Exception(string.Join(
                    ",",
                    ModelState.Values
                                .SelectMany(x => x.Errors)
                                .Select(x => x.ErrorMessage)
                                .ToArray()));
            }
        }
    }
}
