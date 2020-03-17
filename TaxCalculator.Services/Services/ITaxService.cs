using System.Threading.Tasks;
using TaxCalculator.Services.Models;

namespace TaxCalculator.Services
{
    public interface ITaxService
    {
        Task<RatesResponse> GetTaxRateByLocation(Location location);
        Task<OrderTaxes> CalculateOrderTaxes(Order order);
    }
}
