using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using IW.TodoService.Api.Authentication;
using IW.TodoService.Api.DataModels;
using IW.TodoService.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IW.TodoService.Api.Services
{
   public interface IJwtTokenService
   {
      string CreateJwt(User user);
      string GenerateTokenResponse(string encodedJwt);
   }

   public class JwtTokenService : IJwtTokenService
   {
      private const string USER_AUTHORIZATION = "user_authorization";
      private readonly AuthenticationSettings _authenticationSettings;
      private readonly JsonSerializerSettings _serializerSettings;

      public JwtTokenService(IOptions<AuthenticationSettings> authenticationSettings)
         : this(authenticationSettings.Value)
      { }

      public JwtTokenService(AuthenticationSettings authenticationSettings)
      {
         _authenticationSettings = authenticationSettings;
         _serializerSettings = new JsonSerializerSettings
         {
            Formatting = Formatting.Indented,
         };
      }

      public string CreateJwt(User user)
      {
         if (user == null) throw new ArgumentNullException(nameof(user));

         var now = DateTime.UtcNow;

         var claims = new List<Claim>
         {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
               new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username ?? ""),
            new Claim(USER_AUTHORIZATION, string.Join(",", user.Roles)),
         };

         var payload = new JwtPayload(
            _authenticationSettings.Issuer,
            _authenticationSettings.Audience,
            new List<Claim>(),
            now,
            now.Add(new TimeSpan(0, _authenticationSettings.JwtTimeToLiveMinutes, 0))
         );

         payload.AddClaims(claims);

         var jwt = new JwtSecurityToken(
            new JwtHeader(JwtTokenConfigurationService.GetSigningCredentials(_authenticationSettings)), payload);

         return new JwtSecurityTokenHandler().WriteToken(jwt);

      }

      public string GenerateTokenResponse(string encodedJwt)
      {
         var response = new
         {
            accessToken = encodedJwt,
            expires = _authenticationSettings.JwtTimeToLiveMinutes * 60,
         };

         // Serialize and return the response
         return JsonConvert.SerializeObject(response, _serializerSettings);
      }

      public static User ToUser(ClaimsPrincipal principal)
      {
         if (principal == null) return null;

         var roleArray = principal.Claims.FirstOrDefault(i => i.Type == USER_AUTHORIZATION)?.Value.Split(',');
         var roles = roleArray == null
            ? new List<RoleEnum>() 
            : roleArray.Select(a => (RoleEnum) Enum.Parse(typeof(RoleEnum), a)).ToList();


         return new User
         {
            Id = Convert.ToInt64(principal.Claims.FirstOrDefault(i => i.Type == JwtRegisteredClaimNames.Sub)?.Value),
            FirstName = principal.Claims.FirstOrDefault(i => i.Type == JwtRegisteredClaimNames.GivenName)?.Value,
            LastName = principal.Claims.FirstOrDefault(i => i.Type == JwtRegisteredClaimNames.FamilyName)?.Value,
            Username = principal.Claims.FirstOrDefault(i => i.Type == JwtRegisteredClaimNames.UniqueName)?.Value,
            EmailAddress = principal.Claims.FirstOrDefault(i => i.Type == JwtRegisteredClaimNames.Email)?.Value,
            Roles = roles
         };
      }
   }
}
