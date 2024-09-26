using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Contatos.API.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace Contatos.API.IntegrationTests.Controllers
{
    [TestFixture]
    public class ContatosControllerTests
    {
        private HttpClient _client;
        
        private readonly dynamic _credencial = new
        {
            Username = "Grupo31",
            Password = "Grupo#31"
        };

        private readonly Contato _contato1 = new() { Nome = "Teste1", Email = "teste1@teste.com", DDD = 31, Telefone = "123456789" };
        private readonly Contato _contato2 = new() { Nome = "Teste2", Email = "teste2@teste.com", DDD = 11, Telefone = "987654321" };
        private readonly Contato _contato3 = new() { Nome = "Teste3", Email = "teste3@teste.com", DDD = 21, Telefone = "456123789" };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var factory = new CustomWebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        private async Task SetupAuthorizationHeader()
        {
            if (_client.DefaultRequestHeaders.Authorization == null)
            {
                var token = await GetAccessToken();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<string> GetAccessToken()
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(_credencial), 
                Encoding.UTF8, 
                "application/json");

            var response = await _client.PostAsync("/Autenticacao", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<string>(jsonContent);
                if (!string.IsNullOrEmpty(token))
                {
                    return token;
                }
            }

            throw new InvalidOperationException("Failed to obtain access token.");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
        }

        [Test]
        public async Task Deve_Retornar_Ok_Para_Get_Contatos()
        {
            //Arrange
            await SetupAuthorizationHeader();

            // Act
            var response = await _client.GetAsync("/Contatos");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Deve_Retornar_Created_Para_Post_Contatos()
        {
            // Arrange
            var json = JsonConvert.SerializeObject(_contato1);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            await SetupAuthorizationHeader();

            // Act
            var response = await _client.PostAsync("/Contatos", stringContent);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public async Task Deve_Retornar_NoContent_Para_Post_Contatos()
        {
            // Arrange
            var json = JsonConvert.SerializeObject(_contato2);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            await SetupAuthorizationHeader();

            // Act - Insert a new contact
            var postResponse = await _client.PostAsync("/Contatos", stringContent);
            postResponse.EnsureSuccessStatusCode();
            var jsonContent = await postResponse.Content.ReadAsStringAsync();
            var novoContato = JsonConvert.DeserializeObject<Contato>(jsonContent);

            // Act - Update the contact
            var putResponse = await _client.PutAsync($"/Contatos/{novoContato?.Id}", stringContent);

            // Assert
            Assert.That(putResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
