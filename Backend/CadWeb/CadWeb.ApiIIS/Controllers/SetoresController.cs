using CadWeb.Dados.DataContexts;
using CadWeb.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CadWeb.ApiIIS.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1")]
    public class SetoresController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();

        [Authorize()]
        [Route("setores")]
        public HttpResponseMessage GetSetors()
        {
            var results = db.Setor.ToList();
            //foreach (var result in results)
            //{
            //  result.Senha = encrypt.Encryptword(result.Senha);
            //  user.Add(result);

            //}
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
        [Authorize()]
        [Route("setores/{id:int}")]
        public HttpResponseMessage GetSetorsById(int id)
        {
            var results = db.Setor.Where(x => x.Id == id);
            //foreach (var result in results)
            //{
            //   result.Senha = encrypt.Encryptword(result.Senha);
            //   user = result;                 

            //}

            return Request.CreateResponse(HttpStatusCode.OK, results);

        }
        //[Authorize()]
        //[Route("usuarios/getusuarioemail/{email?}")]
        //public HttpResponseMessage GetUsuariosByEmail(string email)
        //{

        //    var result = db.Usuarios.Where(x => x.Email.Equals(email));

        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
        [Authorize()]
        [HttpPost]
        [Route("setores")]
        public HttpResponseMessage PostSetor(Setor setor)
        {

            if (setor == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Setor.Add(setor);
                db.SaveChanges();
                var result = setor;
                return Request.CreateResponse(HttpStatusCode.Created, result);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção do setor");
            }

        }
        [Authorize()]
        [HttpPatch]
        [Route("setores")]
        public HttpResponseMessage PathSetor(Setor setor)
        {
            if (setor == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Entry<Setor>(setor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var result = setor;
                return Request.CreateResponse(HttpStatusCode.OK, "Setor alterado com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha durante a alteração do cadastro do setor");
            }

        }
        [Authorize()]
        [HttpPut]
        [Route("setores")]
        public HttpResponseMessage PutSetor(Setor setor)
        {
            if (setor == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Entry<Setor>(setor).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                //usuario.Senha = encrypt.Encryptword(usuario.Senha);
                var result = setor;
                return Request.CreateResponse(HttpStatusCode.Created, result);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro de setor");
            }

        }
        [Authorize()]
        [HttpDelete]
        [Route("setores")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Setor.Remove(db.Setor.Find(id));
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Setor excluido");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir setor");
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

        private bool SetorExists(int id)
        {
            return db.Setor.Count(e => e.Id == id) > 0;
        }
    }
}
