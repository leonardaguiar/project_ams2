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
    public class SemestresProjetoController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();
        //[Authorize()]
        [Route("semestresprojeto")]
        public HttpResponseMessage GetSemestresProjeto()
        {

            var results = db.SemestresProjeto.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Authorize()]
        [Route("semestresprojeto/{id:int}")]
        public HttpResponseMessage GetSemestresProjetoById(int id)
        {

            var results = db.SemestresProjeto.Where(x => x.Id == id);

            return Request.CreateResponse(HttpStatusCode.OK, results);

        }

        //[Authorize()]
        //[Route("usuarios/getusuarioemail/{email?}")]
        //public HttpResponseMessage GetUsuariosByEmail(string email)
        //{
        //    ValuesController values = new ValuesController();

        //    if (!email.Equals(values.Get().ToString()))
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, "Acesso ao recurso negado");
        //    }
        //    var result = db.Usuarios.Where(x => x.Email.Equals(email));

        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        [HttpPost]
        [Route("semestresprojeto")]
        public HttpResponseMessage PostSemestre(SemestreProjeto semestre)
        {

            if (semestre == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possivel gravar os dados, as informações estão incompletas");
            }

            try
            {

                db.SemestresProjeto.Add(semestre);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created, semestre);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção do SEMESTRE");
            }

        }

        [Authorize()]
        [HttpPut]
        [Route("semestresprojeto")]
        public HttpResponseMessage PutSemestreProjeto(SemestreProjeto semestre)
        {
            if (semestre == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possível alterar o SEMESTRE. Informações incompletas");
            }

            try
            {

                db.Entry<SemestreProjeto>(semestre).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "SEMESTRE alterado com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro do SEMESTRE");
            }
        }

        [Authorize()]
        [HttpDelete]
        [Route("semestresprojeto")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.SemestresProjeto.Remove(db.SemestresProjeto.Find(id));
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Semestre excluído com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir o SEMESTRE");
            }

        }
    }
}
