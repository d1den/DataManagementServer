using DataManagementServer.AppServer.Utils;
using DataManagementServer.Common.Models;
using DataManagementServer.Sdk.Channels;
using Microsoft.AspNetCore.Mvc;
using Pagination;
using Pagination.Pages;
using System;
using System.Collections.Generic;

namespace DataManagementServer.AppServer.Controllers
{
    public class GroupController : CommonController
    {
        private readonly IGroupService _GroupService;

        public GroupController(IGroupService groupService)
        {
            _GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }

        [HttpPost]
        public ActionResult<GroupModel> CreateGroup()
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                Guid id = _GroupService.Create();
                GroupModel group = _GroupService.Retrieve(id);
                return group;
            });
        }

        [HttpPost]
        public ActionResult<GroupModel> CreateGroupInGroup([FromQuery(Name = "parentId")][NotEmpty] Guid parentId)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                Guid id = _GroupService.Create(parentId);
                GroupModel group = _GroupService.Retrieve(id);
                return group;
            });
        }


        [HttpPost]
        public ActionResult<GroupModel> CreateGroupByModel([FromBody] GroupModel groupModel)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                Guid id = _GroupService.Create(groupModel);
                GroupModel group = _GroupService.Retrieve(id);
                return group;
            });
        }

        [HttpPatch]
        public ActionResult<GroupModel> UpdateGroupByModel([FromBody] GroupModel groupModel)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                _GroupService.Update(groupModel);
                GroupModel group = _GroupService.Retrieve(groupModel.Id);
                return group;
            });
        }

        [HttpGet]
        public ActionResult<GroupModel> GetGroup([FromQuery(Name = "groupId")][NotEmpty] Guid groupId)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                GroupModel group = _GroupService.Retrieve(groupId);
                return group;
            });
        }

        [HttpGet]
        public ActionResult<Page<GroupModel>> GetGroupByParent(
            [FromQuery(Name = "groupId")][NotEmpty] Guid parentId,
            [FromQuery(Name = "page")] PageRequest pageRequest,
            [FromQuery(Name = "allFields")] bool allFields = true)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<GroupModel> group = _GroupService.RetrieveByParent(parentId, allFields);
                return Page(group, pageRequest);
            });
        }

        [HttpGet]
        public ActionResult<Page<GroupModel>> GetAllGroup(
            [FromQuery(Name = "page")] PageRequest pageRequest,
            [FromQuery(Name = "allFields")] bool allFields = true)
        {
            return ExecuteWithValidateAndHandleErrors(() =>
            {
                IList<GroupModel> group = _GroupService.RetrieveAll(allFields);
                return Page(group, pageRequest);
            });
        }
    }
      
}
