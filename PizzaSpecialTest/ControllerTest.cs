using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PizzaAspDotNetCoreWebApiProject.Controllers;
using PizzaAspDotNetCoreWebApiProject.Model;
using PizzaAspDotNetCoreWebApiProject.NLog;
using PizzaAspDotNetCoreWebApiProject.PizzaRepository;

namespace PizzaSpecialTest
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public async Task Get_IfValueIsValid_ReturnsListofPizza(int index) //Syntax for TestMethod --> MethodName_Condition_ReturnValue
        {
            //Arrange --> For intializing
            var expectedOutput = new List<PizzaModel>() 
            {
                new PizzaModel() {Id=1, BasePrice=(decimal)11.23, Description="rty", Name="rty"},
                new PizzaModel() {Id=2, BasePrice=(decimal)12.23, Description="xyz", Name="xyz"}
            };

            Mock<IPizzaRepositoryLogic> repo = new Mock<IPizzaRepositoryLogic>();
            Mock<IPizzaNLog> logger = new Mock<IPizzaNLog>();
            repo.Setup(x => x.GetAllPizzas()).ReturnsAsync(expectedOutput);

            var obj = new PizzaController(repo.Object, logger.Object);

            //Act --> For method calling
            var output = (OkObjectResult)await obj.Get();   //is-TypeCast --> gives compile time error and its used when we are sure that object can be typecasted as per what we think
                //var demo = obj.Get() as OkObjectResult;   //as-TypeCast --> gives run time error and its used when we are not sure that object can be typecasted as per what we think
            var actualOutput = (List<PizzaModel>)output.Value;

            //Assert --> For checking actual and expected output using FluentAssertions or normal builtin Testing functionalities
            Assert.AreEqual(expectedOutput.Count, actualOutput.Count);
            actualOutput[index].Id.Should().Be(expectedOutput[index].Id);
            actualOutput[index].Name.Should().Be(expectedOutput[index].Name);
            actualOutput[index].BasePrice.Should().Be(expectedOutput[index].BasePrice);
            actualOutput[index].Description.Should().Be(expectedOutput[index].Description);
        }
        

        [TestMethod]
        [DataRow(0)]
        [DataRow(7)]
        public async Task GetId_IfIdIsInvalid_ReturnsNotFound(int id)
        {
            //Arrange
            var expectedOutput = "Invalid Id";

            Mock<IPizzaRepositoryLogic> repo = new Mock<IPizzaRepositoryLogic>();
            Mock<IPizzaNLog> logger = new Mock<IPizzaNLog>();
            repo.Setup(x => x.GetPizza(id)).ReturnsAsync((PizzaModel)null);

            var obj = new PizzaController(repo.Object, logger.Object);

            //Act
            var output = (NotFoundObjectResult) await obj.Get(id);
            var actualOutput = output.Value;

            //Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }


        [TestMethod]
        [DataRow(0)]
        [DataRow(7)]
        public async Task GetId_IfIdIsValid_ReturnsOk(int id)
        {
            //Arrange
            var expectedOutput = new PizzaModel() { Id = 1, BasePrice = (decimal)11.23, Description = "rty", Name = "rty" };
            
            Mock<IPizzaRepositoryLogic> repo = new Mock<IPizzaRepositoryLogic>();
            Mock<IPizzaNLog> logger = new Mock<IPizzaNLog>();
            repo.Setup(x => x.GetPizza(id)).ReturnsAsync(expectedOutput);

            var obj = new PizzaController(repo.Object, logger.Object);

            //Act
            var output = (OkObjectResult) await obj.Get(id);
            var actualOutput = (PizzaModel)output.Value;

            //Assert
            actualOutput.Id.Should().Be(expectedOutput.Id);
            actualOutput.Name.Should().Be(expectedOutput.Name);
            actualOutput.BasePrice.Should().Be(expectedOutput.BasePrice);
            actualOutput.Description.Should().Be(expectedOutput.Description);
        }


        [TestMethod]
        [DataRow(null, "11.22", "xyz", " Invalid Name")]
        [DataRow("abc", null, "xyz", " Invalid BasePrice")]
        [DataRow("abc", "11.22", null, " Invalid Description")]
        [DataRow(null, null, "xyz", " Invalid Name Invalid BasePrice")]
        [DataRow(null, "11.22", null, " Invalid Name Invalid Description")]
        [DataRow("abc", null, null, " Invalid BasePrice Invalid Description")]
        [DataRow(null, null, null, " Invalid Name Invalid BasePrice Invalid Description")]
        public async Task Post_IfValueIsNotValid_ReturnsNotFound(string name, string basePrice, string description, string expectedOutput)
        {
            //Arrange
            decimal? basePriceFormated = null;
            if (basePrice != null)
            {
                basePriceFormated = Decimal.Parse(basePrice);
            }
            var pizza = new PizzaModelForPost() { BasePrice = basePriceFormated, Description = description, Name = name };
            var tuple = new Tuple<PizzaModel, string>(null, expectedOutput);
            
            Mock<IPizzaRepositoryLogic> repo = new Mock<IPizzaRepositoryLogic>();
            Mock<IPizzaNLog> logger = new Mock<IPizzaNLog>();
            repo.Setup(x => x.AddPizza(pizza)).ReturnsAsync(tuple);
            
            var obj = new PizzaController(repo.Object, logger.Object);

            //Act
            var output = (NotFoundObjectResult) await obj.Post(pizza);
            var actualOutput = output.Value;

            //Assert
            actualOutput.Should().Be(expectedOutput);
        }


        [TestMethod]
        public async Task Post_IfValueIsValid_ReturnsOk()
        {
            //Arrange
            var expectedOutput = new PizzaModel() { Id = 1, BasePrice = (decimal)11.23, Description = "rty", Name = "rty" };
            var pizza = new PizzaModelForPost() { BasePrice = (decimal)11.23, Description = "rty", Name = "rty" };
            var tuple = new Tuple<PizzaModel, string>(expectedOutput, "");
            
            Mock<IPizzaRepositoryLogic> repo = new Mock<IPizzaRepositoryLogic>();
            Mock<IPizzaNLog> logger = new Mock<IPizzaNLog>();
            repo.Setup(x => x.AddPizza(pizza)).ReturnsAsync(tuple);

            var obj = new PizzaController(repo.Object, logger.Object);

            //Act
            var output = (OkObjectResult) await obj.Post(pizza);
            var actualOutput = (PizzaModel)output.Value;

            //Assert
            actualOutput.Name.Should().Be(expectedOutput.Name);
            actualOutput.BasePrice.Should().Be(expectedOutput.BasePrice);
            actualOutput.Description.Should().Be(expectedOutput.Description);
        }


        [TestMethod]
        public async Task Put_IfValueIsInvalid_ReturnsNotFound()
        {
            //Arrange
            var expectedOutput = "Invalid Id";
            var pizza = new PizzaModel() { Id = 1, BasePrice = (decimal)11.23, Description = "rty", Name = "rty" };
            
            Mock<IPizzaRepositoryLogic> repo = new Mock<IPizzaRepositoryLogic>();
            Mock<IPizzaNLog> logger = new Mock<IPizzaNLog>();
            repo.Setup(x => x.UpdatePizza(pizza)).ReturnsAsync((PizzaModel)null);
            
            var obj = new PizzaController(repo.Object, logger.Object);

            //Act
            var output = (NotFoundObjectResult) await obj.Put(pizza);
            var actualOutput = output.Value;

            //Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }


        [TestMethod]
        public async Task Put_IfIdIsValid_ReturnsOk()
        {
            //Arrange
            var expectedOutput = new PizzaModel() { Id = 1, BasePrice = (decimal)11.23, Description = "rty", Name = "rty" };
            
            Mock<IPizzaRepositoryLogic> repo = new Mock<IPizzaRepositoryLogic>();
            Mock<IPizzaNLog> logger = new Mock<IPizzaNLog>();
            repo.Setup(x => x.UpdatePizza(expectedOutput)).ReturnsAsync(expectedOutput);
            
            var obj = new PizzaController(repo.Object, logger.Object);

            //Act
            var output = (OkObjectResult) await obj.Put(expectedOutput);
            var actualOutput = (PizzaModel)output.Value;

            //Assert
            actualOutput.Id.Should().Be(expectedOutput.Id);
            actualOutput.Name.Should().Be(expectedOutput.Name);
            actualOutput.BasePrice.Should().Be(expectedOutput.BasePrice);
            actualOutput.Description.Should().Be(expectedOutput.Description);
        }


        [TestMethod]
        [DataRow(0)]
        [DataRow(7)]
        public async Task Delete_IfIdIsInvalid_ReturnsNotFound(int id)
        {
            //Arrange
            var expectedOutput = "Invalid Id";
            
            Mock<IPizzaRepositoryLogic> repo = new Mock<IPizzaRepositoryLogic>();
            Mock<IPizzaNLog> logger = new Mock<IPizzaNLog>();
            repo.Setup(x => x.DeletePizza(id)).ReturnsAsync((PizzaModel)null);
            
            var obj = new PizzaController(repo.Object, logger.Object);

            //Act
            var output = (NotFoundObjectResult) await obj.Delete(id);
            var actualOutput = output.Value;

            //Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }


        [TestMethod]
        [DataRow(0)]
        [DataRow(7)]
        public async Task Delete_IfIdIsValid_ReturnsOk(int id)
        {
            //Arrange
            var expectedOutput = new PizzaModel() { Id = 1, BasePrice = (decimal)11.23, Description = "rty", Name = "rty" };
            
            Mock<IPizzaRepositoryLogic> repo = new Mock<IPizzaRepositoryLogic>();
            Mock<IPizzaNLog> logger = new Mock<IPizzaNLog>();
            repo.Setup(x => x.DeletePizza(id)).ReturnsAsync(expectedOutput);
            
            var obj = new PizzaController(repo.Object, logger.Object);

            //Act
            var output = (OkObjectResult) await obj.Delete(id);
            var actualOutput = (PizzaModel)output.Value;

            //Assert
            actualOutput.Id.Should().Be(expectedOutput.Id);
            actualOutput.Name.Should().Be(expectedOutput.Name);
            actualOutput.BasePrice.Should().Be(expectedOutput.BasePrice);
            actualOutput.Description.Should().Be(expectedOutput.Description);
        }
    }
}
