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
   public class SetorMap : EntityTypeConfiguration<Setor>
    {
        public SetorMap()
        {
            ToTable("Setor");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).HasMaxLength(200).IsRequired();
            Property(x => x.Responsavel).HasMaxLength(200);
          
        }
    }
}
