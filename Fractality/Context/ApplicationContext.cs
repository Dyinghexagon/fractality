using Fractality.Models;
using Microsoft.EntityFrameworkCore;

namespace Fractality.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        private readonly IConfiguration _configuration;

        public ApplicationContext(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
    }
}