using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiDemo.Attirbutes;
using WebApiDemo.Authority;

namespace WebApiDemo.Filters.AuthFilters
{
    public class JwtTokenAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var strSecretKey = config.GetValue<string>("SecretKey");
            var claims = Authenticator.VerifyToken(token, strSecretKey);

            if (claims == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            else
            {
                var requiredClaims = context.ActionDescriptor.EndpointMetadata.OfType<RequiredClaimAttribute>().ToList();
                if(!requiredClaims.All(rc => claims.Any(c=> c.Type.ToLower() == rc.ClaimType.ToLower() && c.Value.ToLower() == rc.ClaimValue.ToLower()) && requiredClaims != null))
                {
                    context.Result = new StatusCodeResult(403);
                }

            }
        }
    }
}
