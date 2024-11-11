using BLL.DTO;
using Bogus;
using DAL.Entities;
using DAL.Persistanse;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;
using Tests.IntegrationTests;

namespace Tests.Integrationtests.FridgeController
{
    public class UpdateFridgeTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeFridgeGenerator.InitializeData();
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task UpdateFridge_Success_ShouldUpdateFridge()
        {
            var faker = new Faker();
            var fridge = _fakeFridgeGenerator.Fridges.First();

            var addFridgeResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), $"/api/fridge")
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

            addFridgeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var fridgeResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("GET"), $"/api/fridge/serial/{fridge.serial}/{fridge.boxNumber}"));

            var fridgeRes = JsonSerializer.Deserialize<Fridge>(await fridgeResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/fridge/{fridgeRes.id}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new FridgeAddDTO
                    {
                        Name = faker.Random.String(10),
                        Model = faker.Random.String(10),
                        Serial = fridgeRes.serial,
                        BoughtDate = fridgeRes.boughtDate,
                        BoxNumber = fridgeRes.boxNumber
                    }),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act

            var response = await _httpClient.SendAsync(request);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedFridgeResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("GET"), $"/api/fridge/serial/{fridgeRes.serial}/{fridgeRes.boxNumber}"));

            var updatedFridge = JsonSerializer.Deserialize<Fridge>(await updatedFridgeResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            updatedFridge.name.Should().NotBe(fridgeRes.name);
            updatedFridge.model.Should().NotBe(fridgeRes.model);
        }

        [Fact]
        public async Task UpdateFridge_Fail_WhenFridgeNotExist()
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/api/fridge/{Guid.NewGuid()}")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(new FridgeAddDTO
                    {
                        Name = "Test",
                        Model = "Test",
                        Serial = Guid.NewGuid().ToString(),
                        BoughtDate = DateTime.Now,
                        BoxNumber = 1
                    }),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act

            var response = await _httpClient.SendAsync(request);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
