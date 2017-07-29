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
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("Usuario");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).HasMaxLength(200).IsRequired();
            Property(x => x.SobreNome).HasMaxLength(200).IsRequired();
            Property(x => x.Email).HasMaxLength(200).IsRequired();
            Property(x => x.Senha).IsRequired();
        }
    }
}
