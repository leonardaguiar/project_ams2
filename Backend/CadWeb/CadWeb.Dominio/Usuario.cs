using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadWeb.Dominio
{
   public class Usuario
    {
       
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public bool IsAtivo { get; set; }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
