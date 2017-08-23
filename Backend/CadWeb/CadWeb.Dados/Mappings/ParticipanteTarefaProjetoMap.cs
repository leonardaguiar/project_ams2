using CadWeb.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadWeb.Dados.Mappings
{
    public class ParticipanteTarefaProjetoMap : EntityTypeConfiguration<ParticipanteTarefaProjeto>
    {
        public ParticipanteTarefaProjetoMap()
        {
            ToTable("ParticipantesTarefaProjeto");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasOptional(x => x.Docente);
            HasOptional(x => x.Discente);
            Property(x => x.TipoParticipante);
        
        }
    }
}
