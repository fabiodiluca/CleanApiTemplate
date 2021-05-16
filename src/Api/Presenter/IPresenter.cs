using CleanTemplate.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CleanTemplate.Api
{
    public interface IPresenter
    {
        IActionResult ActionResult { get; }

        void Handler(IEnumerable<UseCaseResponseMessageBase> responses);
        void Handler(UseCaseResponseMessageBase response);
        void Handler(Exception exception, bool outputExceptionDetailsToResponse);
    }
}