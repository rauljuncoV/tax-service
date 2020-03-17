using System.Threading.Tasks;
using TaxCalculator.Services.Models;
using TaxCalculator.Services.TaxCalculators;

namespace TaxCalculator.Services
{
    public class TaxService : ITaxService
    {
        private readonly ITaxCalculator _taxCalculator;

        public TaxService(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        public Task<OrderTaxes> CalculateOrderTaxes(Order order)
        {
            return _taxCalculator.CalculateOrderTaxes(order);
        }

        public Task<RatesResponse> GetTaxRateByLocation(Location location)
        {
            return _taxCalculator.GetTaxRateByLocation(location);
        }

    }
}
