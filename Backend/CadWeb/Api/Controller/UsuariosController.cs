using CadWeb.Dados;
using CadWeb.Dados.DataContexts;
using CadWeb.Dominio;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;

namespace Api.Controller
{
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {

        private CadWebDataContext db = new CadWebDataContext();

        [Route("usuarios")]
        public HttpResponseMessage GetUsuarios() {

            var result = db.Usuarios.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("usuarios/{id}")]
        public HttpResponseMessage GetUsuariosById(int id)
        {

            var result = db.Usuarios.Where(x => x.Id == id).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("usuarios/{nome}/{senha}")]
        public HttpResponseMessage GetUsuariosByNome(string nome, string senha)
        {

            var result = db.Usuarios.Where(x => x.Nome == nome && x.Senha == senha ).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("usuarios")]
        public HttpResponseMessage PosUsuario(Usuario usuario)
        {
            if (usuario == null) {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                var result = usuario;
                return Request.CreateResponse(HttpStatusCode.Created, result);

            }
            catch {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção do usuario");
            }
           
        }

        [Route("usuario/teste")]
        public HttpResponseMessage GetUsuariosTeste(string teste)
        {

            var result = new
            {
                hora = 17,
                minuto = 12,
                segundo = 25
            };
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuarios.Count(e => e.Id == id) > 0;
        }
      
    }
}
