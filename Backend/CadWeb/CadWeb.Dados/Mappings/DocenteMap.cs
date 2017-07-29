using CadWeb.Dominio;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CadWeb.Dados.Mappings
{
    public class DocenteMap : EntityTypeConfiguration<Docente>
    {
        public DocenteMap()
        {
            ToTable("Docente");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).HasMaxLength(200).IsRequired();
            Property(x => x.Matricula).HasMaxLength(40);
            Property(x => x.CPF);
            Property(x => x.Endereco);
            Property(x => x.Email).HasMaxLength(200);
            Property(x => x.Status);
            
        }
    }
}
