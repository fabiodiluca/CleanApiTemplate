using CleanTemplate.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CleanTemplate.Api
{
    public interface IPresenter
    {
        IActionResult ActionResult { get; }

        void Handler(IEnumerable<UseCaseResultMessageBase> responses);
        void Handler(UseCaseResultMessageBase response);
        void Handler(Exception exception, bool outputExceptionDetailsToResponse);
    }
}