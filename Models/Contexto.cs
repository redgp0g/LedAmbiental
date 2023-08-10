using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LedAmbiental.Models
{
    public class Contexto : DbContext
    {

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }
        public DbSet<Movimentacao> Movimentacao { get; set; }
    }
}