using Application.DTO;
using Bogus;
using Domain.Entities;
using System.Net;
using System.Text.Json;
using System.Text;
using FluentAssertions;
using AutoMapper.Configuration.Annotations;

namespace Tests.IntegrationTests.Controllers.ListController
{
    public class AddProductToListTests : ControllerTests
    {
        [Fact]
        public async Task AddProductToList_Success_ShouldAddProductToList()
        {
            var faker = new Faker();

            var addRequestBody = new AddListDTO
            {
                Name = faker.Commerce.Product(),
                Weight = faker.Random.Number(1, 10),
                FridgeId = faker.Random.Number(1, 10),
                BoxNumber = faker.Random.Number(1, 10),
                Price = faker.Random.Decimal(1, 10)
            };

            var addResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), "/api/list")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(addRequestBody),
                    Encoding.UTF8,
                    "application/json")
            });

            addResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _httpClient.GetAsync($"/api/list/name/{addRequestBody.Name}");

            var json = await getResponse.Content.ReadAsStringAsync();

            var list = JsonSerializer.Deserialize<ProductsList>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var product = new AddProductDTO
            {
                Name = faker.Commerce.Product(),
                PricePerKilo = faker.Random.Number(1, 10),
                ExpirationTime = new TimeSpan()
            };

            var addPrResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), $"/api/products")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(product),
                    Encoding.UTF8,
                    "application/json")
            });

            addPrResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getPrResponse = await _httpClient.GetAsync($"/api/products/all");

            var jsonPr = await getResponse.Content.ReadAsStringAsync();

            var pr = JsonSerializer.Deserialize<Product>(jsonPr, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var request = new AddProductToListDTO
            {
                ProductId = pr.Id,
                Weight = faker.Random.Number(1, 10),
                Cost = faker.Random.Number(1, 10)
            };

            // Act

            var response = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), $"/api/list/{list.Id}/product")
            {
                Content = new StringContent(
                        JsonSerializer.Serialize(request),
                        Encoding.UTF8,
                        "application/json"
                        )
            });

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddProductToList_Fail_InvalidDTO()
        {
            var faker = new Faker();
            var request = new AddProductToListDTO();

            // Act

            var response = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), $"api/{faker.Internet.DomainName}/product"));
        }
    }
}
