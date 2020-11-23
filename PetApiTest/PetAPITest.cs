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
        private readonly TestServer server;
        private readonly HttpClient client;
        public PetAPITest()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Fact]
        public async Task Should_add_pet()
        {
            await client.DeleteAsync("petStore/Clear");
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

        [Fact]
        public async Task Should_return_pets_when_get_by_name()
        {
            await client.DeleteAsync("petStore/Clear");
            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            StringContent requestBody = new StringContent(JsonConvert.SerializeObject(pet), Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/AddNewPet", requestBody);

            var response = await client.GetAsync("petStore/Baymax");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_remove_pet_when_been_bought()
        {
            await client.DeleteAsync("petStore/Clear");
            Pet pet1 = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "Wuhuang", type: "cat", color: "black", price: 3000);
            StringContent requestBody1 = new StringContent(JsonConvert.SerializeObject(pet1), Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(JsonConvert.SerializeObject(pet2), Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/AddNewPet", requestBody1);
            await client.PostAsync("petStore/AddNewPet", requestBody2);

            var response = await client.DeleteAsync("petStore/{Wuhuang}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet1, actualPet);
        }
    }
}
