using System.Net;
using System.Text;
using System.Text.Json;
using Application.DTO;
using Bogus;
using Domain.Entities;
using FluentAssertions;

namespace Tests.IntegrationTests.Controllers.ListController;

public class DeleteProductFromListTests : ControllerTests
{
    [Fact]
    public async Task DeleteProductFromList_Success_ShouldDeleteProduct()
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

            var jsonPr = await getPrResponse.Content.ReadAsStringAsync();

            var pr = JsonSerializer.Deserialize<List<Product>>(jsonPr, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })[0];

            var request = new AddProductToListDTO
            {
                ProductId = pr.Id,
                Weight = faker.Random.Number(1, 10),
                Cost = faker.Random.Number(1, 10)
            };

            var response3 = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), $"/api/list/{list.Id}/product")
            {
                Content = new StringContent(
                        JsonSerializer.Serialize(request),
                        Encoding.UTF8,
                        "application/json"
                        )
            });

            response3.StatusCode.Should().Be(HttpStatusCode.OK);
            
            // Act

            var response = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/List/{list.Id}/{pr.Id}"));
            
            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}