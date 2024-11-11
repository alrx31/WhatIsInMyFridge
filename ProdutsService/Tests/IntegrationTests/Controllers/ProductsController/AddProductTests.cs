using Application.DTO;
using Bogus;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Tests.IntegrationTests.Controllers.ProductsController
{
    public class AddProductTests : ControllerTests
    {
        [Fact]
        public async Task AddProduct_Success_ShouldAddProduct()
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

            // Act

            var response = await _httpClient.SendAsync(request);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
