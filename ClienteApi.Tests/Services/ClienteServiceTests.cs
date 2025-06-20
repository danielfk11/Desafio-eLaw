using AutoMapper;
using ClienteApi.DTOs;
using ClienteApi.Models;
using ClienteApi.Repositories;
using ClienteApi.Services;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;


namespace ClienteApi.Tests.Services
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ClienteService _clienteService;

        public ClienteServiceTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cliente, ClienteDto>().ReverseMap();
                cfg.CreateMap<ClienteCreateDto, Cliente>();
                cfg.CreateMap<Endereco, EnderecoDto>().ReverseMap();
            });

            _mapper = config.CreateMapper();

            _clienteService = new ClienteService(_clienteRepositoryMock.Object, _mapper, Mock.Of<ILogger<ClienteService>>());
        }

        [Fact]
        public async Task CriarAsync_DeveRetornarSucessoQuandoClienteEhValidoENovo()
        {
            var dto = GetClienteCreateDto();
            _clienteRepositoryMock.Setup(r => r.ObterPorEmailAsync(dto.Email)).ReturnsAsync((Cliente?)null);

            var (sucesso, erro, clienteCriado) = await _clienteService.CriarAsync(dto);

            Assert.True(sucesso);
            Assert.Null(erro);
            Assert.NotNull(clienteCriado);
            Assert.Equal(dto.Email, clienteCriado.Email);
            _clienteRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public async Task CriarAsync_DeveFalharQuandoEmailJaExiste()
        {
            var dto = GetClienteCreateDto();
            _clienteRepositoryMock.Setup(r => r.ObterPorEmailAsync(dto.Email)).ReturnsAsync(new Cliente());

            var (sucesso, erro, clienteCriado) = await _clienteService.CriarAsync(dto);

            Assert.False(sucesso);
            Assert.Equal("Email já cadastrado.", erro);
            Assert.Null(clienteCriado);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarListaDeClientes()
        {
            var lista = new List<Cliente> { new Cliente { Nome = "Cliente 1", Email = "a@a.com", Endereco = GetEndereco() } };
            _clienteRepositoryMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(lista);

            var resultado = await _clienteService.ObterTodosAsync();

            Assert.NotNull(resultado);
            Assert.Single(resultado);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarClienteQuandoExiste()
        {
            var cliente = new Cliente { Id = Guid.NewGuid(), Nome = "Teste", Email = "email@email.com", Endereco = GetEndereco() };
            _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(cliente.Id)).ReturnsAsync(cliente);

            var resultado = await _clienteService.ObterPorIdAsync(cliente.Id);

            Assert.NotNull(resultado);
            Assert.Equal(cliente.Nome, resultado?.Nome);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarNullQuandoNaoExiste()
        {
            _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cliente?)null);

            var resultado = await _clienteService.ObterPorIdAsync(Guid.NewGuid());

            Assert.Null(resultado);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarClienteExistente()
        {
            var id = Guid.NewGuid();
            var clienteExistente = new Cliente { Id = id, Nome = "Antigo", Email = "antigo@email.com", Endereco = GetEndereco() };
            var novoDto = GetClienteDto();

            _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync(clienteExistente);

            var (sucesso, erro) = await _clienteService.AtualizarAsync(id, novoDto);

            Assert.True(sucesso);
            Assert.Null(erro);
            _clienteRepositoryMock.Verify(r => r.AtualizarAsync(clienteExistente), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_DeveFalharQuandoClienteNaoExiste()
        {
            _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cliente?)null);

            var (sucesso, erro) = await _clienteService.AtualizarAsync(Guid.NewGuid(), GetClienteDto());

            Assert.False(sucesso);
            Assert.Equal("Cliente não encontrado.", erro);
        }

        [Fact]
        public async Task RemoverAsync_DeveRemoverClienteQuandoExiste()
        {
            var id = Guid.NewGuid();
            var cliente = new Cliente { Id = id, Nome = "Remover", Email = "remover@email.com", Endereco = GetEndereco() };

            _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync(cliente);

            var sucesso = await _clienteService.RemoverAsync(id);

            Assert.True(sucesso);
            _clienteRepositoryMock.Verify(r => r.RemoverAsync(id), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveFalharQuandoClienteNaoExiste()
        {
            _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cliente?)null);

            var sucesso = await _clienteService.RemoverAsync(Guid.NewGuid());

            Assert.False(sucesso);
        }

        private ClienteCreateDto GetClienteCreateDto()
        {
            return new ClienteCreateDto
            {
                Nome = "Daniel Kiffer",
                Email = "daniel@email.com",
                Telefone = "12345678",
                Endereco = new EnderecoDto
                {
                    Rua = "Rua X",
                    Numero = "99",
                    Cidade = "Cidade Y",
                    Estado = "Estado Z",
                    Cep = "00000-000"
                }
            };
        }

        private ClienteDto GetClienteDto()
        {
            return new ClienteDto
            {
                Id = Guid.NewGuid(),
                Nome = "Daniel Kiffer",
                Email = "daniel@email.com",
                Telefone = "12345678",
                Endereco = new EnderecoDto
                {
                    Rua = "Rua X",
                    Numero = "99",
                    Cidade = "Cidade Y",
                    Estado = "Estado Z",
                    Cep = "00000-000"
                }
            };
        }

        private Endereco GetEndereco()
        {
            return new Endereco
            {
                Rua = "Rua A",
                Numero = "123",
                Cidade = "Cidade B",
                Estado = "Estado C",
                Cep = "11111-111"
            };
        }
    }
}
