using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaAspDotNetCoreWebApiProject.Model;

namespace PizzaAspDotNetCoreWebApiProject.PizzaRepository
{
    public interface IPizzaRepositoryLogic
    {
        Task<IList<PizzaModel>> GetAllPizzas();

        Task<PizzaModel> GetPizza(int id);

        Task<Tuple<PizzaModel, string>> AddPizza(PizzaModelForPost pizza);

        Task<PizzaModel> UpdatePizza(PizzaModel pizza);

        Task<PizzaModel> DeletePizza(int id);
    }
}
