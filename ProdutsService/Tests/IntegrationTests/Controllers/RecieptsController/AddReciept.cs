using Application.DTO;
using Bogus;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Tests.IntegrationTests.Controllers.RecieptsController
{
    public class AddReciept : ControllerTests
    {
        [Fact]
        public async Task AddReciept_Success_ShouldReturnOk()
        {
            // Arrange
            var faker = new Faker();

            var addRequestBody = new AddRecieptDTO
            {
                Name = faker.Random.String(16),
                CookDuration = new TimeSpan(),
                Portions = faker.Random.Number(1, 10),
                Kkal = faker.Random.Number(1, 10)
            };

            // Act
            var addResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), "/api/reciepts")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(addRequestBody),
                    Encoding.UTF8,
                    "application/json")
            });

            // Assert
            addResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
