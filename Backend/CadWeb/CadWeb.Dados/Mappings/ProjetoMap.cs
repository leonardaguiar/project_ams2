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
    class ProjetoMap :  EntityTypeConfiguration<Projeto>
    {
        public ProjetoMap()
        {
            ToTable("Projetos");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).HasMaxLength(200).IsRequired();
            Property(x => x.Descricao);
            Property(x => x.Tipo);
            Property(x => x.Status);
            Property(x => x.Semestre).HasMaxLength(100);
            Property(x => x.Data_Inicio);
            Property(x => x.Data_fim);
        }
    }
}
