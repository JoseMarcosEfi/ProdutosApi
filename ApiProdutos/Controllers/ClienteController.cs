using ApiProdutos.Data;
using ApiProdutos.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ApiProdutos.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IValidator<Cliente> _clienteValidator;
        public ClienteController(ApplicationDbContext dbcontext, IValidator<Cliente> clienteValidator)
        {
            _dbcontext = dbcontext;
            _clienteValidator = clienteValidator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var clientes = _dbcontext.Clientes
                    .Select(c => new {c.Nome, c.Email})
                    .ToList();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("{cpf}")]
        public IActionResult Get(string cpf)
        {
            try
            {
                var cliente = _dbcontext.Clientes.
                    Where(c => c.Cpf == cpf)
                    .Select(c => new { c.Nome, c.Cep, c.Cidade })
                    .SingleOrDefault();

                if (cliente == null)
                {
                    return NotFound();
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    return BadRequest();
                }
                var validationResult = await _clienteValidator.ValidateAsync(cliente);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                _dbcontext.Clientes.Add(cliente);
                await _dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { cpf = cliente.Cpf }, cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("{cpf}")]
        public async Task<IActionResult> Put(string cpf, [FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null || cliente.Cpf != cpf)
                {
                    return BadRequest();
                }
                var dbCliente = _dbcontext.Clientes.Find(cpf);

                if (dbCliente == null)
                {
                    return NotFound();
                }
                var validationResult = await _clienteValidator.ValidateAsync(cliente);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                dbCliente.Cpf = cliente.Cpf;
                dbCliente.Nome = cliente.Nome;
                dbCliente.Email = cliente.Email;
                dbCliente.Cep = cliente.Cep;
                dbCliente.Estado = cliente.Estado;
                dbCliente.Cidade = cliente.Cidade;
                dbCliente.Endereco = cliente.Endereco;
                dbCliente.Bairro = cliente.Bairro;
                dbCliente.Complemento = cliente.Complemento;
                             
                
                await _dbcontext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("{cpf}")]
        public IActionResult Delete(string cpf)
        {
            try
            {
                var dbCliente = _dbcontext.Clientes.Find(cpf);

                if (dbCliente == null)
                {
                    return NotFound();
                }

                _dbcontext.Clientes.Remove(dbCliente);
                _dbcontext.SaveChanges();



                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


    }
}
