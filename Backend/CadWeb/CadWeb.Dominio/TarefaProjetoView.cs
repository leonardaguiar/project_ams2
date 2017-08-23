using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadWeb.Dominio
{
    public class TarefaProjetoView
    {
        public TarefaProjeto TarefaProjeto { get; set; }
        public List<ParticipanteTarefaProjeto> ParticipantesTarefa { get; set; }
    }
}
