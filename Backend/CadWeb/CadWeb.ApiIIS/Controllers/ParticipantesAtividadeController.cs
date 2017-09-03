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
    public class ParticipantesAtividadeController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();
        [Authorize()]
        [Route("participantesatividade")]
        public HttpResponseMessage GetParticipantesAtividade()
        {

            var results = db.ParticipantesAtividadeProjeto.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Authorize()]
        [Route("participantesatividade/{id:int}")]
        public HttpResponseMessage GetParticipanteAtividadeById(int id)
        {

            //var results = db.ParticipantesAtividadeProjeto.Where(x => x.Id == id);

            var participantes = from prtp in db.ParticipantesAtividadeProjeto
                                where prtp.Id == id
                                select new
                                {

                                    prtp.Discente,
                                    prtp.DiscenteId,
                                    prtp.Docente,
                                    prtp.DocenteId,
                                    prtp.Id,
                                    prtp.AtividadeProjetoId,
                                    prtp.TipoParticipante
                                };

            ParticipanteAtividadeProjeto participante = new ParticipanteAtividadeProjeto();
            if (participantes != null)
            {
                
                foreach (var prtp in participantes)
                {
                    
                   
                    participante.DiscenteId = prtp.DiscenteId;
                    participante.DocenteId = prtp.DocenteId;
                    participante.Id = prtp.Id;
                    participante.AtividadeProjetoId= prtp.AtividadeProjetoId;
                    participante.TipoParticipante = prtp.TipoParticipante;
                    participante.Docente = prtp.Docente;
                    participante.Discente = prtp.Discente;
                                       
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, participante);

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
        [Authorize()]
        [HttpPost]
        [Route("participantesatividade")]
        public HttpResponseMessage PostParticipante(ParticipanteAtividadeProjeto participante)
        {

            if (participante == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possivel gravar os dados, as informações estão incompletas");
            }

            try
            {

                db.ParticipantesAtividadeProjeto.Add(participante);
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
        [Route("participantesatividade")]
        public HttpResponseMessage PutParticipanteAtividade(ParticipanteAtividadeProjeto participante)
        {
            if (participante == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possível alterar o participante. Informações incompletas");
            }

            try
            {

                db.Entry<ParticipanteAtividadeProjeto>(participante).State = System.Data.Entity.EntityState.Modified;
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
        [Route("participantesatividade/{id:int}")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.ParticipantesAtividadeProjeto.Remove(db.ParticipantesAtividadeProjeto.Find(id));
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
