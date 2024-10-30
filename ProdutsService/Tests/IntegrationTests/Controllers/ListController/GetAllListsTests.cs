using Application.DTO;
using Bogus;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Tests.IntegrationTests.Controllers.ListController
{
    public class GetAllListsTests : ControllerTests
    {
        [Fact]
        public async Task GetAllLists_Success_ShouldReturnAllLists()
        {
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

            addResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/list");
            
            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
