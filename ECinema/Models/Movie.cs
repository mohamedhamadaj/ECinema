namespace ECinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Price { get; set; }
        
        public double Rate { get; set; }
        public string MainImg { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; } = null!;

        public List<Actor> Actors { get; set; } 
        public List<MovieSubimage> MovieSubimages { get; set; }
    }
}
