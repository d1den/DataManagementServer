using DataManagementServer.AppServer.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Pagination.Pages;
using System.Collections.Generic;
using Pagination;

namespace DataManagementServer.AppServer.Controllers
{
    public abstract class CommonController : ControllerBase
    {
        public ActionResult<T> ExecuteWithValidateAndHandleErrors<T>(Func<T> func)
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


        public void RequestIsValidOrThrown()
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

        public Page<T> Page<T>(IList<T> values, PageRequest pageRequest)
        {
            return new Page<T>(
                values.Skip(pageRequest.PageSize * (pageRequest.PageNumber - 1)).Take(pageRequest.PageSize).ToList<T>(),
                pageRequest.PageSize,
                pageRequest.PageNumber,
                values.Count
                );
        }
    }
}
