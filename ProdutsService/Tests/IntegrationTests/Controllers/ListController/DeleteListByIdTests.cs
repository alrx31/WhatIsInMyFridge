using Application.DTO;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Tests.IntegrationTests.Controllers.ListController
{
    public class DeleteListByIdTests : ControllerTests
    {
        [Fact]
        public async Task DeleteListById_Success_ShouldDeleteList()
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

            // Act
            var response = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/list/{list.Id}"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
