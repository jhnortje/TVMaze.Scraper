using Microsoft.EntityFrameworkCore;
using TVMaze.Infrastructure.Data.Entities;
using TVMaze.Infrastructure.Data.Mapping;

namespace TVMaze.Infrastructure.EntityFramework
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public ApplicationContext()
        {
        }

        public DbSet<Cast> Cast { get; set; }
        public DbSet<CastShow> CastShow { get; set; }
        public DbSet<Show> Show { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new CastMap(modelBuilder.Entity<Cast>());
            new ShowMap(modelBuilder.Entity<Show>());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO: move the Connections String from Web Project this project.  So we can share with Scrapper Service.
                optionsBuilder.UseSqlServer(@"server=.;database=TVMaze;Integrated Security=true;");
            }
        }
    }
}
