using CleanTemplate.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
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
        public void HandleSingleResultWithNoErrorsShouldReturnHttpStatusCodeOk()
        {
            presenter.Handle(useCaseResultWithNoErrors);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            Assert.AreEqual((int)HttpStatusCode.OK, contentResult.StatusCode);
        }

        [Test]
        public void HandleSingleResultShouldSerializeDataToJsonObjectDataPropertyOfResponse()
        {
            presenter.Handle(useCaseResultWithNoErrors);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            var deserializedResult = JsonConvert.DeserializeObject<UseCaseResult<Data>>(contentResult.Content);
            Assert.AreEqual(true, useCaseResultWithNoErrors.Data.Equals(deserializedResult.Data));
        }

        [Test]
        public void HandleSingleResultWithAnyErrorsShouldReturnHttpStatusCodeBadRequest()
        {
            presenter.Handle(useCaseResultWithErrors);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, contentResult.StatusCode);
        }

        [Test]
        public void HandleSingleResultShouldSerializeErrorsToJsonObjectErrorsPropertyOfResponse()
        {
            presenter.Handle(useCaseResultWithErrors);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            var deserializedResult = JsonConvert.DeserializeObject<UseCaseResult<Data>>(contentResult.Content);
            var expectedErrorsList = useCaseResultWithErrors.Errors.ToList();
            var errorList = deserializedResult.Errors.ToList();

            Assert.AreEqual(expectedErrorsList.Count, errorList.Count);

            for (int i = 0; i < expectedErrorsList.Count; i++)
                Assert.AreEqual(expectedErrorsList[i], errorList[i]);
        }

        [Test]
        public void HandleMultipleResultsThatHaveAtLeastOneWithNoErrorsShouldReturnHttpStatusCodeOk()
        {
            var useCaseResults = new List<UseCaseResult<Data>>() { useCaseResultWithNoErrors, useCaseResultWithErrors };
            presenter.Handle(useCaseResults);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            Assert.AreEqual((int)HttpStatusCode.OK, contentResult.StatusCode);
        }

        [Test]
        public void HandleMultipleResultsThatOnlyHaveResultdWithErrorsShouldReturnHttpStatusCodeBadRequest()
        {
            var useCaseResults = new List<UseCaseResult<Data>>() { useCaseResultWithErrors, useCaseResultWithErrors };
            presenter.Handle(useCaseResults);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, contentResult.StatusCode);
        }

        [Test]
        public void HandleMultipleResultShouldSerializeDataToJsonObjectDataPropertyOfResponse()
        {
            var useCaseResults = new List<UseCaseResult<Data>>() { useCaseResultWithNoErrors, useCaseResultWithErrors };
            presenter.Handle(useCaseResults);
            var actionResult = presenter.ActionResult;
            var contentResult = actionResult as ContentResult;
            var deserializedResults = JsonConvert.DeserializeObject<UseCaseResult<Data>[]>(contentResult.Content);
            for (int i = 0; i < deserializedResults.Length; i++)
                Assert.AreEqual(deserializedResults[i].Data, useCaseResults[i].Data);
        }
    }
}
