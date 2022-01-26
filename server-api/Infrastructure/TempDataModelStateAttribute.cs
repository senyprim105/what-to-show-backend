using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace server_api.Infrastructure
{
    public class TempDataToModelStateErrorsAttribute : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext  filterContext)
        {
            IEnumerable<string> modelErrors = ((Controller)filterContext.Controller).TempData.GetModelErrors();
            if (modelErrors != null
                && modelErrors.Count() > 0)
            {
                modelErrors.ToList()
                           .ForEach(x => ((Controller)filterContext.Controller).ModelState.AddModelError("", x));
            }
            base.OnActionExecuting(filterContext);
        }
    }
     public class ModelStateErrorsToTempDataAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            if (!controller.ViewData.ModelState.IsValid)
            {

                if (filterContext.Result is RedirectResult
                    || filterContext.Result is RedirectToRouteResult
                    || filterContext.Result is RedirectToActionResult
                    || filterContext.Result is LocalRedirectResult
                    )
                {
                    List<string> errors = new List<string>();
                    foreach (var obj in controller.ViewData.ModelState.Values)
                    {
                        foreach (var error in obj.Errors)
                        {
                            errors.Add(error.ErrorMessage);
                        }
                    }
                    controller.TempData.AddModelErrors(errors);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
