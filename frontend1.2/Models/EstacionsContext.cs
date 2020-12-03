using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication7.Models;

namespace AppMeteo.Models
{
    public class EstacionsContext : DbContext
    {

        public DbSet<Estacions> Estacio { get; set; }
        public DbSet<Ubicacio> Ubicacio { get; set; }

        public EstacionsContext(DbContextOptions<EstacionsContext> options)
            : base(options)
        {

        }


    }
}
