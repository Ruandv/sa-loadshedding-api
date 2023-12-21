using Models.Jokes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
                    url = System.Net.WebUtility.UrlDecode(url);
                    // Handle CORS if needed
                    if (url.StartsWith("https://"))
                    {
                        client.DefaultRequestHeaders.Add("Origin", "*"); // Replace with your app's origin
                    }
                    // decude the url

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                    var guid = Guid.NewGuid().ToString();
                    File.WriteAllBytes("./services/data/" + guid + ".jpg", fileBytes);
                    //return guid + ".jpg";
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