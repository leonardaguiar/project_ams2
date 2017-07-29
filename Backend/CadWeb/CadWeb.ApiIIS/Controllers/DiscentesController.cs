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
    public class DiscentesController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();

        [Authorize()]
        [Route("discentes")]
        public HttpResponseMessage GetDiscentes()
        {
            var results = db.Discente.ToList();
            //foreach (var result in results)
            //{
            //  result.Senha = encrypt.Encryptword(result.Senha);
            //  user.Add(result);

            //}
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Authorize()]
        [Route("discentes/{id:int}")]
        public HttpResponseMessage GetDiscentesById(int id)
        {
            var results = db.Discente.Where(x => x.Id == id);
            //foreach (var result in results)
            //{
            //   result.Senha = encrypt.Encryptword(result.Senha);
            //   user = result;                 

            //}

            return Request.CreateResponse(HttpStatusCode.OK, results);

        }
        [Authorize()]
        [Route("discentes/filter/{matricula?}")]
        public HttpResponseMessage GetFilterMatricula(string matricula)
        {

            var result = db.Discente.Where(x => x.Matricula.Equals(matricula));

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Authorize()]
        [Route("discentes/filter/{cpf?}")]
        public HttpResponseMessage GetFilterCPF(string cpf)
        {

            var result = db.Discente.Where(x => x.CPF.Equals(cpf));

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Authorize()]
        [HttpPost]
        [Route("discentes")]
        public HttpResponseMessage PostDiscente(Discente discente)
        {

            if (discente == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
          
            try
            {                
                db.Discente.Add(discente);
                db.SaveChanges();
                var result = discente;
                return Request.CreateResponse(HttpStatusCode.Created, result);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção do discente");
            }

        }
        [Authorize()]
        [HttpPatch]
        [Route("discentes")]
        public HttpResponseMessage PathDiscente(Discente discente)
        {
            if (discente == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Entry<Discente>(discente).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var result = discente;
                return Request.CreateResponse(HttpStatusCode.OK, "Discente alterado com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha durante a alteração do cadastro do discente");
            }

        }
        [Authorize()]
        [HttpPut]
        [Route("discentes")]
        public HttpResponseMessage PutDiscente(Discente discente)
        {
            if (discente == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Entry<Discente>(discente).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var result = discente;
                return Request.CreateResponse(HttpStatusCode.Created, result);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro de discente");
            }

        }
        [Authorize()]
        [HttpDelete]
        [Route("discentes")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Discente.Remove(db.Discente.Find(id));
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Discente excluido");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir discente");
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

        private bool DiscenteExists(int id)
        {
            return db.Discente.Count(e => e.Id == id) > 0;
        }
    }
}
