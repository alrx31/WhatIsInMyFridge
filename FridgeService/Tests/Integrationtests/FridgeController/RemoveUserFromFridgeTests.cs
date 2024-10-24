using BLL.DTO;
using Bogus;
using DAL.Entities;
using DAL.Persistanse;
using FluentAssertions;
using Renci.SshNet.Security.Cryptography;
using System.Net;
using System.Text;
using System.Text.Json;
using Tests.IntegrationTests;

namespace Tests.Integrationtests.FridgeController
{
    public class RemoveUserFromFridgeTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeFridgeGenerator.InitializeData();
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task RemoveUserFromFridge_Success_ShouldRemoveUser()
        {
            var fridge = _fakeFridgeGenerator.Fridges.First();

            var userId = new Faker().Random.Number(1, 100);

            var addResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), $"/api/fridge")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new FridgeAddDTO
                    {
                        Name = fridge.name,
                        Model = fridge.model,
                        Serial = fridge.serial,
                        BoughtDate = fridge.boughtDate,
                        BoxNumber = fridge.boxNumber
                    }),
                    Encoding.UTF8,
                    "application/json")
            });

            addResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var addUserResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), $"/api/fridge/{fridge.serial}/{fridge.boxNumber}/users/{userId}"));

            addUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var fridgeRespo = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("GET"), $"/api/fridge/serial/{fridge.serial}/{fridge.boxNumber}"));

            var fridgeRes = JsonSerializer.Deserialize<Fridge>(await fridgeRespo.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/fridge/{fridgeRes.id}/users/{userId}");

            // Act 
            var response = await _httpClient.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RemoveUserFromFridge_Fail_InfalidUserOrFridgeId()
        {
            var faker = new Faker();

            // Act

            var response = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/fridge/{faker.Random.Int(-10,0)}/users/{faker.Random.Int(-10,0)}"));

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
