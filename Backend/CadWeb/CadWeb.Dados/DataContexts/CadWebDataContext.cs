using CadWeb.Dados.Mappings;
using CadWeb.Dominio;
using System.Data.Entity;

namespace CadWeb.Dados.DataContexts
{
    public class CadWebDataContext : DbContext
    {
        public CadWebDataContext() 
        {
            //gohorse2.chfx4nncv5vw.us - west - 2.rds.amazonaws.com
            //Database.Connection.ConnectionString = "Data Source=LEO-PC\\EXPERT;Initial Catalog=cadweb;Integrated Security=True;";
            Database.Connection.ConnectionString = "Data Source=gohorse2.chfx4nncv5vw.us-west-2.rds.amazonaws.com;Initial Catalog=cadweb;User ID=root;Password=11072017;";
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            //Database.SetInitializer<CadWebDataContext>(new CadWebDataContextsInitializer());
        }
       public IDbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UsuarioMap());
            base.OnModelCreating(modelBuilder);
        }
    }
    public class CadWebDataContextsInitializer : DropCreateDatabaseIfModelChanges<CadWebDataContext>
    {

        protected override void Seed(CadWebDataContext context)
        {
            context.Usuarios.Add(new Usuario { Id = 1, Nome = "Leonardo", IsAtivo = true, Senha = "1234" });
            context.SaveChanges();
            base.Seed(context);
        }
    }
}

