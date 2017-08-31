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
            //Database.Connection.ConnectionString = "Data Source=localhost\\Expert;Initial Catalog=cadweb;User ID=sa;Password=gehmni;";
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            //Database.SetInitializer<CadWebDataContext>(new CadWebDataContextsInitializer());
        }
       public IDbSet<Usuario> Usuarios { get; set; }
       public IDbSet<Setor> Setor { get; set; }
       public IDbSet<Discente> Discente { get; set; }
       public IDbSet<Docente> Docente { get; set; }
       public IDbSet<Projeto> Projeto { get; set; }
       public IDbSet<TarefaProjeto> TarefasProjeto { get; set; }
       public IDbSet<AtividadeProjeto> AtividadesProjeto { get; set; }
       public IDbSet<ParticipanteProjeto> ParticipantesProjeto  { get; set; }
       public IDbSet<ParticipanteAtividadeProjeto> ParticipantesAtividadeProjeto { get; set; }
       public IDbSet<ParticipanteTarefaProjeto> ParticipantesTarefaProjeto { get; set; }
       public IDbSet<SemestreProjeto> SemestresProjeto { get; set; }
       public IDbSet<AnexoProjeto> AnexosProjeto { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new SetorMap());
            modelBuilder.Configurations.Add(new DocenteMap());
            modelBuilder.Configurations.Add(new DiscenteMap());
            modelBuilder.Configurations.Add(new ProjetoMap());
            modelBuilder.Configurations.Add(new TarefaProjetoMap());
            modelBuilder.Configurations.Add(new AtividadeProjetoMap());
            modelBuilder.Configurations.Add(new ParticipanteProjetoMap());
            modelBuilder.Configurations.Add(new ParticipanteTarefaProjetoMap());
            modelBuilder.Configurations.Add(new ParticipanteAtividadeProjetoMap());
            modelBuilder.Configurations.Add(new SemestreProjetoMap());
            base.OnModelCreating(modelBuilder);
        }
    }
    public class CadWebDataContextsInitializer : DropCreateDatabaseIfModelChanges<CadWebDataContext>
    {

        protected override void Seed(CadWebDataContext context)
        {
            //context.Usuarios.Add(new Usuario {Id = 0, Nome = "Leonardo", SobreNome= "Aguiar", Email ="leo.aguiar.silva@gmail.com", IsAtivo = true, Senha = "1234" });
            //context.SaveChanges();
            base.Seed(context);
        }
    }
}

