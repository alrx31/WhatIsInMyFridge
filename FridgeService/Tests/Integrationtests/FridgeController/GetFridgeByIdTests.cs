using BLL.DTO;
using DAL.Persistanse;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;
using Tests.IntegrationTests;

namespace Tests.Integrationtests.FridgeController
{
    public class GetFridgeByIdTests:ControllerTests
    {
        protected override void InitializeDatabase(ApplicationDbContext dataContext)
        {
            _fakeFridgeGenerator.InitializeData();
            dataContext.SaveChanges();
        }

        [Fact]
        public async Task GetFridgeById_Success_WhenFridgeExist()
        {
            var fridge = _fakeFridgeGenerator.Fridges.First();

            var responseAdd = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), $"/api/fridge")
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

            responseAdd.StatusCode.Should().Be(HttpStatusCode.OK);

            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/fridge/{fridge.id}");

            //Act
            var response = await _httpClient.SendAsync(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
