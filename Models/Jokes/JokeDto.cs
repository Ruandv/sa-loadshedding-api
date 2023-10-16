using System.Text.Json.Serialization;

namespace Models.Jokes
{
    public class JokeDto
    {
        [JsonPropertyName("joke")] 
        public string Joke { get; set; }
        [JsonPropertyName("answer")]
        public string Answer { get; set; }
    }
}
