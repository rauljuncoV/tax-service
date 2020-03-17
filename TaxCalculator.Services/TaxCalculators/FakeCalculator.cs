using System;
using System.Threading.Tasks;
using TaxCalculator.Services.Models;

namespace TaxCalculator.Services.TaxCalculators
{
    public class FakeCalculator : ITaxCalculator
    {
        public FakeCalculator()
        {
        }

        public Task<OrderTaxes> CalculateOrderTaxes(Order order)
        {
            return Task.FromResult(new OrderTaxes
            {
                Tax = new Tax
                {
                    Shipping = order.Shipping,
                    OrderTotalAmount = order.Amount,
                    TaxableAmount = 0,
                    AmountToCollect = 0,
                    FreightTaxable = false,
                    TaxSource = string.Empty
                }
            });
        }

        public Task<RatesResponse> GetTaxRateByLocation(Location location)
        {
            return Task.FromResult(new RatesResponse
            {
                Rate = new Rate
                {
                    City = location.City,
                    CityRate = 0,
                    CombinedDistrictRate = 0,
                    CombinedRate = 0,
                    Country = location.Country,
                    CountryRate = 0,
                    FreightTaxable = false,
                    State = location.State,
                    StateRate = 0,
                    Zip = location.ZIP
                }
            });
        }
    }
}
