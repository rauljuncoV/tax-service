using TaxCalculator.Services.Models;
using TaxCalculator.Services.Artifacs;

namespace TaxCalculator.Tests
{
    public static class Initializer
    {
        public const string API_TOKEN = "db80e8995ea330f4b309074ccbd8f50b";
        public const string API_URL = "https://api.taxjar.com/v2";

        public static Location GetValidLocation()
        {
            return new Location { ZIP = "05495-2086", Street = "312 Hurricane Lane", City = "Williston", State = "CA", Country = Constants.COUNTRY_US };
        }
        public static Order GetOrder()
        {
            return new Order
            {
                FromCountry = Constants.COUNTRY_US,
                FromZip = "10118",
                FromState = "NY",
                FromCity = "New York",
                FromStreet = "350 5th Avenue",

                ToCountry = Constants.COUNTRY_US,
                ToZip = "10541",
                ToState = "NY",
                ToCity = "Mahopac",
                ToStreet = "668 Route Six",

                Amount = (float)19.95,
                Shipping = 10
            };
        }
        public static Order GetNYOrderWithItems()
        {
            var order = GetOrder();

            order.LineItems.Add(new OderItem
            {
                Id = "1",
                Quantity = 1,
                ProductTaxCode = "20010",
                UnitPrice = 15,
                Discount = 0
            });

            return order;
        }

    }
}
