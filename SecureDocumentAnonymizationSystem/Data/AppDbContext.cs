using Microsoft.EntityFrameworkCore;
using SecureDocumentAnonymizationSystem.Models;
using System.Collections.Generic;

namespace SecureDocumentAnonymizationSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Makale> Makaleler { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<Message> Messages { get; set; }    
    }
}
