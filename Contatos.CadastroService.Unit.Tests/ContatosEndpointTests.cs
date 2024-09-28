using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using Contatos.InclusaoService.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace Contatos.CadastroService.Unit.Tests
{
    [TestFixture]
    public class ContatosEndpointTests
    {
        private WebApplicationFactory<Program> _factory;
        private Mock<IConnectionFactory> _mockConnectionFactory;
        private Mock<IConnection> _mockConnection;
        private Mock<IModel> _mockChannel;

        [SetUp]
        public void SetUp()
        {
            _mockConnectionFactory = new Mock<IConnectionFactory>();
            _mockConnection = new Mock<IConnection>();
            _mockChannel = new Mock<IModel>();

            _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockConnection.Object);
            _mockConnection.Setup(c => c.CreateModel()).Returns(_mockChannel.Object);

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton(_mockConnectionFactory.Object);
                    });
                });
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        [Test]
        public async Task PostContato_ReturnsSuccessStatusCode()
        {
            // Arrange
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
