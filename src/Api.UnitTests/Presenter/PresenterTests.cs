using CleanTemplate.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace Api.UnitTests.Presenter
{
    public class PresenterTests
    {
        UseCaseResult<Data> useCaseResultWithNoErrors;
        UseCaseResult<Data> useCaseResultWithErrors;
        CleanTemplate.Api.Presenter presenter;

        [SetUp]
        public void Setup()
        {
            useCaseResultWithNoErrors = new UseCaseResult<Data>(new Data() { Id = -1, DataContent = "Test" });
            useCaseResultWithErrors = new UseCaseResult<Data>(new CleanTemplate.Application.Notifications.NotificationError(-1,"Error"));
            presenter = new CleanTemplate.Api.Presenter();
        }

        internal class Data
        {
            public int Id { get; set; }
            public string DataContent { get; set; }

            public override bool Equals(object obj)
            {
                var objCompare = obj as Data;
                if (objCompare == null)
                    return false;
                return objCompare.Id == Id &&
                        objCompare.DataContent == DataContent;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        [Test]
        public void SingleResultShouldSerializeContentToJsonToDataPropertyOfResponse()
        {
            presenter.Handle(useCaseResultWithNoErrors);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            var deserializedResult = JsonConvert.DeserializeObject<UseCaseResult<Data>>(contentResult.Content);
            Assert.AreEqual(true, useCaseResultWithNoErrors.Data.Equals(deserializedResult.Data));
        }

        [Test]
        public void SingleResultWithNoErrorsShouldReturnHttpStatusCodeOk()
        {
            presenter.Handle(useCaseResultWithNoErrors);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            Assert.AreEqual((int)HttpStatusCode.OK, contentResult.StatusCode);
        }

        [Test]
        public void SingleResultWithAnyErrorsShouldReturnHttpStatusCodeBadRequest()
        {
            presenter.Handle(useCaseResultWithErrors);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, contentResult.StatusCode);
        }
    }
}
