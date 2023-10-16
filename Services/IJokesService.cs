using System.Collections;
using System.Threading.Tasks;
using Models.Jokes;

namespace Services
{
    public interface IJokesService
    {
        Task<JokeDto> GetJoke();
    }
}