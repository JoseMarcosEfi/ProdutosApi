using ApiProdutos.Data;
using ApiProdutos.Entities;
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
        public ProdutosController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var produtos = _dbcontext.Produtos.ToList();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var produto = _dbcontext.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {
            if (produto == null)
            {
                return BadRequest();
            }

            _dbcontext.Produtos.Add(produto);
            _dbcontext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Produto produto)
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

            dbProduto.Nome = produto.Nome;
            dbProduto.Preco = produto.Preco;

            _dbcontext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
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


    }
}
