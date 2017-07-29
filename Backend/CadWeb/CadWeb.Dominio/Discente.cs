using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadWeb.Dominio
{
    public class Discente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Matricula { get; set; }
        public bool Status { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
    }
}
