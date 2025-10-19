using Microsoft.EntityFrameworkCore;
using ECinema.Models;

namespace WebApplication2.DataAccess.EntityConfigration
{
    public class MovieImageEntityTypeconfigrations : IEntityTypeConfiguration<MovieSubimage>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MovieSubimage> builder)
        {
            builder.HasKey(e => new { e.MovieId, e.Img });
           
        }
    }
}
