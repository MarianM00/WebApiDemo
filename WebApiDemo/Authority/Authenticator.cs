using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace WebApiDemo.Authority
{
    public static class Authenticator
    {
        public static bool Authentication(string clientId, string secret)
        {
            var app = AppRepository.GetApplicationByClientId(clientId);
            if(app == null)
            {
                return false;
            }   
            return (app.ClientId == clientId && app.Secret == secret);  
        }

        public static string CreateToken(string clientId, DateTime expiresAt, string strsecretKey)
        {

            //Algo
            //Payload(Claims)
            //Signing Key

            var app = AppRepository.GetApplicationByClientId(clientId);

            var claims = new List<Claim>
            {
                new Claim("AppName", app.ApplicationName??String.Empty),
            };

            var scopes = app?.Scopes?.Split(',');
            if(scopes != null && scopes.Length > 0)
            {
                foreach (var scope in scopes)
                {
                    claims.Add(new Claim(scope.ToLower(), "true"));
                }
            }

            var secretKey = Encoding.ASCII.GetBytes(strsecretKey);

            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
                claims: claims,
                expires: expiresAt,
                notBefore: DateTime.Now
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static IEnumerable<Claim>? VerifyToken(string token, string strSecretKey)
        {
            if (string.IsNullOrEmpty(token)) return null;

            if(token.StartsWith("Bearer"))
            {
                token = token.Substring(6).Trim();
            }   

            SecurityToken securityToken;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(strSecretKey)),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                },
                out securityToken);

                if(securityToken != null)
                {
                    var tokenObject = tokenHandler.ReadJwtToken(token);
                    return tokenObject.Claims??(new List<Claim>());
                }
                else
                {
                    return null;
                }
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}
