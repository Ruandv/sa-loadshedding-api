using Models.Jokes;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Services
{
    public class JokeService : ReadWrite, IJokesService
    {

        public JokeService()
        {
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