using TaxCalculator.Services;
using TaxCalculator.Services.TaxCalculators;
using Xunit;

namespace TaxCalculator.Tests
{
    public class TaxServiceTests
    {

        [Fact]
        public async void CalculateOrderTaxes_FakeCalculator_ShouldWork()
        {
            var stubCalculator = new FakeCalculator();

            var order = Initializer.GetNYOrderWithItems();

            var taxService = new TaxService(stubCalculator);

            var orderTaxes = await taxService.CalculateOrderTaxes(order);

            Assert.Equal(order.Shipping, orderTaxes.Tax.Shipping);
            Assert.Equal(order.Amount, orderTaxes.Tax.OrderTotalAmount);
            Assert.Equal(0, orderTaxes.Tax.TaxableAmount);
            Assert.Equal(0, orderTaxes.Tax.AmountToCollect);
            Assert.False(orderTaxes.Tax.FreightTaxable);
            Assert.Equal(string.Empty, orderTaxes.Tax.TaxSource);
        }
        [Fact]
        public async void GetTaxRateByLocation_FakeCalculator_ShouldWork()
        {
            var stubCalculator = new FakeCalculator();

            var location = Initializer.GetValidLocation();

            var taxService = new TaxService(stubCalculator);

            var rates = await taxService.GetTaxRateByLocation(location);

            Assert.Equal(location.City, rates.Rate.City);
            Assert.Equal(0, rates.Rate.CityRate);
            Assert.Equal(0, rates.Rate.CombinedDistrictRate);
            Assert.Equal(0, rates.Rate.CombinedRate);
            Assert.Equal(location.Country, rates.Rate.Country);
            Assert.Equal(0, rates.Rate.CountryRate);
            Assert.Equal(0, rates.Rate.CountyRate);
            Assert.False(rates.Rate.FreightTaxable);
            Assert.Equal(location.State, rates.Rate.State);
            Assert.Equal(0, rates.Rate.StateRate);
            Assert.Equal(location.ZIP, rates.Rate.Zip);
        }

        [Fact]
        public async void CalculateOrderTaxes_ValidOrder_ShouldReturnSameShippingAmount()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            var order = Initializer.GetNYOrderWithItems();

            var taxService = new TaxService(taxJarCalculator);

            var orderTaxes = await taxService.CalculateOrderTaxes(order);

            Assert.Equal(order.Shipping, orderTaxes.Tax.Shipping);
        }

        [Fact]
        public async void CalculateOrderTaxes_ValidOrder_NYRateShouldBeInRange()
        {
            var expectedMinimunLocationRate = 0;
            var expectedMaximunLocationRate = 0.1;

            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            var order = Initializer.GetNYOrderWithItems();

            var taxService = new TaxService(taxJarCalculator);

            var orderTaxes = await taxService.CalculateOrderTaxes(order);

            Assert.InRange(orderTaxes.Tax.Rate, expectedMinimunLocationRate, expectedMaximunLocationRate);
        }

        [Fact]
        public async void GetTaxRateByLocation_LocationState_ShouldBeTheSame()
        {
            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            var location = Initializer.GetValidLocation();

            var taxService = new TaxService(taxJarCalculator);

            var rates = await taxService.GetTaxRateByLocation(location);

            Assert.Equal(location.State, rates.Rate.State);
        }

        [Fact]
        public async void CalculateOrderTaxes_CALocation_CAStateRateShouldBeInRange()
        {
            var expectedMinimunStateRate = 0;
            var expectedMaximunStateRate = 0.1;

            var taxJarCalculator = new TaxJar(Initializer.API_URL, Initializer.API_TOKEN);

            var location = Initializer.GetValidLocation();

            var taxService = new TaxService(taxJarCalculator);

            var rates = await taxService.GetTaxRateByLocation(location);

            Assert.InRange(rates.Rate.StateRate, expectedMinimunStateRate, expectedMaximunStateRate);
        }

    }
}
