using System;
namespace ApiProdutos.Exceptions
{
    public class CpfInvalidoException : Exception 
    {
        public CpfInvalidoException() : base("CPF inválido") { }
        public CpfInvalidoException(string message) : base(message) { }
        public CpfInvalidoException(string message, Exception inner): base(message, inner) { }
    }
}
