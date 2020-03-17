using System;
using TaxCalculator.Services.Models;

namespace TaxCalculator.Services.Artifacs.Helpers
{
    public class LocationRequestBuilder
    {
        private Location _location;
        public LocationRequestBuilder(Location location)
        {
            _location = location;
        }

        public string GetQueryStringParams()
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (string.IsNullOrWhiteSpace(_location.ZIP))
            {
                throw new ArgumentException("The Zip Code is required.");
            }

            if (!string.IsNullOrWhiteSpace(_location.Street))
            {
                queryParams.Add("street", _location.Street);
            }

            if (!string.IsNullOrWhiteSpace(_location.City))
            {
                queryParams.Add("city", _location.City);
            }

            if (!string.IsNullOrWhiteSpace(_location.State))
            {
                queryParams.Add("state", _location.State);
            }

            if (!string.IsNullOrWhiteSpace(_location.Country))
            {
                queryParams.Add("country", _location.Country);
            }

            return $"{_location.ZIP}?{queryParams.ToString()}";
        }
    }
}
