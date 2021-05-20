using CleanTemplate.Api;
using CleanTemplate.Api.Filters;
using CleanTemplate.Application.Notifications;
using CleanTemplate.Application.UseCases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
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
using System.Security.Claims;
using System.Threading;

namespace Api.Tests.Filters
{
    public class HttpResponseExceptionFilterTests
    {
        HttpResponseExceptionFilter _httpResponseExceptionFilter;
        Mock<ILogger<HttpResponseExceptionFilter>> log;

        [SetUp]
        public void Setup()
        {
            var webHostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            webHostingEnvironmentMock.Setup(x => x.EnvironmentName)
                .Returns("Development");

            log = new Mock<ILogger<HttpResponseExceptionFilter>>();

            _httpResponseExceptionFilter = new HttpResponseExceptionFilter(
                  _SetupPresenterMock().Object
                , webHostingEnvironmentMock.Object
                , log.Object
            );
        }

        private Mock<IPresenter> _SetupPresenterMock()
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
            presenterMock.Setup(x => x.ActionResult)
                .Returns(contentResult);
            return presenterMock;
        }

        private Mock<ActionExecutedContext> _SetupActionExecutedContextMock()
        {
            var actionContextMock = new Mock<ActionContext>();
            actionContextMock.Object.HttpContext = new FakeHttpContext();
            actionContextMock.Object.RouteData = new Microsoft.AspNetCore.Routing.RouteData();
            actionContextMock.Object.ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();

            var actionExecutedContextMock = new Mock<ActionExecutedContext>(
                actionContextMock.Object,
                Mock.Of<IList<IFilterMetadata>>(),
                Mock.Of<Controller>()
            );

            //Simulates there it ocurred an exception on the context
            actionExecutedContextMock.Setup(x => x.Exception)
                .Returns(new Exception("Context exception ocurred."));

            return actionExecutedContextMock;
        }

        [Test]
        public void ResponseMustPresentAnErrorInContextResultIfThereIsAnExceptionInContext()
        {
            var actionExecutedContextMock = _SetupActionExecutedContextMock();

            IActionResult actionExecutedContextResult = null;
            actionExecutedContextMock
                .SetupSet<IActionResult>(x => x.Result = It.IsAny<IActionResult>())
                .Callback(value => {
                    actionExecutedContextResult = value;
            });

            _httpResponseExceptionFilter.OnActionExecuted(actionExecutedContextMock.Object);

            var contentResult = actionExecutedContextResult as ContentResult;
            var useCaseResponse = JsonConvert.DeserializeObject<UseCaseResult<string>>(contentResult.Content);
            Assert.AreEqual(true, useCaseResponse.Errors.Any());
        }

        [Test]
        public void MustSetExceptionHandledInContextIfThereIsAnExceptionInContext()
        {
            var actionExecutedContextMock = _SetupActionExecutedContextMock();

            bool actionExecutedContextExceptionHandled = false;
            actionExecutedContextMock
            .SetupSet<bool>(x => x.ExceptionHandled = It.IsAny<bool>())
            .Callback(value => {
                actionExecutedContextExceptionHandled = value;
            });

            _httpResponseExceptionFilter.OnActionExecuted(actionExecutedContextMock.Object);

            Assert.AreEqual(true, actionExecutedContextExceptionHandled);
        }

        [Test]
        public void MustLogErrorExceptionHandledInContextIfThereIsAnExceptionInContext()
        {
            var actionExecutedContextMock = _SetupActionExecutedContextMock();

            _httpResponseExceptionFilter.OnActionExecuted(actionExecutedContextMock.Object);

            Func<object, Type, bool> state = (v, t) => true;
            log.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))
            );
        }

        internal class FakeHttpContext : HttpContext
        {
            public override IFeatureCollection Features => throw new NotImplementedException();

            public override HttpRequest Request => throw new NotImplementedException();

            public override HttpResponse Response => throw new NotImplementedException();

            public override ConnectionInfo Connection => throw new NotImplementedException();

            public override WebSocketManager WebSockets => throw new NotImplementedException();

            public override ClaimsPrincipal User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public override void Abort()
            {
                throw new NotImplementedException();
            }
        }
    }
}
