using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAspDotNetCoreWebApiProject.Model
{
    public class PizzaModelForPost
    {
        public string Name { get; set; }

        public decimal? BasePrice { get; set; }

        public string Description { get; set; }
    }
}
