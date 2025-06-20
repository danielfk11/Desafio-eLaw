using AutoMapper;
using ClienteApi.DTOs;
using ClienteApi.Models;
using ClienteApi.Repositories;
using Microsoft.Extensions.Logging;

namespace ClienteApi.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper, ILogger<ClienteService> logger)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ClienteDto>> ObterTodosAsync()
        {
            _logger.LogInformation("Buscando todos os clientes...");
            var clientes = await _clienteRepository.ObterTodosAsync();
            _logger.LogInformation("Total de clientes encontrados: {Count}", clientes.Count);
            return _mapper.Map<List<ClienteDto>>(clientes);
        }

        public async Task<ClienteDto?> ObterPorIdAsync(Guid id)
        {
            _logger.LogInformation("Buscando cliente com ID: {Id}", id);
            var cliente = await _clienteRepository.ObterPorIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente com ID {Id} não encontrado.", id);
                return null;
            }

            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<(bool sucesso, string? erro, ClienteDto? clienteCriado)> CriarAsync(ClienteCreateDto clienteDto)
        {
            _logger.LogInformation("Tentando criar cliente com email: {Email}", clienteDto.Email);

            var existente = await _clienteRepository.ObterPorEmailAsync(clienteDto.Email);
            if (existente != null)
            {
                _logger.LogWarning("Email {Email} já está cadastrado.", clienteDto.Email);
                return (false, "Email já cadastrado.", null);
            }

            var cliente = _mapper.Map<Cliente>(clienteDto);

            if (!cliente.EhValido())
            {
                _logger.LogWarning("Dados inválidos para criação do cliente: {@Cliente}", clienteDto);
                return (false, "Dados inválidos.", null);
            }

            await _clienteRepository.AdicionarAsync(cliente);
            _logger.LogInformation("Cliente criado com sucesso. ID: {Id}", cliente.Id);

            var clienteDtoRetorno = _mapper.Map<ClienteDto>(cliente);
            return (true, null, clienteDtoRetorno);
        }


        public async Task<(bool sucesso, string? erro)> AtualizarAsync(Guid id, ClienteDto clienteDto)
        {
            _logger.LogInformation("Tentando atualizar cliente ID: {Id}", id);
            var clienteExistente = await _clienteRepository.ObterPorIdAsync(id);
            if (clienteExistente == null)
            {
                _logger.LogWarning("Cliente com ID {Id} não encontrado para atualização.", id);
                return (false, "Cliente não encontrado.");
            }

            clienteExistente.Nome = clienteDto.Nome;
            clienteExistente.Email = clienteDto.Email;
            clienteExistente.Telefone = clienteDto.Telefone;
            clienteExistente.Endereco = _mapper.Map<Endereco>(clienteDto.Endereco);

            if (!clienteExistente.EhValido())
            {
                _logger.LogWarning("Dados inválidos para atualização do cliente ID {Id}.", id);
                return (false, "Dados inválidos.");
            }

            await _clienteRepository.AtualizarAsync(clienteExistente);
            _logger.LogInformation("Cliente ID {Id} atualizado com sucesso.", id);
            return (true, null);
        }

        public async Task<bool> RemoverAsync(Guid id)
        {
            _logger.LogInformation("Tentando remover cliente ID: {Id}", id);
            var cliente = await _clienteRepository.ObterPorIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente ID {Id} não encontrado para remoção.", id);
                return false;
            }

            await _clienteRepository.RemoverAsync(id);
            _logger.LogInformation("Cliente ID {Id} removido com sucesso.", id);
            return true;
        }
    }
}
