using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaAspDotNetCoreWebApiProject.Model;

namespace PizzaAspDotNetCoreWebApiProject.Data
{
    public interface IPizzaDbContext
    {
        DbSet<PizzaModel> PizzaDbTable { get; set; }

        int SaveChanges();
    }
}
