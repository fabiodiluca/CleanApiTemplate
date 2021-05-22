using CleanTemplate.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CleanTemplate.Api
{
    public interface IPresenter
    {
        IActionResult ActionResult { get; }

        void Handle(IEnumerable<UseCaseResultMessageBase> responses);
        void Handle(UseCaseResultMessageBase response);
        void Handler(Exception exception, bool outputExceptionDetailsToResponse);
    }
}