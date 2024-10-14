using IOB___Controle_de_Saldo_Bancario.DTO;
using IOB___Controle_de_Saldo_Bancario.Service;
using Microsoft.AspNetCore.Mvc;

namespace IOB___Controle_de_Saldo_Bancario.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContasBancariasController : ControllerBase
    {
        private readonly IServicoBancario _servicoBancario;

        public ContasBancariasController(IServicoBancario servicoBancario)
        {
            _servicoBancario = servicoBancario;
        }

        [HttpPost("criar/{nome}")]
        public async Task<IActionResult> CriarConta(string nome)
        {
            await _servicoBancario.CriarContaBancariaAsync(nome);
            return Ok("Conta criada com sucesso!");
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarContas()
        {
            var ContasBancarias = await _servicoBancario.ListarContasBancariasAsync();
            return Ok(ContasBancarias.Select(conta => new ContasBancariasDTO { Id = conta.Id, Nome = conta.Name }));
        }

        [HttpPut("creditar/{contaId}/{valor}")]
        public async Task<IActionResult> Creditar(int contaId, decimal valor)
        {
            await _servicoBancario.CreditarAsync(contaId, valor);
            return Ok("Crédito realizado com sucesso!");
        }

        [HttpPut("debitar/{contaId}/{valor}")]
        public async Task<IActionResult> Debitar(int contaId, decimal valor)
        {
            await _servicoBancario.DebitarAsync(contaId, valor);
            return Ok("Débito realizado com sucesso!");
        }

        [HttpGet("saldo/{contaId}")]
        public async Task<IActionResult> ConsultarSaldo(int contaId)
        {
            var saldo = await _servicoBancario.ConsultarSaldoAsync(contaId);
            return Ok(saldo);
        }

        [HttpGet("extrato/{contaId}")]
        public async Task<IActionResult> ListarTransacoes(int contaId)
        {
            var transacoes = await _servicoBancario.ListarTransacoesAsync(contaId);

            return Ok(transacoes.Select(item => new HistoricoTransacaoDTO()
            {
                Id = item.Id,
                Tipo = item.OperationType.ToString(),
                Valor = item.Value,
                Data = item.CreateDate.ToString("dd/MM/yyyy, HH:mm:ss")
            }));
        }

        [HttpPut("transferir")]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaDto dto)
        {
            await _servicoBancario.TransferirAsync(dto.ContaOrigemId, dto.ContaDestinoId, dto.Valor);
            return Ok("Transferência realizada com sucesso!");
        }
    }
}
