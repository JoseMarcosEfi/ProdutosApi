using ApiProdutos.Entities;
using Microsoft.EntityFrameworkCore;
namespace ApiProdutos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.Cpf);
        }
    }


}
