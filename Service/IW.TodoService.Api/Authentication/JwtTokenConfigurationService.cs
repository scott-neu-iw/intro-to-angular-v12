using System.Text;
using IW.TodoService.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IW.TodoService.Api.Authentication
{
   public static class JwtTokenConfigurationService
   {
      public static void AddJwtAuthorization(this IServiceCollection services, AuthenticationSettings settings)
      {
         services.AddAuthentication(x =>
            {
               x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.JwtSecretKey)),
                  ValidateIssuer = false,
                  ValidateAudience = false
               };
            });
      }

      public static SigningCredentials GetSigningCredentials(AuthenticationSettings settings)
      {
         return new SigningCredentials(GetSymmetricSecurityKey(settings), SecurityAlgorithms.HmacSha256Signature);
      }

      private static SymmetricSecurityKey GetSymmetricSecurityKey(AuthenticationSettings settings)
      {
         return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.JwtSecretKey));
      }
   }
}
