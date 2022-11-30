using DataManagementServer.Common.Models;
using DataManagementServer.Sdk.Channels;
using Microsoft.AspNetCore.Mvc;
using Pagination;
using Pagination.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataManagementServer.AppServer.Controllers
{
    /// <summary>
    /// Контроллер каналов
    /// </summary>
    public class ChannelController : CommonController
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
        public ActionResult<ChannelModel> CreateChannelInGroup([FromQuery(Name = "groupId")] Guid groupId)
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
        public ActionResult<ChannelModel> GetChannel([FromQuery(Name = "channelId")] Guid channelId)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                ChannelModel channel = _ChannelService.Retrieve(channelId, true);
                return channel;
            });
        }

        [HttpGet]
        public ActionResult<ChannelModel> GetChannelFields(
            [FromQuery(Name = "channelId")] Guid channelId,
            [FromQuery(Name = "fields")] IList<string> fields)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                ChannelModel channel = _ChannelService.Retrieve(channelId, fields.ToArray());
                return channel;
            });
        }

        [HttpGet]
        public ActionResult<Page<ChannelModel>> GetChannelByGroup(
            [FromQuery(Name = "groupId")] Guid groupId,
             [FromQuery(Name = "page")] PageRequest pageRequest)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<ChannelModel> channels = _ChannelService.RetrieveByGroup(groupId, true);
                return Page(channels, pageRequest);
            });
        }

        [HttpGet]
        public ActionResult<Page<ChannelModel>> GetChannelByGroupFields(
            [FromQuery(Name = "groupId")] Guid groupId,
            [FromQuery(Name = "fields")] IList<string> fields,
            [FromQuery(Name = "page")] PageRequest pageRequest)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<ChannelModel> channels = _ChannelService.RetrieveByGroup(groupId, fields.ToArray());
                return Page(channels, pageRequest);
            });
        }

        [HttpGet]
        public ActionResult<Page<ChannelModel>> GetChannelAll([FromQuery(Name = "page")] PageRequest pageRequest)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<ChannelModel> channels = _ChannelService.RetrieveAll(true);
                return Page(channels, pageRequest);
            });
        }

        [HttpGet]
        public ActionResult<Page<ChannelModel>> GetChannelAllFields(
            [FromQuery(Name = "fields")] IList<string> fields,
            [FromQuery(Name = "page")] PageRequest pageRequest)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<ChannelModel> channels = _ChannelService.RetrieveAll(fields.ToArray());
                return Page(channels, pageRequest);
            });
        }

        [HttpPatch]
        public ActionResult<ChannelModel> UpdateChannelByModel([FromBody] ChannelModel model)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                _ChannelService.Update(model);
                ChannelModel channel = _ChannelService.Retrieve(model.Id);
                return channel;
            });
        }

        [HttpDelete]
        public ActionResult DeleteChannel([FromQuery(Name = "channelId")] Guid channelId)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                _ChannelService.Delete(channelId);
            });
        }
    }
}
