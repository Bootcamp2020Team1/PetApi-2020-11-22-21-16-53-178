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
            client.DeleteAsync("pets/Clear");
        }

        [Fact]
        public async Task Should_add_pet()
        {
            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("pets/newPet", requestBody);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_return_all_pets_when_getall()
        {
            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("pets/newPet", requestBody);

            var response = await client.GetAsync("pets");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet }, actualPets);
        }

        [Fact]
        public async Task Should_return_pets_when_get_by_name()
        {
            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            StringContent requestBody = new StringContent(JsonConvert.SerializeObject(pet), Encoding.UTF8, "application/json");
            await client.PostAsync("pets/newPet", requestBody);

            var response = await client.GetAsync("pets/Baymax");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_remove_pet_when_been_bought()
        {
            Pet pet1 = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "Wuhuang", type: "cat", color: "black", price: 3000);
            StringContent requestBody1 = new StringContent(JsonConvert.SerializeObject(pet1), Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(JsonConvert.SerializeObject(pet2), Encoding.UTF8, "application/json");
            await client.PostAsync("pets/newPet", requestBody1);
            await client.PostAsync("pets/newPet", requestBody2);

            await client.DeleteAsync("pets/Wuhuang");
            var response = await client.GetAsync("pets");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet1 }, actualPets);
        }

        [Fact]
        public async Task Should_modify_price_when_patch()
        {
            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            StringContent requestBody = new StringContent(JsonConvert.SerializeObject(pet), Encoding.UTF8, "application/json");
            await client.PostAsync("pets/newPet", requestBody);
            PetUpdateModel petUpdate = new PetUpdateModel(name: "Baymax", price: 3000);
            StringContent updateRequestBody = new StringContent(JsonConvert.SerializeObject(petUpdate), Encoding.UTF8, "application/json");

            var response = await client.PatchAsync("pets/Baymax", updateRequestBody);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(3000, actualPet.Price);
        }

        [Fact]
        public async Task Should_return_pets_by_type()
        {
            Pet pet1 = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "Wuhuang", type: "cat", color: "black", price: 3000);
            StringContent requestBody1 = new StringContent(JsonConvert.SerializeObject(pet1), Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(JsonConvert.SerializeObject(pet2), Encoding.UTF8, "application/json");
            await client.PostAsync("pets/newPet", requestBody1);
            await client.PostAsync("pets/newPet", requestBody2);

            var response = await client.GetAsync("pets?type=cat");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet2 }, actualPets);
        }

        [Fact]
        public async Task Should_return_pets_by_color()
        {
            Pet pet1 = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "Wuhuang", type: "cat", color: "black", price: 3000);
            StringContent requestBody1 = new StringContent(JsonConvert.SerializeObject(pet1), Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(JsonConvert.SerializeObject(pet2), Encoding.UTF8, "application/json");
            await client.PostAsync("pets/newPet", requestBody1);
            await client.PostAsync("pets/newPet", requestBody2);

            var response = await client.GetAsync("pets?color=white");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet1 }, actualPets);
        }

        [Fact]
        public async Task Should_return_pets_by_price_range()
        {
            Pet pet1 = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "Wuhuang", type: "cat", color: "black", price: 3000);
            StringContent requestBody1 = new StringContent(JsonConvert.SerializeObject(pet1), Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(JsonConvert.SerializeObject(pet2), Encoding.UTF8, "application/json");
            await client.PostAsync("pets/newPet", requestBody1);
            await client.PostAsync("pets/newPet", requestBody2);

            var response = await client.GetAsync("pets?min=3000&max=4000");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet2 }, actualPets);
        }
    }
}
