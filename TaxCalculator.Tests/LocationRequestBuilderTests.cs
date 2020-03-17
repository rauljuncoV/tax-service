using System;
using TaxCalculator.Services.Artifacs.Helpers;
using TaxCalculator.Services.Models;
using Xunit;
using TaxCalculator.Services.Artifacs;

namespace TaxCalculator.Tests
{
    public class LocationRequestBuilderTests
    {
        [Fact]
        public void GetQueryStringParams_EmptyLocation_ShouldFaild()
        {
            var location = new Location();

            var locationRequestBuilder = new LocationRequestBuilder(location);

            Assert.Throws<ArgumentException>(() => locationRequestBuilder.GetQueryStringParams());
        }

        [Fact]
        public void GetQueryStringParams_EmptyZipLocation_ShouldFaild()
        {
            var location = new Location { ZIP = "", Street = "312 Hurricane Lane", City = "Williston", State = "CA", Country = Constants.COUNTRY_US };

            var locationRequestBuilder = new LocationRequestBuilder(location);

            Action act = () => locationRequestBuilder.GetQueryStringParams();

            var exception = Assert.Throws<ArgumentException>(act);

            Assert.Equal("The Zip Code is required.", exception.Message);
        }

        [Theory]
        [InlineData("05495-2086", "312 Hurricane Lane", "", "", "", "05495-2086?street=312+Hurricane+Lane")]
        [InlineData("05495-2086", "312 Hurricane Lane", "Williston", "", "", "05495-2086?street=312+Hurricane+Lane&city=Williston")]
        [InlineData("05495-2086", "312 Hurricane Lane", "Williston", "CA", "", "05495-2086?street=312+Hurricane+Lane&city=Williston&state=CA")]
        [InlineData("05495-2086", "312 Hurricane Lane", "Williston", "CA", Constants.COUNTRY_US, "05495-2086?street=312+Hurricane+Lane&city=Williston&state=CA&country=US")]
        public void GetQueryStringParams_ValidLocation_ShouldWork(string zip, string street, string city, string state, string country, string expected)
        {
            var location = new Location { ZIP = zip, Street = street, City = city, State = state, Country = country };

            var locationRequestBuilder = new LocationRequestBuilder(location);

            var queryString = locationRequestBuilder.GetQueryStringParams();

            Assert.Equal(expected, queryString);
        }
    }
}
