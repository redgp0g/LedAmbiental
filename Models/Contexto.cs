using LedAmbiental.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CadastroFalhas.Models
{
    public class Contexto : DbContext
    {

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }
        public DbSet<Entrada> Entrada{ get; set; }
        public DbSet<Material> Material { get; set; }
    }
}