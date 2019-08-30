using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Orange.ApiTokenValidation.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NullModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var nullArgs = context.ActionArguments.Where(x => x.Value == null).ToList();
            if (nullArgs.Any())
            {
                context.Result = new BadRequestObjectResult(
                    $"The argument cannot be null: {string.Join(", ", nullArgs.Select(x => x.Key))}");
            }
        }
    }
}
