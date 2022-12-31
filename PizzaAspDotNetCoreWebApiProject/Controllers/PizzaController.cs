using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaAspDotNetCoreWebApiProject.Data;
using PizzaAspDotNetCoreWebApiProject.PizzaRepository;
using PizzaAspDotNetCoreWebApiProject.Model;
using PizzaAspDotNetCoreWebApiProject.NLog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzaAspDotNetCoreWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaRepositoryLogic _repo;
        private readonly IPizzaNLog _logger;

        public PizzaController(IPizzaRepositoryLogic repo, IPizzaNLog logger)
        {
            this._repo = repo;
            this._logger = logger;
        }

        // GET: api/<PizzaController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this._repo.GetAllPizzas();
            this._logger.Information("All pizzas retrieved successfully");
            return Ok(result);
        }

        // GET api/<PizzaController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await this._repo.GetPizza(id);

            try
            {
                if (result == null)
                {
                    throw new Exception("Invalid Id");
                }
                this._logger.Information("Requested one pizza retrieved successfully");
                return Ok(result);
            }
            catch (Exception e)
            {
                this._logger.Error("Can't retrieve requested pizza due to " + e.Message);
                return NotFound(e.Message);
            }
        }

        // POST api/<PizzaController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PizzaModelForPost pizza)
        {
            var result = await this._repo.AddPizza(pizza);
            try
            {
                if(result.Item1 == null)
                {
                    throw new Exception(result.Item2);
                }
                this._logger.Information("Pizza added successfully");
                return Ok(result.Item1);
            }
            catch (Exception e)
            {
                this._logger.Error("Can't add pizza due to" + e.Message);
                return NotFound(e.Message);
            }
        }

        // PUT api/<PizzaController>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PizzaModel pizza)
        {
            var result = await this._repo.UpdatePizza(pizza);
            try
            {
                if (result == null)
                {
                    throw new Exception("Invalid Id");
                }
                this._logger.Information("Requested one pizza updated successfully");
                return Ok(result);
            }
            catch(Exception e)
            {
                this._logger.Error("Can't update requested pizza due to " + e.Message);
                return NotFound(e.Message);
            }
        }

        // DELETE api/<PizzaController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this._repo.DeletePizza(id);

            try
            {
                if (result == null)
                {
                    throw new Exception("Invalid Id");
                }
                this._logger.Information("Pizza deleted successfully");
                return Ok(result);
            }
            catch (Exception e)
            {
                this._logger.Error("Can't delete pizza due to " + e.Message);
                return NotFound(e.Message);
            }
        }
    }
}
