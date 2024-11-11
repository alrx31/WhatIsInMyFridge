using Application.DTO;
using Bogus;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;
using Domain.Entities;

namespace Tests.IntegrationTests.Controllers.RecieptsController;

public class SuggestRecieptsTests : ControllerTests
{
    [Fact]
    public async Task SuggestReciepts_Success_ShouldReturnReciept()
    {
        var faker = new Faker();

        var product = new AddProductDTO
        {
            Name = faker.Commerce.Product(),
            PricePerKilo = faker.Random.Number(1, 10),
            ExpirationTime = new TimeSpan()
        };

        var addPrResponse = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"), $"/api/products")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(product),
                Encoding.UTF8,
                "application/json")
        });

        addPrResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getPrResponse = await _httpClient.GetAsync($"/api/Products/all");

        var jsonPr = await getPrResponse.Content.ReadAsStringAsync();

        var pr = JsonSerializer.Deserialize<List<Product>>(jsonPr, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })[0];

        var request = new AddProductToListDTO
        {
            ProductId = pr.Id,
            Weight = faker.Random.Number(1, 10),
            Cost = faker.Random.Number(1, 10)
        };

        var addRequestBody = new AddRecieptDTO
        {
            Name = faker.Internet.UserName(),
            CookDuration = new TimeSpan(1, 0, 0),
            Portions = faker.Random.Number(1, 10),
            Kkal = faker.Random.Number(1, 10)
        };

        var request1 = new HttpRequestMessage(new HttpMethod("PUT"), "/api/reciepts")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(addRequestBody),
                Encoding.UTF8,
                "application/json")
        };

        var response1 = await _httpClient.SendAsync(request1);

        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        // get all reciepts

        var response2 =
            await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("GET"),
                "/api/reciepts/all?page=1&count=1"));

        var json = await response2.Content.ReadAsStringAsync();

        var reciepts =
            JsonSerializer.Deserialize<List<Reciept>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        reciepts.Should().HaveCount(1);

        var AddPrrequest = new AddProductToRecieptDTO
        {
            ProductId = pr.Id,
            RecieptId = reciepts[0].Id,
            Weight = faker.Random.Number(1, 10)
        };
        
        var response3 =
            await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("PUT"),
                "/api/Reciepts/reciept/products")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(AddPrrequest),
                    Encoding.UTF8,
                    "application/json"
                )
            });

        response3.StatusCode.Should().Be(HttpStatusCode.OK);

        var SuggertRequest = new SuggestRecieptDTO
        {
            products = new List<ProductInReciept>
            {
                new ProductInReciept(
                    pr,
                    faker.Random.Number(12,15)
                    )
            }
        };
        
        // Act

        var response =
            await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("POST"), "/api/Reciepts/reciept/suggest")
            {
                Content = new StringContent(
                        JsonSerializer.Serialize(SuggertRequest),
                        Encoding.UTF8,
                        "application/json"
                    )
            });
        
        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resJson = await response.Content.ReadAsStringAsync();

        var suggestReciept = JsonSerializer.Deserialize<Reciept>(resJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        suggestReciept.Id.Should().BeEquivalentTo(reciepts[0].Id);
    }
}