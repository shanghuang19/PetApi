using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace PetAPITest
{
    public class PetsAPITest
    {
        private HttpClient httpClient;
        public PetsAPITest()
        {
            WebApplicationFactory<Program> webApplicationFactory = new WebApplicationFactory<Program>();
            this.httpClient = webApplicationFactory.CreateClient();
        }
        [Fact]
        public async Task Should_return_pet_with_status_code_when_created_given_a_JSON_pet()
        {
            // Given
            WebApplicationFactory<Program> webApplicationFactory = new WebApplicationFactory<Program>();
            HttpClient httpClient = webApplicationFactory.CreateClient();

            Pet petGiven = new Pet("Snowball", "Cat", "White", 99);
            string jsonPetgiven = JsonConvert.SerializeObject(petGiven);

            // When

            //HttpResponseMessage message = await client.PostAsJsonAsync("api/pets", pet);
            //Pet? result = await message.Content.ReadFromJsonAsync<Pet>();

            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("api/pets",new StringContent(jsonPetgiven, Encoding.UTF8, "application/json"));
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            Pet? petCreated = JsonConvert.DeserializeObject<Pet>(result);
            // Then
            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            Assert.NotNull(petCreated);
            Assert.Equal(petGiven, petCreated);
        }

        

        [Fact]
        public async Task Should_return_pet_bad_request_when_created_given_an_existed_JSON_pet()
        {
            await httpClient.DeleteAsync("api/pets");
            // Given
            Pet petGiven = new Pet("Dudu", "Cat", "White", 99);

            // When
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("api/pets", petGiven);
            Pet? petCreated = await httpResponseMessage.Content.ReadFromJsonAsync<Pet>();

            HttpResponseMessage httpResponseMessage1 = await httpClient.PostAsJsonAsync("api/pets", petGiven);
            Pet? petCreated1 = await httpResponseMessage1.Content.ReadFromJsonAsync<Pet>();

            // Then
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage1.StatusCode);
        }

    }
}