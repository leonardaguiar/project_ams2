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
    class ParticipanteProjetoMap : EntityTypeConfiguration<ParticipanteProjeto>
    {
        public ParticipanteProjetoMap()
        {
            ToTable("ParticipantesProjeto");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(x => x.Projeto);
            HasOptional(x => x.Docente);
            HasOptional(x => x.Discente);
            Property(x => x.TipoParticipante);
            Property(x => x.Coordenador);
            Property(x => x.Comissao);            

        }
    }
}
