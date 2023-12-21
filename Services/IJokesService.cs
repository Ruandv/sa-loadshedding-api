using Models.Jokes;
using System.Threading.Tasks;

namespace Services
{
    public interface IJokesService
    {
        Task<JokeDto> GetJoke();
    }
}