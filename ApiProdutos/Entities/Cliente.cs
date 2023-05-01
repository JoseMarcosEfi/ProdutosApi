using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Flunt.Validation;
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

        public async Task<bool> ValidarCep()
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{Cep}/json/");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content))
                    {
                        dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                        Endereco = result.logradouro;
                        Complemento = result.complemento;
                        Bairro = result.bairro;
                        Cidade = result.localidade;
                        Estado = result.uf;

                        return true;

                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }                      
        }
        public void Validate() 
        {
            try
            {
                AddNotifications(new Contract()
                    .isTrue(ValidarCpf(), "CPF", "CPF inválido"));
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private bool ValidarCpf()
        {
            try
            {
                //Verifica se o cpf é nulo ou vazio
                if (string.IsNullOrEmpty(Cpf))
                    return false;
                //Remove caracteres não numéricos do CPF
                var cpfNumeros = Regex.Replace(Cpf, @"[^\d]", string.Empty);

                //Verifica se o CPF ctem 11 digitos
                if (cpfNumeros.Length != 11)
                    return false;

                //Calcula os digitos verificadores

                int soma = 0;
                for (int i = 0; i < 9; i++)
                {
                    soma += int.Parse(cpfNumeros[i].ToString()) * (10 - i);
                }
                int primeiroDigitoVerificador = 11 - soma % 11;
                if (primeiroDigitoVerificador > 9)
                {
                    primeiroDigitoVerificador = 0;
                }

                soma = 0;
                for (int i = 0; i < 10; i++)
                {
                    soma += int.Parse(cpfNumeros[i].ToString()) * (11 - i);
                }
                int segundoDigitoVerificador = 11 - soma % 11;
                if (segundoDigitoVerificador > 9)
                {
                    segundoDigitoVerificador = 0;
                }
                //Verifica se os digitos verificados estão corretos
                return cpfNumeros.EndsWith(primeiroDigitoVerificador.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}
