using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace ApiProdutos.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }
                
    }
}
