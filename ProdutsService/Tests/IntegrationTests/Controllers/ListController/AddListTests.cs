using Application.DTO;
using Bogus;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Tests.IntegrationTests.Controllers.ListController
{
    public class AddListTests : ControllerTests
    {
        [Fact]
        public async Task AddList_Success_ShouldReturnOk()
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

            // Act
            var addResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), "/api/list")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(addRequestBody),
                    Encoding.UTF8,
                    "application/json")
            });

            // Assert
            addResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddList_Fail_ValidationError()
        {
            var faker = new Faker();

            var addRequestBody = new AddListDTO
            {

            };

            // Act
            var addResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), "/api/list")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(addRequestBody),
                    Encoding.UTF8,
                    "application/json")
            });

            // Assert
            addResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
