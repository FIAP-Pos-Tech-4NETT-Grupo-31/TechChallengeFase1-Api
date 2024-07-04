using Contatos.API.Controllers;
using Contatos.API.Dto;
using Contatos.API.Interfaces;
using Contatos.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Contatos.API.Tests.Controllers
{
    [TestFixture]
    public class ContatosControllerTests
    {
        private ContatosController _contatoController;
        private Mock<IContatoService> _mockContatoService;

        private readonly List<ContatoDtoResponse> _mockListaDeContatos = [
            new() {
                Id = 1, Nome = "Jo�o",Email = "joao@test.com", Telefone = "995678721", DDD = 1,
                Regiao = new()
                {
                    DDD = 32,
                    UF = "MG"
                }
            },
            new() {
                Id = 2, Nome = "Maria", Email = "maria@test.com", Telefone = "99764326", DDD = 2,
                Regiao = new Regiao(){
                    DDD = 11,
                    UF = "SP"
                }
            },
            new() {
                Id = 2, Nome = "Jos�", Email = "jose@test.com", Telefone = "309882983", DDD = 3,
                Regiao = new Regiao(){
                    DDD = 11,
                    UF = "SP"
                }
            }
        ];

        [SetUp]
        public void Setup()
        {
            _mockContatoService = new Mock<IContatoService>();
            _contatoController = new ContatosController(_mockContatoService.Object, null);
        }

        [Test]
        public async Task Deve_Retornar_Ok_Para_Get_Contatos()
        {
            // Arrange
            var contatos = _mockListaDeContatos
                .Select(contatoDto => (Contato)contatoDto);

            _mockContatoService
                .Setup(x => x.RetornarListaDeContatos(null))
                .ReturnsAsync(new Tuple<IEnumerable<Contato>, bool>(contatos, false));

            // Act
            var result = await _contatoController.GetAll(null);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(_mockListaDeContatos));
        }

        [Test]
        public async Task Deve_Retornar_Contatos_Filtrados_Por_DDD()
        {
            // Arrange
            var contatos = _mockListaDeContatos
                .Select(contatoDto => (Contato)contatoDto);

            var ddd = 11;
            var contatosFiltrados = contatos
                .Where(contato => contato.Regiao?.DDD == ddd);

            _mockContatoService
                .Setup(x => x.RetornarListaDeContatos(ddd))                
                .Returns(Task.FromResult<Tuple<IEnumerable<Contato>, bool>>(new Tuple<IEnumerable<Contato>, bool>(contatosFiltrados, false)));

            // Act
            var result = await _contatoController.GetAll(ddd);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            var contatosFiltradosDto = contatosFiltrados
                .Select(contato => (ContatoDtoResponse)contato);
            Assert.That(okResult.Value, Is.EqualTo(contatosFiltradosDto));
        }
    }
}
