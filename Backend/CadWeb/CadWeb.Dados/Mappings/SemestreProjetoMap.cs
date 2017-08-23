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
   public class SemestreProjetoMap : EntityTypeConfiguration<SemestreProjeto>
    {
        public SemestreProjetoMap()
        {
            ToTable("SemestresProjeto");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(x => x.Projeto);
            Property(x => x.Ano);
            Property(x => x.Semestre);
        }
    }
}
