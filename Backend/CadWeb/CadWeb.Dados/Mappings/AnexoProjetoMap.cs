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
    public class AnexoProjetoMap : EntityTypeConfiguration<AnexoProjeto>
    {
        public AnexoProjetoMap()
        {
            ToTable("AnexosProjeto");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(x => x.Projeto);
            Property(x => x.Descricao).HasMaxLength(200);
            Property(x => x.Link);
            Property(x => x.Data);
        }
                
    }
}
