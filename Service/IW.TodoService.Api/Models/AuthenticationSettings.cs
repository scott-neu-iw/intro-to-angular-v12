namespace IW.TodoService.Api.Models
{
   public class AuthenticationSettings
   {
      public string JwtSecretKey { get; set; }
      public int JwtTimeToLiveMinutes { get; set; }
      public string Issuer { get; set; }
      public string Audience { get; set; }
   }
}
