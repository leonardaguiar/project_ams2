using CadWeb.ApiIIS.Utils;
using CadWeb.Dados.DataContexts;
using CadWeb.Dominio;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CadWeb.ApiIIS.Controllers
{
    
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1")]
    public class UsuariosController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();

            [Authorize()]
            [Route("usuarios")]
            public HttpResponseMessage GetUsuarios()
            {
              Encrypt encrypt = new Encrypt();
              List<Usuario> user = new List<Usuario>();
              var results = db.Usuarios.ToList();
              //foreach (var result in results)
              //{
              //  result.Senha = encrypt.Encryptword(result.Senha);
              //  user.Add(result);

              //}
            return Request.CreateResponse(HttpStatusCode.OK, results);
            }
            [Authorize()]
            [Route("usuarios/{id:int}")]
            public HttpResponseMessage GetUsuariosById(int id)
            {
                Encrypt encrypt = new Encrypt();
                Usuario user = new Usuario();                 
                
                var results = db.Usuarios.Where(x => x.Id == id);
                //foreach (var result in results)
                //{
                //   result.Senha = encrypt.Encryptword(result.Senha);
                //   user = result;                 
                  
                //}

               return Request.CreateResponse(HttpStatusCode.OK, results);

        }
             [Authorize()]
             [Route("usuarios/getusuarioemail/{email?}")]
            public HttpResponseMessage GetUsuariosByEmail(string email)
            {
                ValuesController values = new ValuesController();

            if (!email.Equals(values.Get().ToString())) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Acesso ao recurso negado");
            }
                var result = db.Usuarios.Where(x => x.Email.Equals(email));

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            [HttpPost]
            [Route("usuarios")]
            public HttpResponseMessage PostUsuario(Usuario usuario)
            {
             
            if (usuario == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            var result2 = db.Usuarios.Where(x => x.Email.Equals(usuario.Email)).ToList();
            if (result2.Count>0)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "Usuário já existe");

            }

            try
                {
                    Encrypt encrypt = new Encrypt();
                    usuario.Senha = encrypt.RetornarMD5(usuario.Senha);
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    var result = usuario;
                    return Request.CreateResponse(HttpStatusCode.Created, result);

                }
                catch
                {

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção do usuario");
                }

            }
        [Authorize()]
        [HttpPatch]
        [Route("usuarios")]
        public HttpResponseMessage PathUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {   
                db.Entry<Usuario>(usuario).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var result = usuario;
                return Request.CreateResponse(HttpStatusCode.OK, "Usuario alterado com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha durante a alteração do cadastro e usuario");
            }

        }
        [Authorize()]
        [HttpPut]
        [Route("usuarios")]
        public HttpResponseMessage PutUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                //Verifica se houve a tentativa de alerar a senha
                //if (db.Usuarios.Where(x => x.Id  == usuario.Id && x.Senha.Equals(usuario.Senha)).ToList().Count()<=0) {
                //    return Request.CreateResponse(HttpStatusCode.NotModified, "Impossivel alterar a senha");
                //}

                var values = db.Usuarios.Where(x => x.Id == usuario.Id);

                //Define a senha com a mesma senha já salva no banco
                foreach (var value in values) {
                    usuario.Senha = value.Senha;
                }
                //Encrypt encrypt = new Encrypt();
                //usuario.Senha = encrypt.RetornarMD5(usuario.Senha);
                db.Entry<Usuario>(usuario).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                //usuario.Senha = encrypt.Encryptword(usuario.Senha);
                var result = usuario;
                return Request.CreateResponse(HttpStatusCode.OK, "Usuario alterado com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro de usuario");
            }

        }
        [Authorize()]
        [HttpDelete]
        [Route("usuarios")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Usuarios.Remove(db.Usuarios.Find(id));
                db.SaveChanges();
           
                return Request.CreateResponse(HttpStatusCode.OK, "Usuario excluido");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir usuario");
            }

        }

        
        [Route("usuarios/teste")]
            public HttpResponseMessage GetTeste()
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