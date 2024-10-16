using FluentAssertions;
using System.Net;

namespace Tests.IntegrationTests
{
    public class Test : ControllerTests
    {
        [Fact]
        public async Task Test1()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/user/{1}");

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
