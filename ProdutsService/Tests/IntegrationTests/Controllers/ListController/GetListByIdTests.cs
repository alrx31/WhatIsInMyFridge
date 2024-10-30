using Application.DTO;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Tests.IntegrationTests.Controllers.ListController
{
    public class GetListByIdTests : ControllerTests
    {
        [Fact]
        public async Task GetListById_Success_ShouldReturnOk()
        {
            // Arrange
            var faker = new Faker();

            var addRequestBody = new AddListDTO
            {
                Name = faker.Random.String(16),
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

            var getByNameResponse = await _httpClient.GetAsync($"/api/list/name/{addRequestBody.Name}");

            var json = await getByNameResponse.Content.ReadAsStringAsync();

            var existList = JsonSerializer.Deserialize<ProductsList>(json);

            // Act
            var getResponse = await _httpClient.GetAsync($"/api/list/{existList.Id}");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
