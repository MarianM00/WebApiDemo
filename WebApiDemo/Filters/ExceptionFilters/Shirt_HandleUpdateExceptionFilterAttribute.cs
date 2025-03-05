using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiDemo.Data;

namespace WebApiDemo.Filters.ExceptionFilters
{
    public class Shirt_HandleUpdateExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ApplicationDbContext _dbContext;

        public Shirt_HandleUpdateExceptionFilterAttribute(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var strShirtId = context.RouteData.Values["id"] as string;
            if (int.TryParse(strShirtId, out int shirtId))
            {
                if (_dbContext.Shirts.FirstOrDefault(x => x.ShirtId == shirtId) == null)
                {
                    context.ModelState.AddModelError("ShirtId", "Shirt doesn't exist anymore");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
        }
    }
}
