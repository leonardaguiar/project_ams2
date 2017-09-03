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
    public class ParticipantesTarefaController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();
        [Authorize()]
        [Route("participantestarefa")]
        public HttpResponseMessage GetParticipantesAtividade()
        {

            var results = db.ParticipantesTarefaProjeto.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Authorize()]
        [Route("participantestarefa/{id:int}")]
        public HttpResponseMessage GetParticipanteTarefaById(int id)
        {

            //var results = db.ParticipantesTarefaProjeto.Where(x => x.Id == id);

            var participantes = from prtp in db.ParticipantesTarefaProjeto
                                where prtp.Id == id
                                select new
                                {

                                    prtp.Discente,
                                    prtp.DiscenteId,
                                    prtp.Docente,
                                    prtp.DocenteId,
                                    prtp.Id,
                                    prtp.TarefaProjetoId,
                                    prtp.TipoParticipante
                                };

            ParticipanteTarefaProjeto participante = new ParticipanteTarefaProjeto();
            if (participantes != null)
            {

                foreach (var prtp in participantes)
                {


                    participante.DiscenteId = prtp.DiscenteId;
                    participante.DocenteId = prtp.DocenteId;
                    participante.Id = prtp.Id;
                    participante.TarefaProjetoId = prtp.TarefaProjetoId;
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
        [Route("participantestarefa")]
        public HttpResponseMessage PostParticipante(ParticipanteTarefaProjeto participante)
        {

            if (participante == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possivel gravar os dados, as informações estão incompletas");
            }

            try
            {

                db.ParticipantesTarefaProjeto.Add(participante);
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
        [Route("participantestarefa")]
        public HttpResponseMessage PutParticipanteTarefa(ParticipanteTarefaProjeto participante)
        {
            if (participante == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possível alterar o participante. Informações incompletas");
            }

            try
            {

                db.Entry<ParticipanteTarefaProjeto>(participante).State = System.Data.Entity.EntityState.Modified;
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
        [Route("participantestarefa/{id:int}")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.ParticipantesTarefaProjeto.Remove(db.ParticipantesTarefaProjeto.Find(id));
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
