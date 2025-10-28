
using Microsoft.EntityFrameworkCore;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Infrastructure.Data.Context
{
    public class EcommerceDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<CarrinhoItem> CarrinhosItens { get; set; }

        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    "server=localhost;user id=root;password=1234;database=db_ecommerce",
                    ServerVersion.Parse("8.0.32-mysql")
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            // Chaves primárias
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Cliente>().HasKey(c => c.Id);
            modelBuilder.Entity<Fornecedor>().HasKey(f => f.Id);
            modelBuilder.Entity<Produto>().HasKey(p => p.Id);
            modelBuilder.Entity<Estoque>().HasKey(e => e.Id);
            modelBuilder.Entity<Venda>().HasKey(v => v.Id);
            modelBuilder.Entity<Carrinho>().HasKey(c => c.Id);
            modelBuilder.Entity<CarrinhoItem>().HasKey(i => i.Id);

            // Relacionamentos
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.User)
                .WithMany() 
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Fornecedor>()
               .HasOne(f => f.User)
               .WithMany() 
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Vendas)
                .HasForeignKey(v => v.ClienteId);

            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Produto)
                .WithMany(p => p.Vendas)
                .HasForeignKey(v => v.ProdutoId);

            modelBuilder.Entity<CarrinhoItem>()
                .HasOne(i => i.Carrinho)
                .WithMany(c => c.Itens)
                .HasForeignKey(i => i.CarrinhoId);

            // Valores padrão
            modelBuilder.Entity<Carrinho>()
                .Property(c => c.Exibir)
                .HasDefaultValue("SIM");

            base.OnModelCreating(modelBuilder);
        }
    }
}
