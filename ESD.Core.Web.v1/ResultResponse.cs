using System.Text.Json.Serialization;

namespace ESD.Core.Web
{
   public class ResultResponse
   {
      [JsonPropertyName("success")]
      public bool Success { get; set; } = true;
      [JsonPropertyName("message")]
      public string Message { get; set; }
      [JsonPropertyName("url")]
      public string Url { get; set; }
      [JsonPropertyName("data")]
      public object Data { get; set; }
   }
}
