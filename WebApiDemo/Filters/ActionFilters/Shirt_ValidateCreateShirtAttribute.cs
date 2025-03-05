using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiDemo.Data;
using WebApiDemo.Models;

namespace WebApiDemo.Filters.ActionFilters
{
    public class Shirt_ValidateCreateShirtAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;

        public Shirt_ValidateCreateShirtAttribute(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
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
                var existingShirt = _dbContext.Shirts.FirstOrDefault
                    (s =>
                        !string.IsNullOrWhiteSpace(shirt.Brand) &&
                        !string.IsNullOrWhiteSpace(s.Brand) &&
                        s.Brand.ToLower() == shirt.Brand.ToLower() &&
                        !string.IsNullOrWhiteSpace(shirt.Color) &&
                        !string.IsNullOrWhiteSpace(s.Color) &&
                        s.Color.ToLower() == shirt.Color.ToLower() &&
                        !string.IsNullOrWhiteSpace(shirt.Gender) &&
                        !string.IsNullOrWhiteSpace(s.Gender) &&
                        s.Gender.ToLower() == shirt.Gender.ToLower() &&
                        shirt.Size.HasValue &&
                        s.Size.HasValue &&
                        shirt.Size.Value == s.Size.Value);

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
