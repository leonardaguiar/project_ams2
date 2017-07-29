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
    public class DocentesController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();

        [Authorize()]
        [Route("docentes")]
        public HttpResponseMessage Getdocentes ()
        {
            var results = db.Docente.ToList();
            //foreach (var result in results)
            //{
            //  result.Senha = encrypt.Encryptword(result.Senha);
            //  user.Add(result);

            //}
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
        [Authorize()]
        [Route("docentes/{id:int}")]
        public HttpResponseMessage GetDocentesById(int id)
        {
            var results = db.Docente.Where(x => x.Id == id);
            //foreach (var result in results)
            //{
            //   result.Senha = encrypt.Encryptword(result.Senha);
            //   user = result;                 

            //}

            return Request.CreateResponse(HttpStatusCode.OK, results);

        }
        [Authorize()]
        [Route("docentes/filter/{matricula?}")]
        public HttpResponseMessage GetFilterMatricula(string matricula)
        {

            var result = db.Docente.Where(x => x.Matricula.Equals(matricula));

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Authorize()]
        [Route("docentes/filter/{cpf?}")]
        public HttpResponseMessage GetFilterCPF(string cpf)
        {

            var result = db.Docente.Where(x => x.CPF.Equals(cpf));

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Authorize()]
        [HttpPost]
        [Route("docentes")]
        public HttpResponseMessage PostDiscente(Docente docente)
        {

            if (docente == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Docente.Add(docente);
                db.SaveChanges();
                var result = docente;
                return Request.CreateResponse(HttpStatusCode.Created, result);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção do docente");
            }

        }
        [Authorize()]
        [HttpPatch]
        [Route("docentes")]
        public HttpResponseMessage PathDiscente(Docente docente)
        {
            if (docente == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Entry<Docente>(docente).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var result = docente;
                return Request.CreateResponse(HttpStatusCode.OK, "Docente alterado com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha durante a alteração do cadastro do docente");
            }

        }
        [Authorize()]
        [HttpPut]
        [Route("docentes")]
        public HttpResponseMessage PutDiscente(Docente docente)
        {
            if (docente == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Entry<Docente>(docente).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                //usuario.Senha = encrypt.Encryptword(usuario.Senha);
                var result = docente;
                return Request.CreateResponse(HttpStatusCode.Created, result);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro de docente");
            }

        }
        [Authorize()]
        [HttpDelete]
        [Route("docentes")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Docente.Remove(db.Docente.Find(id));
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Docente excluido");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir docente");
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

        private bool DocenteExists(int id)
        {
            return db.Docente.Count(e => e.Id == id) > 0;
        }
    }
}
