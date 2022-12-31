using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaAspDotNetCoreWebApiProject.Model;

namespace PizzaAspDotNetCoreWebApiProject.PizzaValidations
{
    public class Validations : IValidations
    {
        public async Task<bool> Validating(PizzaModel record)
        {
            var boolean = false;
            await Task.Run(() =>
            { 
                if (record == null)
                {
                    boolean = true;
                }
            });

            return boolean;
        }

        public async Task<Tuple<bool, string>> PostValidation(PizzaModelForPost model)
        {
            var errormessage = "";
            var boolean = true;

            await Task.Run(() =>
            {
                if (model.Name == null)
                {
                    errormessage += " Invalid Name";
                    boolean = false;
                }
                if (model.BasePrice == null)
                {
                    errormessage += " Invalid BasePrice";
                    boolean = false;
                }
                if (model.Description == null)
                {
                    errormessage += " Invalid Description";
                    boolean = false;
                }
            });
            
            return Tuple.Create(boolean, errormessage);
            //return new Tuple<bool, string>(boolean, errormessage);
        }
    }
}
