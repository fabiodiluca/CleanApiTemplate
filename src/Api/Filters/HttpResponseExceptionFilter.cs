using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanTemplate.Api.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        protected readonly IPresenter _presenter;
        protected readonly IWebHostEnvironment _environment;
        protected readonly ILogger<HttpResponseExceptionFilter> _logger;
        public int Order { get; } = int.MaxValue - 10;

        public HttpResponseExceptionFilter(
            IPresenter presenter, 
            IWebHostEnvironment environment,
            ILogger<HttpResponseExceptionFilter> logger)
        {
            _presenter = presenter;
            _environment = environment;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                _logger.LogError(context.Exception, "Unexpected exception.");
                _presenter.Handler(context.Exception, _environment.IsDevelopment());
                context.Result = _presenter.ActionResult;
                context.ExceptionHandled = true;
            }
        }
    }
}
