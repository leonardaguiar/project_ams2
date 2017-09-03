using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadWeb.Dominio
{
    public class ProjetoView
    {
        public Projeto Projeto { get; set; }
        //public List<SemestreProjeto> Semestres { get; set; }
        public List<TarefaProjeto> Tarefas { get; set; }
        public List<AtividadeProjeto> Atividades{ get; set; }
        public List<ParticipanteProjeto> ParticipantesProjeto { get; set; }
        //public List<ParticipanteAtividadeProjeto> ParticipantesAtividade { get; set; }
        //public List<ParticipanteTarefaProjeto> ParticipantesTarefa { get; set; }
        public List<AnexoProjeto> AnexosProjeto { get; set; }
    }
}
