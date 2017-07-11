using CadWeb.Dados.Mappings;
using CadWeb.Dominio;
using System.Data.Entity;

namespace CadWeb.Dados.DataContexts
{
    public class CadWebDataContext : DbContext
    {
        public CadWebDataContext() : base("CadWebConnectionString")
        {
            Database.SetInitializer<CadWebDataContext>(new CadWebDataContextsInitializer());
        }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UsuarioMap());
            base.OnModelCreating(modelBuilder);
        }
    }
    public class CadWebDataContextsInitializer : DropCreateDatabaseIfModelChanges<CadWebDataContext> {

        protected override void Seed(CadWebDataContext context)
        {
            
            base.Seed(context);
        }
    }
}
