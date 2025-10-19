

namespace ECinema.ViewModels
{
    namespace ECinema.ViewModels
    {
        public class FilterMovieVM
        {
            // لازم يكون فيه constructor فاضي
            public FilterMovieVM()
            {
            }

            // وبعد كده حط الخصائص اللي بتستخدمها
            public string? Name{ get; set; }
            public int? CategoryId { get; set; }
            public decimal? Price { get; set; }
            public int? CinemaId { get; set; }
            public int? ActorId { get; set; }

        }
    }

}
