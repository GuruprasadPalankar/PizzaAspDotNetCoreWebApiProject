using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PizzaAspDotNetCoreWebApiProject.Model;
using PizzaAspDotNetCoreWebApiProject.PizzaValidations;

namespace PizzaSpecialTest
{
    [TestClass]
    public class ValidationTest
    {
        [TestMethod]
        public async Task Validating_IfValueIsNull_ReturnsTrue()
        {
            //Arrange
            var obj = new Validations();
            var expectedOutput = true;

            //Act
            var actualOutput = await obj.Validating((PizzaModel)null);

            //Assert
            actualOutput.Should().Be(expectedOutput);
        }


        [TestMethod]
        public async Task Validating_IfValueIsNotNull_ReturnsFalse()
        {
            //Arrange
            var obj = new Validations();
            var expectedOutput = false;
            var pizza = new PizzaModel() { Id = 1, BasePrice = (decimal)11.23, Description = "rty", Name = "rty" };

            //Act
            var actualOutput = await obj.Validating(pizza);

            //Assert
            actualOutput.Should().Be(expectedOutput);
        }


        [TestMethod]
        public async Task PostValidation_IfNonOfTheValueIsNull_ReturnsTrueandEmptyString()
        {
            //Arrange
            var obj = new Validations();
            var pizza = new PizzaModelForPost() { BasePrice = (decimal)11.23, Description = "rty", Name = "rty" };
            var expectedOutput = new Tuple<bool, string>(true, "");

            //Act
            var actualOutput = await obj.PostValidation(pizza);

            //Assert
            actualOutput.Item1.Should().Be(expectedOutput.Item1);
            actualOutput.Item2.Should().Be(expectedOutput.Item2);
        }


        [TestMethod]
        [DataRow(null, "11.22", "xyz", " Invalid Name")]
        [DataRow("abc", null, "xyz", " Invalid BasePrice")]
        [DataRow("abc", "11.22", null, " Invalid Description")]
        [DataRow(null, null, "xyz", " Invalid Name Invalid BasePrice")]
        [DataRow(null, "11.22", null, " Invalid Name Invalid Description")]
        [DataRow("abc", null, null, " Invalid BasePrice Invalid Description")]
        [DataRow(null, null, null, " Invalid Name Invalid BasePrice Invalid Description")]
        public async Task PostValidation_IfAtleastOneOfTheValueIsNull_ReturnsFalseandErrorString(string name, string basePrice, string description, string errorMessage)
        {
            //Arrange
            decimal? basePriceFormated = null;
            if (basePrice != null)
            {
                basePriceFormated = Decimal.Parse(basePrice);
            }
            var obj = new Validations();
            var pizza = new PizzaModelForPost() { Name = name, BasePrice = basePriceFormated, Description = description };
            var expectedOutput = new Tuple<bool, string>(false, errorMessage);

            //Act
            var actualOutput = await obj.PostValidation(pizza);

            //Assert
            actualOutput.Item1.Should().Be(expectedOutput.Item1);
            actualOutput.Item2.Should().Be(expectedOutput.Item2);
        }
    }
}
