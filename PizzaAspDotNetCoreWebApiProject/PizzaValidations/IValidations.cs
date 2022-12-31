using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaAspDotNetCoreWebApiProject.Model;

namespace PizzaAspDotNetCoreWebApiProject.PizzaValidations
{
    public interface IValidations
    {
        Task<bool> Validating(PizzaModel record);
        Task<Tuple<bool, string>> PostValidation(PizzaModelForPost model);
    }
}
