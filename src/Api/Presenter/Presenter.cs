using CleanTemplate.Application;
using CleanTemplate.Application.Notifications;
using CleanTemplate.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CleanTemplate.Api
{
    public class Presenter : IOutputPort<UseCaseResultMessageBase>, IOutputPort<IEnumerable<UseCaseResultMessageBase>>, IPresenter
    {
        private IActionResult _ActionResult;
        public IActionResult ActionResult { get { return _ActionResult; } }

        public Presenter()
        {
            _ActionResult = new ContentResult();
        }

        public void Handler(UseCaseInvalidResult invalidResult)
        {
            var contentResult = ActionResult as ContentResult;
            contentResult.StatusCode = (int)HttpStatusCode.BadRequest;
            contentResult.Content = JsonConvert.SerializeObject(invalidResult);
        }

        public void Handler(UseCaseResultMessageBase response)
        {
            var contentResult = ActionResult as ContentResult;
            if (!response.AnyErrors())
            {
                contentResult.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                contentResult.StatusCode = (int)HttpStatusCode.OK;
            }
            contentResult.Content = JsonConvert.SerializeObject(response);
        }

        /// <summary>
        /// It at least one user case response was successful it returns HttpStatusCode.OK
        /// If all user cases were unsuccessful it returns HttpStatusCode.BadRequest
        /// </summary>
        /// <param name="responses"></param>
        public void Handler(IEnumerable<UseCaseResultMessageBase> responses)
        {
            var areAllInvalid = responses.Where(w => w.AnyErrors()).Count() == responses.Count();
            object responseMessage = new { errors = responses.Select(x => x.Errors) };

            var contentResult = ActionResult as ContentResult;
            contentResult.StatusCode = areAllInvalid ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;
            contentResult.Content = JsonConvert.SerializeObject(responses);
        }

        public void Handler(Exception exception, bool outputExceptionDetailsToResponse)
        {
            var contentResult = ActionResult as ContentResult;
            contentResult.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new UseCaseResult<bool>(
                new NotificationError(
                    -1,
                    $"Unexcepcted exception.\r\n{(outputExceptionDetailsToResponse?exception:"")}"
                )
            );
            contentResult.Content = JsonConvert.SerializeObject(response);
        }
    }
}
