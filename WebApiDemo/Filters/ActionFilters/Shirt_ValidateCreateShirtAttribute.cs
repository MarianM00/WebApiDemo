using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Models;
using WebApiDemo.Models.Repositories;

namespace WebApiDemo.Filters.ActionFilters
{
    public class Shirt_ValidateCreateShirtAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var shirt = context.ActionArguments["shirt"] as Shirt;
            if (shirt == null)
            {
                context.ModelState.AddModelError("Shirt", "Short object is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                var existingShirt = ShirtRepository.GetShirtByProperties(shirt.Brand, shirt.Color, shirt.Gender, shirt.Size);
                if (existingShirt != null)
                {
                    context.ModelState.AddModelError("Shirt", "Short already exist.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }

            }
        }
    }
}
