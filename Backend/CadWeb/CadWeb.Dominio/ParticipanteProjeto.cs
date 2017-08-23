using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadWeb.Dominio
{
    public class ParticipanteProjeto
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public int? DiscenteId { get; set; }
        public virtual Discente Discente { get; set; }
        public int? DocenteId { get; set; }
        public virtual Docente Docente { get; set; }
        public virtual Projeto Projeto { get; set; }
        //TipoParticipante
        //0 = Discente
        //1 = Doscente
        public int TipoParticipante { get; set; }
        public bool Coordenador { get; set; }
        public bool Comissao { get; set; }
    }

    //public override ToString()
    //{
        
    //}
}
