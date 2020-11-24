using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using Xunit;

namespace PetApiTest
{
    public class PetApiTest
    {
        private readonly TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        private readonly HttpClient client;
        public PetApiTest()
        {
            client = server.CreateClient();
            client.DeleteAsync("petStore/clear");
        }

        // petstore/addNewPet
        [Fact]
        public async Task Should_Add_Pet_When_Add_Pet_()
        {
            //given
            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
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
        public async Task Should_Return_All_Pets_When_Get_All_Pets()
        {
            //given
            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            var petList = new List<Pet>();
            petList.Add(pet);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            //when
            await client.PostAsync("petStore/addNewPet", requestBody);
            var getResponse = await client.GetAsync("petStore/pets");
            //then
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Pet> actualPetList = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(petList, actualPetList);
        }

        [Fact]
        public async Task Should_Return_Correct_Pets_When_Get_Pets_By_Name()
        {
            //given
            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            //when
            await client.PostAsync("petStore/addNewPet", requestBody);
            var getResponse = await client.GetAsync("petStore/petname/Baymax");
            //then
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_Delete_Pets_When_Delete_By_Name()
        {
            //given
            Pet pet1 = new Pet(name: "pet1", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "pet2", type: "cat", color: "white", price: 5000);
            var petList = new List<Pet>()
            {
                pet2
            };
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            //when
            await client.PostAsync("petStore/addNewPet", requestBody1);
            await client.PostAsync("petStore/addNewPet", requestBody2);
            await client.DeleteAsync("petStore/petname/pet1");
            var getResponse = await client.GetAsync("petStore/pets");
            //then
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Pet> actualPetList = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(petList, actualPetList);
        }

        [Fact]
        public async Task Should_Modify_Price_Of_Pets_When_Patch()
        {
            //given
            Pet pet = new Pet(name: "Baymax", type: "dog", color: "white", price: 5000);
            UpdatePriceModel upDating = new UpdatePriceModel(name: "Baymax", price: 200);
            Pet updatedPet = new Pet(name: "Baymax", type: "dog", color: "white", price: 200);
            string request = JsonConvert.SerializeObject(pet);
            string requestUpdate = JsonConvert.SerializeObject(upDating);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            StringContent updateBody = new StringContent(requestUpdate, Encoding.UTF8, "application/json");

            //when
            await client.PostAsync("petStore/addNewPet", requestBody);
            var patchResponse = await client.PatchAsync("petStore/Baymax", updateBody);
            await client.GetAsync("petStore/petname/Baymax");
            //then
            patchResponse.EnsureSuccessStatusCode();
            var responseString = await patchResponse.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(updatedPet, actualPet);
        }

        [Fact]
        public async Task Should_Get_Dogs_When_Get_By_Type()
        {
            //given
            Pet pet1 = new Pet(name: "pet1", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "pet2", type: "cat", color: "white", price: 5000);
            var petList = new List<Pet>()
            {
                pet1
            };
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            //when
            await client.PostAsync("petStore/addNewPet", requestBody1);
            await client.PostAsync("petStore/addNewPet", requestBody2);

            var getResponse = await client.GetAsync("petStore/pettype?type=dog");
            //then
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Pet> actualPetList = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(petList, actualPetList);
        }

        [Fact]
        public async Task Should_Get_Correct_Pets_When_Put_By_Price_Range()
        {
            //given
            Pet pet1 = new Pet(name: "pet1", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "pet2", type: "cat", color: "white", price: 200);
            var priceRange = new PriceRangeModel(100, 1000);
            var petList = new List<Pet>()
            {
                pet2
            };
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            string request3 = JsonConvert.SerializeObject(priceRange);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            StringContent requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            //when
            await client.PostAsync("petStore/addNewPet", requestBody1);
            await client.PostAsync("petStore/addNewPet", requestBody2);

            var getResponse = await client.PutAsync("petStore/petprice", requestBody3);
            //then
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Pet> actualPetList = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(petList, actualPetList);
        }

        [Fact]
        public async Task Should_Get_White_Animals_When_Get_By_Color()
        {
            //given
            Pet pet1 = new Pet(name: "pet1", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "pet2", type: "cat", color: "black", price: 5000);
            var petList = new List<Pet>()
            {
                pet1
            };
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            //when
            await client.PostAsync("petStore/addNewPet", requestBody1);
            await client.PostAsync("petStore/addNewPet", requestBody2);

            var getResponse = await client.GetAsync("petStore/petcolor?color=white");
            //then
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Pet> actualPetList = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(petList, actualPetList);
        }

        [Fact]
        public async Task Should_Get_Correct_Pets_When_Get_By_Price_Range()
        {
            //given
            Pet pet1 = new Pet(name: "pet1", type: "dog", color: "white", price: 5000);
            Pet pet2 = new Pet(name: "pet2", type: "cat", color: "white", price: 200);
            var petList = new List<Pet>()
            {
                pet2
            };
            string request1 = JsonConvert.SerializeObject(pet1);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            //when
            await client.PostAsync("petStore/addNewPet", requestBody1);
            await client.PostAsync("petStore/addNewPet", requestBody2);

            var getResponse = await client.GetAsync("petStore/PriceRange?min=100&max=1000");
            //then
            getResponse.EnsureSuccessStatusCode();
            var responseString = await getResponse.Content.ReadAsStringAsync();
            List<Pet> actualPetList = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(petList, actualPetList);
        }
    }
}
