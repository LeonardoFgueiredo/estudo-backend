using MeuTodo.Models;
using Microsoft.EntityFrameworkCore;

// Data context é a representação do banco. classe X será tabela Y, etc.

namespace MeuTodo.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite(connectionString:"DataSource=app.db;Cache=Shared");
    }
}