using Amazon.SQS;
using IOB___Controle_de_Saldo_Bancario.DTO;
using IOB___Controle_de_Saldo_Bancario.Enum;
using IOB___Controle_de_Saldo_Bancario.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IOB___Controle_de_Saldo_Bancario.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContasBancariasController : ControllerBase
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IServicoBancario _servicoBancario;
        private readonly string _queueUrl;
        private readonly ILogger<ContasBancariasController> _logger;

        public ContasBancariasController(IServicoBancario servicoBancario, IConfiguration configuration, IAmazonSQS sqsClient, ILogger<ContasBancariasController> logger)
        {
            _servicoBancario = servicoBancario;
            _queueUrl = configuration.GetValue<string>("SqsSettings:QueueUrl");
            _sqsClient = sqsClient;
            _logger = logger;
        }

        [HttpPost("criar/{nome}")]
        public async Task<IActionResult> CriarConta(string nome)
        {
            try
            {
                await _servicoBancario.CriarContaBancariaAsync(nome);
                return Ok("Conta criada com sucesso!");
            }
            catch
            {
                return BadRequest("Não foi possível criar uma nova conta.");
            }
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarContas()
        {
            try
            {
                var ContasBancarias = await _servicoBancario.ListarContasBancariasAsync();
                return Ok(ContasBancarias.Select(conta => new ContasBancariasDTO { Id = conta.Id, Nome = conta.Name }));
            }
            catch
            {
                return BadRequest("Não foi possível recuperar as contas.");
            }
        }

        [HttpGet("saldo/{contaId}")]
        public async Task<IActionResult> ConsultarSaldo(int contaId)
        {
            try
            {
                var saldo = await _servicoBancario.ConsultarSaldoAsync(contaId);
                return Ok(saldo);
            }
            catch
            {
                return BadRequest("Não foi possível recuperar o saldo.");
            }
        }

        [HttpGet("extrato/{contaId}")]
        public async Task<IActionResult> ListarTransacoes(int contaId)
        {
            try
            {
                var transacoes = await _servicoBancario.ListarTransacoesAsync(contaId);

                return Ok(transacoes.Select(item => new HistoricoTransacaoDTO()
                {
                    Id = item.Id,
                    Tipo = item.OperationType,
                    Valor = item.Value,
                    Data = item.CreateDate.ToString("dd/MM/yyyy, HH:mm:ss")
                }));
            }
            catch
            {
                return BadRequest("Não foi possível recuperar as transações.");
            }
        }

        [HttpPut("creditar/{contaId}/{valor}")]
        public async Task<IActionResult> Creditar(int contaId, decimal valor)
        {

            try
            {
                await EnviarParaFilaSQS(valor, OperationTypeEnum.Credit, contaId);
                return Ok("Crédito realizado com sucesso!");
            }
            catch
            {
                return BadRequest("Não foi possível realizar o crédito.");
            }
        }

        [HttpPut("debitar/{contaId}/{valor}")]
        public async Task<IActionResult> Debitar(int contaId, decimal valor)
        {
            try
            {
                await EnviarParaFilaSQS(valor, OperationTypeEnum.Debit, contaId);
                return Ok("Débito realizado com sucesso!");
            }
            catch
            {
                return BadRequest("Não foi possível realizar o débito.");
            }
        }

        [HttpPut("transferir")]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaDto dto)
        {
            try
            {
                await EnviarParaFilaSQS(dto.Valor, OperationTypeEnum.Transfer, dto.ContaDestinoId, dto.ContaOrigemId);
                return Ok("Transferência realizada com sucesso!");
            }
            catch
            {
                return BadRequest("Não foi possível realizar a transferência.");
            }
        }

        private async Task EnviarParaFilaSQS(decimal valor, Enum.OperationTypeEnum tipoOperacao, int contaDestino, int contaOrigem = 0)
        {
            try
            {
                var mensagemEnvio = new SqsMessageDTO
                {
                    TipoTransacao = tipoOperacao,
                    ContaDestinoId = contaDestino,
                    ContaOrigemId = contaOrigem,
                    Valor = valor
                };

                await _sqsClient.SendMessageAsync(_queueUrl, JsonConvert.SerializeObject(mensagemEnvio));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar mensagem para a fila SQS. TipoTransacao: {TipoTransacao}, Valor: {valor}, Conta destino: {contaDestino}, Conta origem: {contaOrigem}", tipoOperacao, valor, contaDestino, contaOrigem);
                throw new Exception("Erro ao enviar mensagem para a fila", ex);
            }
        }
    }
}
