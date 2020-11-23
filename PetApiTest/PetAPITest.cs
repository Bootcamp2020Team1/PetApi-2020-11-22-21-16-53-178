using System;
using Xunit;
using PetApi;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PetApiTest
{
    public class PetAPITest
    {
        [Fact]
        public async Task Should_add_pet()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();

            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("petStore/AddNewPet", requestBody);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_return_all_pets_when_getall()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            await client.DeleteAsync("petStore/Clear");

            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/AddNewPet", requestBody);

            var response = await client.GetAsync("petStore/Pets");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet }, actualPets);
        }
    }
}
