﻿using Application.DTO;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Tests.IntegrationTests.Controllers.RecieptsController
{
    public class GetAllReciepts : ControllerTests
    {
        [Fact]
        public async Task GetAllReciepts_Success_ShouldReturnReciept()
        {
            var faker = new Faker();

            var addRequestBody = new AddRecieptDTO
            {
                Name = faker.Internet.UserName(),
                CookDuration = new TimeSpan(1, 0, 0),
                Portions = faker.Random.Number(1, 10),
                Kkal = faker.Random.Number(1, 10)
            };

            var request = new HttpRequestMessage(new HttpMethod("PUT"), "/api/reciepts")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(addRequestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            var response1 = await _httpClient.SendAsync(request);

            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            
            // Act

            var response2 = await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod("GET"), "/api/reciepts/all?page=1&count=1"));

            // Assert

            var json = await response2.Content.ReadAsStringAsync();

            var reciepts = JsonSerializer.Deserialize<List<Reciept>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });



            reciepts.Should().HaveCount(1);
        }
    }
}
