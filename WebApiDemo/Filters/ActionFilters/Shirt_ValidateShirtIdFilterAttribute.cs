﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiDemo.Data;

namespace WebApiDemo.Filters.ActionFilters
{
    public class Shirt_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;

        public Shirt_ValidateShirtIdFilterAttribute(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var shirtId = context.ActionArguments["id"] as int?;

            if (shirtId.HasValue)
            {
                if (shirtId.Value <= 0)
                {
                    context.ModelState.AddModelError("ShirtId", "Id should be greater than zero");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else
                {
                    var shirt = _dbContext.Shirts.Find(shirtId.Value);

                    if (shirt == null)
                    {

                        context.ModelState.AddModelError("ShirtId", "Shirt doesn't exist");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["shirt"] = shirt;
                    }

                }
            }
        }
    }
}
