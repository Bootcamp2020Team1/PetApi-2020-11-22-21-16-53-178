using System;
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
    public class PetAPITest
    {
        // petstore/addNewPet
        [Fact]
        public async Task Should_Add_Pet_When_Add_Pet_()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
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
    }
}
