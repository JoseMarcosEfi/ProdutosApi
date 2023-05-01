using ApiProdutos.Data;
using ApiProdutos.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ApiProdutos.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IValidator<Produto> _produtoValidator;
        public ProdutosController(ApplicationDbContext dbcontext, IValidator<Produto> produtoValidator)
        {
            _dbcontext = dbcontext;
            _produtoValidator = produtoValidator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try {
                var produtos = _dbcontext.Produtos.ToList();
                return Ok(produtos);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try {
                var produto = _dbcontext.Produtos.Find(id);

                if (produto == null)
                {
                    return NotFound();
                }

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {
            try {
                if (produto == null)
                {
                    return BadRequest();
                }
                var validationResult = _produtoValidator.Validate(produto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                _dbcontext.Produtos.Add(produto);
                _dbcontext.SaveChanges();

                return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Produto produto)
        {
            try
            {
                if (produto == null || produto.Id != id)
                {
                    return BadRequest();
                }
                var dbProduto = _dbcontext.Produtos.Find(id);

                if (dbProduto == null)
                {
                    return NotFound();
                }
                var validationResult = _produtoValidator.Validate(produto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                dbProduto.Nome = produto.Nome;
                dbProduto.Preco = produto.Preco;

                _dbcontext.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var dbProduto = _dbcontext.Produtos.Find(id);

                if (dbProduto == null)
                {
                    return NotFound();
                }

                _dbcontext.Produtos.Remove(dbProduto);
                _dbcontext.SaveChanges();



                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }


    }
}
