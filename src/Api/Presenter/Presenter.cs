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
    public class Presenter : IOutputPort<UseCaseResponseMessageBase>, IOutputPort<IEnumerable<UseCaseResponseMessageBase>>, IPresenter
    {
        private IActionResult _ActionResult;
        public IActionResult ActionResult { get { return _ActionResult; } }

        public Presenter()
        {
            _ActionResult = new ContentResult();
        }

        public void Handler(UseCaseResponseMessageBase response)
        {
            var contentResult = ActionResult as ContentResult;
            if (response.GetHttpStatusToOverride().HasValue)
            {
                contentResult.StatusCode = response.GetHttpStatusToOverride().Value;
            }
            else if (!response.IsValid())
            {
                contentResult.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                contentResult.StatusCode = (int)HttpStatusCode.OK;
            }
            contentResult.Content = JsonConvert.SerializeObject(response);
        }

        public void Handler(IEnumerable<UseCaseResponseMessageBase> responses)
        {
            var isInvalid = responses.Where(w => w != null && !w.IsValid()).Any();
            object responseMessage = new { errors = responses.Select(x => x.Errors) };

            var contentResult = ActionResult as ContentResult;
            contentResult.StatusCode = isInvalid ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.OK;
            contentResult.Content = JsonConvert.SerializeObject(responses);
        }

        public void Handler(Exception exception, bool outputExceptionDetailsToResponse)
        {
            var contentResult = ActionResult as ContentResult;
            contentResult.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new UseCaseResponseMessage<bool>(
                new NotificationError(
                    -1,
                    $"Unexcepcted exception.\r\n{(outputExceptionDetailsToResponse?exception:"")}"
                )
            );
            contentResult.Content = JsonConvert.SerializeObject(response);
        }
    }
}
