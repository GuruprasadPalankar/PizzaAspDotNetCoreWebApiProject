using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaAspDotNetCoreWebApiProject.Data;
using PizzaAspDotNetCoreWebApiProject.PizzaValidations;
using PizzaAspDotNetCoreWebApiProject.Model;

namespace PizzaAspDotNetCoreWebApiProject.PizzaRepository
{
    public class PizzaRepositoryLogic : IPizzaRepositoryLogic
    {
        private readonly IValidations _validations;
        private readonly IPizzaDbContext _pizzaDbContext;

        public PizzaRepositoryLogic(IValidations validations, IPizzaDbContext pizzaDbContext)
        {
            this._validations = validations;
            this._pizzaDbContext = pizzaDbContext;
        }

        public async Task<IList<PizzaModel>> GetAllPizzas()
        {
            var pizzas = new List<PizzaModel>();
            await Task.Run(() => { 
                pizzas = this._pizzaDbContext.PizzaDbTable.ToList();
            });
            return pizzas;
        }

        public async Task<PizzaModel> GetPizza(int id)
        {
            var record = this._pizzaDbContext.PizzaDbTable.Find(id);
            return await this._validations.Validating(record) ? null : record;
        }

        public async Task<Tuple<PizzaModel, string>> AddPizza(PizzaModelForPost pizza)
        {
            var tup = await this._validations.PostValidation(pizza);

                if (tup.Item1)
                {
                    var record = new PizzaModel()
                    {
                        Name = pizza.Name,
                        BasePrice = pizza.BasePrice,
                        Description = pizza.Description
                    };

                    await this._pizzaDbContext.PizzaDbTable.AddAsync(record);
                    this._pizzaDbContext.SaveChanges();
                    return Tuple.Create(record, tup.Item2);
                    //return new Tuple<PizzaModel, string>(record, tup.Item2);
                }

            return Tuple.Create((PizzaModel)null, tup.Item2);
            //return new Tuple<PizzaModel, string>(null, tup.Item2);
        }

        public async Task<PizzaModel> UpdatePizza(PizzaModel pizza)
        {
            var record = this._pizzaDbContext.PizzaDbTable.Find(pizza.Id);
            if (! await this._validations.Validating(record))
            {
                record.Name = pizza.Name;
                record.BasePrice = pizza.BasePrice;
                record.Description = pizza.Description;
                this._pizzaDbContext.SaveChanges();
                return record;
            }
            return null;
        }

        public async Task<PizzaModel> DeletePizza(int id)
        {
            var record = this._pizzaDbContext.PizzaDbTable.Find(id);
            if (! await this ._validations.Validating(record))
            {
                this._pizzaDbContext.PizzaDbTable.Remove(record);
                this._pizzaDbContext.SaveChanges();
                return record;
            }
            return null;
        }
    }
}
