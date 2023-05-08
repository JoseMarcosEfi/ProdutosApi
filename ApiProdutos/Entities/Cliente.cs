using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Flunt.Validations;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ApiProdutos.Entities
{
    public class Cliente
    {
        [Key]
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public Cliente(){  }
    }

}
