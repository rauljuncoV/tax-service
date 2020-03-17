using System.Threading.Tasks;
using TaxCalculator.Services.Models;

namespace TaxCalculator.Services.TaxCalculators
{
    public interface ITaxCalculator
    {
        Task<RatesResponse> GetTaxRateByLocation(Location location);
        Task<OrderTaxes> CalculateOrderTaxes(Order order);
    }
}
