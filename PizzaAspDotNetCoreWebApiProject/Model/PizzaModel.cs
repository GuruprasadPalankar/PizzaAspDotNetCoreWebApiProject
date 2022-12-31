using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaAspDotNetCoreWebApiProject.Model
{
    public class PizzaModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal? BasePrice { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}