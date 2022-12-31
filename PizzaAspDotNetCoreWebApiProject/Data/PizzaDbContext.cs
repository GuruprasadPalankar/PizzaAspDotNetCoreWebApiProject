using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaAspDotNetCoreWebApiProject.Model;

namespace PizzaAspDotNetCoreWebApiProject.Data
{
    public class PizzaDbContext : DbContext, IPizzaDbContext
    {
        public PizzaDbContext(DbContextOptions<PizzaDbContext> options)
            :base(options)
        {

        }
        public DbSet<PizzaModel> PizzaDbTable { get; set; }
    }
}
