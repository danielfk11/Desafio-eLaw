using ClienteApi.DTOs;
using ClienteApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClienteApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(ClienteService clienteService, ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Recebida solicitação GET para listar todos os clientes.");
            var clientes = await _clienteService.ObterTodosAsync();
            _logger.LogInformation("Total de clientes encontrados: {Count}", clientes.Count());
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Recebida solicitação GET para cliente com ID: {Id}", id);
            var cliente = await _clienteService.ObterPorIdAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente com ID {Id} não encontrado.", id);
                return NotFound();
            }

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteCreateDto clienteDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("POST inválido para criação de cliente. ModelState inválido.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Recebida solicitação POST para criar cliente com email: {Email}", clienteDto.Email);
            var (sucesso, erro, clienteCriado) = await _clienteService.CriarAsync(clienteDto);

            if (!sucesso)
            {
                _logger.LogWarning("Falha ao criar cliente: {Erro}", erro);
                return Conflict(new { mensagem = erro });
            }

            _logger.LogInformation("Cliente criado com sucesso: {Id}", clienteCriado.Id);
            return CreatedAtAction(nameof(GetById), new { id = clienteCriado.Id }, clienteCriado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ClienteDto clienteDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("PUT inválido para cliente ID {Id}. ModelState inválido.", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Recebida solicitação PUT para atualizar cliente ID: {Id}", id);
            var (sucesso, erro) = await _clienteService.AtualizarAsync(id, clienteDto);

            if (!sucesso)
            {
                _logger.LogWarning("Falha ao atualizar cliente ID {Id}: {Erro}", id, erro);
                return NotFound(new { mensagem = erro });
            }

            _logger.LogInformation("Cliente ID {Id} atualizado com sucesso.", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Recebida solicitação DELETE para cliente ID: {Id}", id);
            var sucesso = await _clienteService.RemoverAsync(id);

            if (!sucesso)
            {
                _logger.LogWarning("Falha ao remover cliente ID {Id}: cliente não encontrado.", id);
                return NotFound();
            }

            _logger.LogInformation("Cliente ID {Id} removido com sucesso.", id);
            return NoContent();
        }
    }
}
