using Application.DTO;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Tests.IntegrationTests.Controllers.ProductsController
{
    public class UpdateProduct : ControllerTests
    {
        [Fact]
        public async Task UpdateProduct_Success_ShouldUpdateProduct()
        {
            var faker = new Faker();

            var addRequestBody = new AddProductDTO
            {
                Name = faker.Internet.UserName(),
                PricePerKilo = faker.Random.Decimal(1, 10),
                ExpirationTime = new TimeSpan(1, 0, 0, 0),
            };

            var request = new HttpRequestMessage(new HttpMethod("PUT"), "/api/products")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(addRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            var response1 = await _httpClient.SendAsync(request);

            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            var response2 = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("GET"), "/api/products/all?page=1&count=2"));

            var json = await response2.Content.ReadAsStringAsync();

            var products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            products.Should().HaveCount(2);

            var uptRequestBody = new AddProductDTO
            {
                Name = faker.Internet.UserName(),
                PricePerKilo = faker.Random.Decimal(1, 10),
                ExpirationTime = new TimeSpan(1, 0, 0, 0),
            };

            var uptRequest = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/products/{products[0].Id}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(uptRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act

            var response = await _httpClient.SendAsync(uptRequest);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
