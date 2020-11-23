using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PetApiTest
{
    public class PetApiTest
    {
        [Fact]
        public async Task Should_Add_Pet_When_Add_Pet()
        {
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet("Sun", "dog", "yellow", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            //when
            var response = await client.PostAsync("petStore/addNewPet", requestBody);

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_Return_Pets_When_Get_All_Pet()
        {
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            Pet pet = new Pet("Sun", "dog", "yellow", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);
            //when
            var response = await client.GetAsync("petStore/pets");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet }, actualPets);
        }

        [Fact]
        public async Task Should_Return_Pet_When_Get_Pet_By_Name()
        {
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            Pet pet = new Pet("Sun", "dog", "yellow", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);
            //when
            var response = await client.GetAsync("petStore/petname/Sun");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_Return_Pet_When_Get_Pet_By_Type()
        {
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            Pet pet = new Pet("Moon", "cat", "yellow", 1000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);
            //when
            var response = await client.GetAsync("petStore/pettype/dog");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Pet dog = new Pet("Sun", "dog", "yellow", 5000);
            Assert.Equal(new List<Pet>() { dog }, actualPets);
        }

        [Fact]
        public async Task Should_Return_Pet_When_Get_Pet_By_Color()
        {
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");
            Pet pet = new Pet("Moon", "cat", "yellow", 1000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);
            //when
            var response = await client.GetAsync("petStore/petcolor/yellow");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Pet dog = new Pet("Moon", "cat", "yellow", 1000);
            Assert.Equal(new List<Pet>() { dog }, actualPets);
        }

        [Fact]
        public async Task Should_Return_Pets_When_Get_Pets_By_Price_Range()
        {
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            Pet cat = new Pet("Moon", "cat", "yellow", 1000);
            string requestCat = JsonConvert.SerializeObject(cat);
            StringContent requestBodyCat = new StringContent(requestCat, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBodyCat);

            Pet dog = new Pet("Sun", "dog", "yellow", 5000);
            string requestDog = JsonConvert.SerializeObject(dog);
            StringContent requestBodyDog = new StringContent(requestDog, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBodyDog);
            //when
            var response = await client.GetAsync("petStore/petpricerange/1000-5000");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { cat, dog }, actualPets);
        }

        [Fact]
        public async Task Should_Delete_Pet_When_Delete_By_Name()
        {
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            Pet cat = new Pet("Moon", "cat", "yellow", 1000);
            string requestCat = JsonConvert.SerializeObject(cat);
            StringContent requestBodyCat = new StringContent(requestCat, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBodyCat);

            //when
            await client.DeleteAsync("petStore/petdelete/Moon");
            var response = await client.GetAsync("petStore/petname/Moon");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(null, actualPet);
        }
    }
}
