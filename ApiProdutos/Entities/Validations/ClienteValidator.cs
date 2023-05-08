using ApiProdutos.Exceptions;
using FluentValidation;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ApiProdutos.Entities.Validations
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {

        private readonly HttpClient _httpClient;

        public async Task<bool> ValidarCep(Cliente cliente, string cep)
        {
            try
            {

                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(content))
                {
                    return false;

                }
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                cliente.Endereco = result.logradouro;
                cliente.Complemento = result.complemento;
                cliente.Bairro = result.bairro;
                cliente.Cidade = result.localidade;
                cliente.Estado = result.uf;

                return true;
                
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Erro ao validar CEP: não foi possível se conectar ao servidor", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao validar CEP", ex);
            }
            ;
        }

        //bug a resolver
        private bool ValidarCpf(string cpf)
        {
            try
            {
                
                 //Verifica se o cpf é nulo ou vazio
                 if (string.IsNullOrEmpty(cpf))
                        return false;         
                //Remove caracteres não numéricos do CPF
                var cpfNumeros = Regex.Replace(cpf, @"[^\d]", string.Empty);

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
                throw new CpfInvalidoException("CPF invalido.", ex);
            }
        }

        public ClienteValidator()
        {

            _httpClient = new HttpClient();
            RuleFor(cliente => cliente.Cpf)
                .NotNull()
                .WithMessage("CPF é obrigatório");
                //.Must(ValidarCpf)
                //.WithMessage("CPF inválido");

            RuleFor(cliente => cliente.Nome)
                .NotNull()
                .WithMessage("Nome é obrigatório");

            RuleFor(cliente => cliente.Email)
                .EmailAddress()
                .When(cliente => !string.IsNullOrEmpty(cliente.Email))
                .WithMessage("E-mail inválido");

            RuleFor(cliente => cliente.Cep)
                .NotNull()
                .WithMessage("CEP é obrigatório");

            RuleFor(cliente => cliente.Cep)
                .MustAsync(async (cliente, cep, cancellation) => await ValidarCep(cliente, cep))
                .When(cliente => !string.IsNullOrEmpty(cliente.Cep))
                .WithMessage("CEP inválido");

        }
    }
}

