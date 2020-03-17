using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TaxCalculator.Services.Artifacs.Helpers;
using TaxCalculator.Services.Models;
using TaxCalculator.Services.Artifacs.Extensions;
using TaxCalculator.Services.Artifacs.Exceptions;
using TaxCalculator.Services.Artifacs;

namespace TaxCalculator.Services.TaxCalculators
{
    public class TaxJar : ITaxCalculator
    {
        private readonly string _apiToken;
        private readonly string _apiURL;
        public TaxJar(string apiURL, string apiToken)
        {
            _apiToken = apiToken;
            _apiURL = apiURL;
        }

        private string GetRatesURL()
        {
            return $"{_apiURL}/rates/";
        }

        private string GetTaxesURL()
        {
            return $"{_apiURL}/taxes/";
        }

        private HttpClient InitializeHttpClient()
        {
            if (string.IsNullOrEmpty(_apiURL))
            {
                throw new Exception("Invalid API URL");
            }

            if (string.IsNullOrEmpty(_apiToken))
            {
                throw new Exception("Invalid Autentication Token");
            }

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

            return httpClient;
        }

        public async Task<RatesResponse> GetTaxRateByLocation(Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("Location");
            }

            var queryString = new LocationRequestBuilder(location).GetQueryStringParams();

            var url = $"{GetRatesURL()}{queryString}";

            HttpClient httpClient = InitializeHttpClient();

            HttpResponseMessage streamResponse = null;

            try
            {
                streamResponse = await httpClient.GetAsync(url);

                streamResponse.EnsureSuccessStatusCode();

                string responseContent = await streamResponse.Content.ReadAsStringAsync();

                return responseContent.DeserializeAs<RatesResponse>();
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                if (streamResponse != null)
                {
                    string responseContent = await streamResponse.Content.ReadAsStringAsync();

                    var errorResponse = responseContent.DeserializeAs<TaxJarErrorResponse>();

                    message = $"Error: {errorResponse.Error}. Details: {errorResponse.Detail}.";
                }

                throw new TaxServiceCalculatorException($"Error calculating rates. {message}");
            }
        }

        private void ValidateOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("Order");
            }

            if (string.IsNullOrWhiteSpace(order.ToCountry))
            {
                throw new ArgumentException("The ToCountry is required.");
            }

            if (order.Shipping <= 0)
            {
                throw new ArgumentException("Invalid Shipping value.");
            }

            if (order.LineItems.Count == 0)
            {
                throw new ArgumentException("Either amount or Line Items parameters are required to perform tax calculations.");
            }

            if (order.ToCountry.Equals(Constants.COUNTRY_US) && string.IsNullOrWhiteSpace(order.ToZip))
            {
                throw new ArgumentException("The ToZip parameter is required when ToCountry is 'US'.");
            }

            //TODO: possible improvement on validations - The to_state parameter is required when to_country is 'US’ or 'CA’.
        }

        public async Task<OrderTaxes> CalculateOrderTaxes(Order order)
        {
            ValidateOrder(order);

            var httpClient = InitializeHttpClient();

            HttpResponseMessage streamResponse = null;

            try
            {
                var orderRequest = order.Serialize<Order>();

                streamResponse = await httpClient.PostAsync(GetTaxesURL(), orderRequest);

                streamResponse.EnsureSuccessStatusCode();

                string responseContent = await streamResponse.Content.ReadAsStringAsync();

                return responseContent.DeserializeAs<OrderTaxes>();
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                if (streamResponse != null)
                {
                    string responseContent = await streamResponse.Content.ReadAsStringAsync();

                    var errorResponse = responseContent.DeserializeAs<TaxJarErrorResponse>();

                    message = $"Error: {errorResponse.Error}. Details: {errorResponse.Detail}.";
                }

                throw new TaxServiceCalculatorException($"Error calculating rates. {message}");
            }
        }
    }
}
