using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadWeb.Dominio
{
    public class AtividadeProjetoView
    {
        public AtividadeProjeto AtividadeProjeto { get; set; }
        public List<ParticipanteAtividadeProjeto> ParticipantesAtividade { get; set; }
    }
}
