using AutoMapper.Configuration.Annotations;
using BLL.DTO;
using Bogus;
using DAL.Persistanse;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;
using Tests.IntegrationTests;

namespace Tests.Integrationtests.FridgeController
{
    public class AddFridgeTests : ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeFridgeGenerator.InitializeData();
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task AddFridge_Success_ShouldAddFridgeToDatabase()
        {
            var faker = new Faker();

            var requestBody = new FridgeAddDTO
            {
                Name = faker.Random.String(16),
                Model = faker.Random.String(16),
                Serial = faker.Random.String(16),
                BoughtDate = DateTime.UtcNow,
                BoxNumber = faker.Random.Number(1, 10),
            };

            var request = new HttpRequestMessage(new HttpMethod("PUT"), "/api/fridge")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act
            var response = await _httpClient.SendAsync(request);

            //Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddFridge_Fail_InvalidRequest()
        {
            var faker = new Faker();
            var requestBody = new FridgeAddDTO
            {
                Name = null,
                Model = faker.Random.String(16),
                Serial = faker.Random.String(16),
                BoughtDate = DateTime.UtcNow,
                BoxNumber = faker.Random.Number(1, 10),
            };
            var request = new HttpRequestMessage(new HttpMethod("PUT"), "/api/fridge")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            // Act
            var response = await _httpClient.SendAsync(request);

            //Asert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}