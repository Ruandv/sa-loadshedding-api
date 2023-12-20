using Models.Jokes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Models.Jokes;

namespace Services
{
    public class JokeService : ReadWrite, IJokesService
    {

        public JokeService()
        {
        }

        public async Task<string> GetImage(string url)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Handle CORS if needed
                    if (url.StartsWith("https://"))
                    {
                        client.DefaultRequestHeaders.Add("Origin", "*"); // Replace with your app's origin
                    }

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                    return Convert.ToBase64String(fileBytes);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error fetching file: " + ex.Message);
                    return null;
                }
            }
        }

        public Task<JokeDto> GetJoke()
        {
            var res = ReadFile("./services/data/jsonData/jokes.json");
            var data = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<JokeDto>>(res);
            // get a random number 
            var rnd = RandomNumberGenerator.GetInt32(data.ToList().Count());
            return Task.FromResult((data.ToList())[rnd]);

        }
    }
}