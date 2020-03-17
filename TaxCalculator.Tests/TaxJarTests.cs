using Xunit;
using TaxCalculator.Services.TaxCalculators;
using TaxCalculator.Services.Models;
using TaxCalculator.Services.Artifacs.Exceptions;
using System;
using System.Threading.Tasks;
using TaxCalculator.Services.Artifacs;

namespace TaxCalculator.Tests
{
    public class TaxJarTests
    {
        [Fact]
        public async void GetTaxRateByLocation_InvalidURL_ShouldFail()
        {
            var taxJarCalculator = new TaxJar("non valid", Initializer.API_TOKEN);

            var location = Initializer.GetValidLocation();

            await Assert.ThrowsAnyAsync<TaxServiceCalculatorException>(() => taxJarCalculator.GetTaxRateByLocation(location));
        }

        [Fact]
        public async void GetTaxRateByLocation_InvalidAPIToken_ShouldFail()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, "test");

            var location = Initializer.GetValidLocation();

            await Assert.ThrowsAnyAsync<TaxServiceCalculatorException>(() => taxJarCalculator.GetTaxRateByLocation(location));
        }

        [Fact]
        public async void GetTaxRateByLocation_InvalidLocation_ShouldFail()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            var location = new Location { ZIP = "0", Street = "", City = "", State = "", Country = "" };

            await Assert.ThrowsAnyAsync<TaxServiceCalculatorException>(() => taxJarCalculator.GetTaxRateByLocation(location));
        }

        [Fact]
        public async void GetTaxRateByLocation_NullLocation_ShouldFail()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            Func<Task> result = () => taxJarCalculator.GetTaxRateByLocation(null);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(result);

            Assert.Equal("Location", exception.ParamName);
        }

        [Fact]
        public async void CalculateOrderTaxes_NullOrder_ShouldFail()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            Func<Task> result = () => taxJarCalculator.CalculateOrderTaxes(null);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(result);

            Assert.Equal("Order", exception.ParamName);
        }

        [Fact]
        public async void CalculateOrderTaxes_EmptyToCountry_ShouldFail()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            var order = new Order();

            Func<Task> result = () => taxJarCalculator.CalculateOrderTaxes(order);

            var exception = await Assert.ThrowsAsync<ArgumentException>(result);

            Assert.Equal("The ToCountry is required.", exception.Message);
        }

        [Fact]
        public async void CalculateOrderTaxes_InvalidShipping_ShouldFail()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            var order = new Order { ToCountry = Constants.COUNTRY_US };

            Func<Task> result = () => taxJarCalculator.CalculateOrderTaxes(order);

            var exception = await Assert.ThrowsAsync<ArgumentException>(result);

            Assert.Equal("Invalid Shipping value.", exception.Message);
        }

        [Fact]
        public async void CalculateOrderTaxes_InvalidToZipWhenToCountryUS_ShouldFail()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            var order = Initializer.GetNYOrderWithItems();
            order.ToZip = string.Empty;

            Func<Task> result = () => taxJarCalculator.CalculateOrderTaxes(order);

            var exception = await Assert.ThrowsAsync<ArgumentException>(result);

            Assert.Equal("The ToZip parameter is required when ToCountry is 'US'.", exception.Message);
        }

        [Fact]
        public async void CalculateOrderTaxes_OrderWithNoItems_ShouldFail()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            var order = Initializer.GetOrder();

            Func<Task> result = () => taxJarCalculator.CalculateOrderTaxes(order);

            var exception = await Assert.ThrowsAsync<ArgumentException>(result);

            Assert.Equal("Either amount or Line Items parameters are required to perform tax calculations.", exception.Message);
        }

        //TODO: create a Fake Calculator
    }
}
