using CleanTemplate.Api;
using CleanTemplate.Api.Filters;
using CleanTemplate.Application.Notifications;
using CleanTemplate.Application.UseCases;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Api.Tests.Filters
{
    public class HttpResponseExceptionFilterTests
    {
        HttpResponseExceptionFilter httpResponseExceptionFilter;
        Mock<ILogger<HttpResponseExceptionFilter>> log;

        [SetUp]
        public void Setup()
        {
            var webHostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            webHostingEnvironmentMock.Setup(x => x.EnvironmentName)
                .Returns("Development");

            log = new Mock<ILogger<HttpResponseExceptionFilter>>();

            httpResponseExceptionFilter = new HttpResponseExceptionFilter(
                  _SetupPresenter().Object
                , webHostingEnvironmentMock.Object
                , log.Object
            );
        }

        private Mock<IPresenter> _SetupPresenter()
        {
            var contentResult = new ContentResult();
            contentResult.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new UseCaseResult<bool>(
                new NotificationError(
                    -1,
                    $"Unexcepcted exception."
                )
            );
            contentResult.Content = JsonConvert.SerializeObject(response);

            var presenterMock = new Mock<IPresenter>();
            presenterMock
                .Setup(x => x.ActionResult)
                .Returns(contentResult);

            return presenterMock;
        }

        private Mock<ActionExecutedContext> _SetupActionExecutedContext()
        {
            var actionContextMock = new Mock<ActionContext>();
            actionContextMock.Object.HttpContext = new DefaultHttpContext();
            actionContextMock.Object.RouteData = new Microsoft.AspNetCore.Routing.RouteData();
            actionContextMock.Object.ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();

            var actionExecutedContextMock = new Mock<ActionExecutedContext>(
                actionContextMock.Object,
                Mock.Of<IList<IFilterMetadata>>(),
                Mock.Of<Controller>()
            );

            //Simulates there it ocurred an exception on the context
            actionExecutedContextMock
                .Setup(x => x.Exception)
                .Returns(new Exception("Context exception ocurred."));

            return actionExecutedContextMock;
        }

        [Test]
        public void ResponseShouldConatinAnErrorInContextResultIfThereIsAnExceptionInContext()
        {
            var actionExecutedContextMock = _SetupActionExecutedContext();

            IActionResult actionExecutedContextResult = null;
            actionExecutedContextMock
                .SetupSet<IActionResult>(x => x.Result = It.IsAny<IActionResult>())
                .Callback(value => {
                    actionExecutedContextResult = value;
            });

            httpResponseExceptionFilter.OnActionExecuted(actionExecutedContextMock.Object);

            var contentResult = actionExecutedContextResult as ContentResult;
            var useCaseResponse = JsonConvert.DeserializeObject<UseCaseResult<string>>(contentResult.Content);
            Assert.AreEqual(true, useCaseResponse.Errors.Any());
        }

        [Test]
        public void ShouldSetExceptionHandledInContextIfThereIsAnExceptionInContext()
        {
            var actionExecutedContextMock = _SetupActionExecutedContext();

            bool actionExecutedContextExceptionHandled = false;
            actionExecutedContextMock
            .SetupSet<bool>(x => x.ExceptionHandled = It.IsAny<bool>())
            .Callback(value => {
                actionExecutedContextExceptionHandled = value;
            });

            httpResponseExceptionFilter.OnActionExecuted(actionExecutedContextMock.Object);

            actionExecutedContextExceptionHandled.Should().BeTrue();
        }

        [Test]
        public void ShouldLogErrorExceptionHandledInContextIfThereIsAnExceptionInContext()
        {
            var actionExecutedContextMock = _SetupActionExecutedContext();

            httpResponseExceptionFilter.OnActionExecuted(actionExecutedContextMock.Object);

            Func<object, Type, bool> state = (v, t) => true;
            log.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))
            );
        }
    }
}
