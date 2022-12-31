using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PizzaAspDotNetCoreWebApiProject.Data;
using PizzaAspDotNetCoreWebApiProject.Model;
using PizzaAspDotNetCoreWebApiProject.PizzaRepository;
using PizzaAspDotNetCoreWebApiProject.PizzaValidations;

namespace PizzaSpecialTest
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public async Task GetAllPizzas_IfValueIsValid_ReturnsListOfPizzas()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "PizzaDb")
                .Options;

            var context = new PizzaDbContext(options);
            
            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" }
                );

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" }
                );

            context.SaveChanges();

            Mock<IValidations> validation = new Mock<IValidations>();
            var repo = new PizzaRepositoryLogic(validation.Object, context);

            //Act
            var actualOutput = await repo.GetAllPizzas();

            //Assert
            actualOutput.Count.Should().Be(2);
        }


        [TestMethod]
        public async Task GetPizza_IfIdIsValid_ReturnsPizza()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "PizzaDb1")
                .Options;

            var context = new PizzaDbContext(options);

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" }
                );

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" }
                );

            context.SaveChanges();

            var expectedOutput = new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" };
            
            Mock<IValidations> validation = new Mock<IValidations>();
            validation.Setup(x => x.Validating(It.IsAny<PizzaModel>())).ReturnsAsync(false);
            var repo = new PizzaRepositoryLogic(validation.Object, context);

            //Act
            var actualOutput = await repo.GetPizza(2);

            //Assert
            actualOutput.Id.Should().Be(expectedOutput.Id);
            actualOutput.Name.Should().Be(expectedOutput.Name);
            actualOutput.BasePrice.Should().Be(expectedOutput.BasePrice);
            actualOutput.Description.Should().Be(expectedOutput.Description);
        }


        [TestMethod]
        public async Task GetPizza_IfIdIsNotValid_ReturnsNull()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "PizzaDb2")
                .Options;

            var context = new PizzaDbContext(options);

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" }
                );

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" }
                );

            context.SaveChanges();

            var expectedOutput = (PizzaModel)null;

            Mock<IValidations> validation = new Mock<IValidations>();
            validation.Setup(x => x.Validating(It.IsAny<PizzaModel>())).ReturnsAsync(true);
            var repo = new PizzaRepositoryLogic(validation.Object, context);

            //Act
            var actualOutput = await repo.GetPizza(2);

            //Assert
            actualOutput.Should().Be(expectedOutput);
        }


        [TestMethod]
        public async Task AddPizza_IfNonOfTheValueIsNull_ReturnsPizzaAndEmptyString()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "PizzaDb3")
                .Options;

            var context = new PizzaDbContext(options);

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" }
                );

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" }
                );

            context.SaveChanges();

            var pizza = new PizzaModelForPost() { Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" };
            var expectedOutput = new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" };
            var tuple = new Tuple<bool, string>(true, "");

            Mock<IValidations> validation = new Mock<IValidations>();
            validation.Setup(x => x.PostValidation(It.IsAny<PizzaModelForPost>())).ReturnsAsync(tuple);
            var repo = new PizzaRepositoryLogic(validation.Object, context);

            //Act
            var actualOutput = await repo.AddPizza(pizza);

            //Assert
            actualOutput.Item1.Name.Should().Be(expectedOutput.Name);
            actualOutput.Item1.BasePrice.Should().Be(expectedOutput.BasePrice);
            actualOutput.Item1.Description.Should().Be(expectedOutput.Description);
        }


        [TestMethod]
        [DataRow(null, "11.22", "xyz", " Invalid Name", "1")]
        [DataRow("abc", null, "xyz", " Invalid BasePrice", "2")]
        [DataRow("abc", "11.22", null, " Invalid Description", "3")]
        [DataRow(null, null, "xyz", " Invalid Name Invalid BasePrice", "4")]
        [DataRow(null, "11.22", null, " Invalid Name Invalid Description", "5")]
        [DataRow("abc", null, null, " Invalid BasePrice Invalid Description", "6")]
        [DataRow(null, null, null, " Invalid Name Invalid BasePrice Invalid Description", "7")]
        public async Task AddPizza_IfAtleastOneOfTheValueIsNull_ReturnsNullAndErrorString(string name, string basePrice, string description, string errorMessage, string db)
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "PizzaDb4" + db)
                .Options;

            var context = new PizzaDbContext(options);

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" }
                );

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" }
                );

            context.SaveChanges();

            decimal? basePriceFormated = null;
            if (basePrice != null)
            {
                basePriceFormated = Decimal.Parse(basePrice);
            }
            var pizza = new PizzaModelForPost() { Name = name, BasePrice = basePriceFormated, Description = description };
            var expectedOutput = (PizzaModel)null;
            var tuple = new Tuple<bool, string>(false, errorMessage);

            Mock<IValidations> validation = new Mock<IValidations>();
            validation.Setup(x => x.PostValidation(It.IsAny<PizzaModelForPost>())).ReturnsAsync(tuple);
            var repo = new PizzaRepositoryLogic(validation.Object, context);

            //Act
            var actualOutput = await repo.AddPizza(pizza);

            //Assert
            actualOutput.Item1.Should().Be(expectedOutput);
            actualOutput.Item2.Should().Be(errorMessage);
        }


        [TestMethod]
        public async Task UpdatePizza_IfIdIsValid_ReturnsPizza()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "PizzaDb5")
                .Options;

            var context = new PizzaDbContext(options);

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" }
                );

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" }
                );

            context.SaveChanges();

            var expectedOutput = new PizzaModel() { Id = 2, Name = "pqr", BasePrice = (decimal)13.45, Description = "lmn" };

            Mock<IValidations> validation = new Mock<IValidations>();
            validation.Setup(x => x.Validating(It.IsAny<PizzaModel>())).ReturnsAsync(false);
            var repo = new PizzaRepositoryLogic(validation.Object, context);

            //Act
            var actualOutput = await repo.UpdatePizza(expectedOutput);

            //Assert
            actualOutput.Id.Should().Be(expectedOutput.Id);
            actualOutput.Name.Should().Be(expectedOutput.Name);
            actualOutput.BasePrice.Should().Be(expectedOutput.BasePrice);
            actualOutput.Description.Should().Be(expectedOutput.Description);
        }


        [TestMethod]
        public async Task UpdatePizza_IfIdIsNotValid_ReturnsNull()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "PizzaDb6")
                .Options;

            var context = new PizzaDbContext(options);

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" }
                );

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" }
                );

            context.SaveChanges();

            var expectedOutput = (PizzaModel)null;
            var pizza = new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" };

            Mock<IValidations> validation = new Mock<IValidations>();
            validation.Setup(x => x.Validating(It.IsAny<PizzaModel>())).ReturnsAsync(true);
            var repo = new PizzaRepositoryLogic(validation.Object, context);

            //Act
            var actualOutput = await repo.UpdatePizza(pizza);

            //Assert
            actualOutput.Should().Be(expectedOutput);
        }


        [TestMethod]
        public async Task DeletePizza_IfIdIsValid_ReturnsPizza()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "PizzaDb7")
                .Options;

            var context = new PizzaDbContext(options);

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" }
                );

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" }
                );

            context.SaveChanges();

            var expectedOutput = new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" };

            Mock<IValidations> validation = new Mock<IValidations>();
            validation.Setup(x => x.Validating(It.IsAny<PizzaModel>())).ReturnsAsync(false);
            var repo = new PizzaRepositoryLogic(validation.Object, context);

            //Act
            var actualOutput = await repo.DeletePizza(2);

            //Assert
            actualOutput.Id.Should().Be(expectedOutput.Id);
            actualOutput.Name.Should().Be(expectedOutput.Name);
            actualOutput.BasePrice.Should().Be(expectedOutput.BasePrice);
            actualOutput.Description.Should().Be(expectedOutput.Description);
        }


        [TestMethod]
        public async Task DeletePizza_IfIdIsNotValid_ReturnsNull()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "PizzaDb8")
                .Options;

            var context = new PizzaDbContext(options);

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 1, Name = "xyz", BasePrice = (decimal)11.23, Description = "tyu" }
                );

            context.PizzaDbTable.Add
                (
                    new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" }
                );

            context.SaveChanges();

            var expectedOutput = (PizzaModel)null;
            var pizza = new PizzaModel() { Id = 2, Name = "abc", BasePrice = (decimal)12.23, Description = "pou" };

            Mock<IValidations> validation = new Mock<IValidations>();
            validation.Setup(x => x.Validating(It.IsAny<PizzaModel>())).ReturnsAsync(true);
            var repo = new PizzaRepositoryLogic(validation.Object, context);

            //Act
            var actualOutput = await repo.DeletePizza(2);

            //Assert
            actualOutput.Should().Be(expectedOutput);
        }
    }
}