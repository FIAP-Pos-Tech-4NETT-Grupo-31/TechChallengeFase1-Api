using Contatos.InclusaoService.Dto;
using Testcontainers.RabbitMq;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;

namespace Contatos.CadastroService.Unit.Tests
{
    [TestFixture]
    public class RabbitMqIntegrationTests
    {
        private RabbitMqContainer _rabbitMqContainer;
        private WebApplicationFactory<Program> _factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithHostname("localhost")
                .WithPortBinding(5672, false)
                .WithUsername("guest")
                .WithPassword("Grupo#31")
                .Build();
            _rabbitMqContainer.StartAsync().Wait();
        }

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _rabbitMqContainer.DisposeAsync();
        }

        [Test]
        public async Task PostContato_PublishesMessageToRabbitMq()
        {
            var client = _factory.CreateClient();
            var contato = new ContatoDtoRequest
            {
                Nome = "John Doe",
                DDD = 11,
                Telefone = "123456789",
                Email = "john.doe@example.com"
            };

            // Act
            var response = await client.PostAsJsonAsync("/Contatos", contato);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.That(responseString, Is.EqualTo("Novo contato enviado para RabbitMQ"));
        }       
    }
}