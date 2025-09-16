using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Repositories.Entities;
using Repositories.Entities.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace V7.Services
{
    /// <summary>
    /// Serve and handle token proccess
    /// </summary>
    public static class TokenManager
    {
        /// <summary>
        /// Generate token and store the user info as claim name
        /// </summary>
        /// <param name="Secret">secret phrase from configuration</param>
        /// <param name="UserClaim">Any user data to store</param>
        /// <param name="TokenAge">set in minutes</param>
        /// <returns></returns>
        public static string GenerateToken(string Secret, AuthClaim UserClaim, int TokenAge)
        {
            byte[] key = Encoding.UTF8.GetBytes(Secret);
            SymmetricSecurityKey securityKey = new(key);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(UserClaim))
            }),
                // token will expired in x minute
                Expires = DateTime.UtcNow.AddMinutes(TokenAge),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        /// <summary>
        /// Get user stored info by token
        /// </summary>
        /// <param name="Secret">secret phrase from configuration</param>
        /// <param name="token">token generated from login proccess</param>
        /// <returns></returns>
        public static AuthClaim GetClaim(string Secret, string token)
        {
            AuthClaim result = null!;

            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null!;
                byte[] key = Encoding.UTF8.GetBytes(Secret);
                TokenValidationParameters parameters = new()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out SecurityToken securityToken);
                ClaimsIdentity? identity = null;
                try
                {
                    if (principal != null && principal.Identity != null)
                    {
                        identity = (ClaimsIdentity)principal.Identity;
                        Claim? myClaim = identity.FindFirst(ClaimTypes.Name);
                        if (myClaim != null && myClaim.Value != null)
                        {
                            result = JsonConvert.DeserializeObject<AuthClaim>(myClaim.Value)!; 
                        }
                    }
                }
                catch (NullReferenceException nex)
                {
                    Console.WriteLine(nex.Message);
                }
            }
            catch
            {
                throw new Exception("Failed to get claim");
            }
            return result;
        }
    }
}
