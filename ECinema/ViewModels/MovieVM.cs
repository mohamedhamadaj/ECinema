using ECinema.Models;

namespace ECinema.ViewModels
{
    internal class MovieVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Cinema> Cinemas { get; set; }
        public Movie? Movie { get; set; }
    }
}