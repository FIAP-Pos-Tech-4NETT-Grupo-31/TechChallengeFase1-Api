using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contatos.API.Interfaces;
using Contatos.API.Models;
using Contatos.API.Services;
using Moq;

namespace Contatos.API.Tests.Services
{
    [TestFixture]
    public class ContatoServiceTests    
    {
        private IContatoService _contatoService;
        private Mock<IContatoRepository> _mockContatoRepository;

        private readonly List<Contato> _mockListaDeContatos = new()
        {
            new()
            {
                Id = 1, Nome = "João", Email = "", Telefone = "995678721", DDD = 1,
                Regiao = new()
                {
                    DDD = 32,
                    UF = "MG"
                }
            },
            new()
            {
                Id = 2, Nome = "Maria", Email = "", Telefone = "99764326", DDD = 2,
                Regiao = new Regiao()
                {
                    DDD = 11,
                    UF = "SP"
                }
            }
        };

        [SetUp]
        public void Setup()
        {
            _mockContatoRepository = new Mock<IContatoRepository>();
            _contatoService = new ContatoService(_mockContatoRepository.Object);
        }

        [Test]
        public async Task Deve_Retornar_Lista_De_Contatos()
        {
            // Arrange
            var contatos = _mockListaDeContatos
                .Select(contatoDto => (Contato)contatoDto);

            _mockContatoRepository
                .Setup(x => x.RetornarListaDeContatos(null))
                .Returns(Task.FromResult(new Tuple<IEnumerable<Contato>, bool>(contatos, false)));

            // Act
            var result = await _contatoService.RetornarListaDeContatos(null);

            // Assert
            Assert.IsNotNull(contatos);
            Assert.That(contatos.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task Deve_Retornar_Contato_Por_Id()
        {
            // Arrange
            var contato = _mockListaDeContatos.First();
            _mockContatoRepository
                .Setup(x => x.RetornarContatoPeloId(contato.Id))
                .Returns(Task.FromResult(contato));

            // Act
            var contatoRetornado = await _contatoService.RetornarContatoPeloId(contato.Id);

            // Assert
            Assert.IsNotNull(contatoRetornado);
            Assert.AreEqual(contato.Id, contatoRetornado.Id);
        }
    }
}
