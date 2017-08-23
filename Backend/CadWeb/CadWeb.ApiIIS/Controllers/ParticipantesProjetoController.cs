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
    public class ParticipantesProjetoController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();
        //[Authorize()]
        [Route("participantesprojeto")]
        public HttpResponseMessage GetParticipantesProjeto()
        {

            var results = db.ParticipantesProjeto.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Authorize()]
        [Route("participantesprojeto/{id:int}")]
        public HttpResponseMessage GetParticipanteProjetoById(int id)
        {
            
            var results = db.ParticipantesProjeto.Where(x => x.Id == id);
           
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
        [Route("participantesprojeto")]
        public HttpResponseMessage PostParticipante(ParticipanteProjeto participante)
        {

            if (participante == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possivel gravar os dados, as informações estão incompletas");
            }

            try
            {
                               
                db.ParticipantesProjeto.Add(participante);
                db.SaveChanges();
                                
                return Request.CreateResponse(HttpStatusCode.Created, participante);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção do participante");
            }

        }

        [Authorize()]
        [HttpPut]
        [Route("participantesprojeto")]
        public HttpResponseMessage PutParticipanteProjeto(ParticipanteProjeto participante)
        {
            if (participante == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possível alterar o participante. Informações incompletas");
            }

            try
            {

                db.Entry<ParticipanteProjeto>(participante).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                               
                return Request.CreateResponse(HttpStatusCode.OK, "Participante alterado com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro do participante");
            }
        }

        [Authorize()]
        [HttpDelete]
        [Route("participantesprojeto")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.ParticipantesProjeto.Remove(db.ParticipantesProjeto.Find(id));
                db.SaveChanges();
          
                return Request.CreateResponse(HttpStatusCode.OK, "Participante excluído com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir participante");
            }

        }
    }
}
