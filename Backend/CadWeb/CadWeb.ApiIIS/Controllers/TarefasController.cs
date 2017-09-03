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
    public class TarefasController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();

        [Authorize()]
        [Route("tarefas")]
        public HttpResponseMessage GetTarefas()
        {

            var results = db.TarefasProjeto.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
        [Authorize()]
        [Route("tarefas/{id:int}")]
        public HttpResponseMessage GetTarefaById(int id)
        {
            TarefaProjetoView tarefaview = new TarefaProjetoView();
            var results = db.TarefasProjeto.Where(x => x.Id == id);
            foreach (var result in results)
            {
                tarefaview.TarefaProjeto = result;
            }
            var participantes = from prtf in db.ParticipantesTarefaProjeto
                                where prtf.TarefaProjetoId == id
                                select new
                                {
                                    prtf.DiscenteId,
                                    prtf.Discente,
                                    prtf.DocenteId,
                                    prtf.Docente,
                                    prtf.Id,
                                    prtf.TarefaProjetoId,
                                    prtf.TipoParticipante
                                };

            if (participantes != null)
            {
                tarefaview.ParticipantesTarefa = new List<ParticipanteTarefaProjeto>();
                foreach (var prtp in participantes)
                {
                    ParticipanteTarefaProjeto participante = new ParticipanteTarefaProjeto();
                    participante.Discente = prtp.Discente;
                    participante.DiscenteId = prtp.DiscenteId;
                    participante.Docente = prtp.Docente;
                    participante.DocenteId = prtp.DocenteId;
                    participante.Id = prtp.Id;
                    participante.TarefaProjetoId = prtp.TarefaProjetoId;
                    participante.TipoParticipante = prtp.TipoParticipante;

                    tarefaview.ParticipantesTarefa.Add(participante);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK,tarefaview);

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
        [Route("tarefas")]
        public HttpResponseMessage PostTarefa(TarefaProjetoView tarefaview)
        {

            if (tarefaview == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possivel gravar os dados, as informações estão incompletas");
            }

            try
            {

                TarefaProjeto tarefa = tarefaview.TarefaProjeto;
                db.TarefasProjeto.Add(tarefa);
                db.SaveChanges();
                if (tarefaview.ParticipantesTarefa != null)
                {
                    foreach (ParticipanteTarefaProjeto participante in tarefaview.ParticipantesTarefa)
                    {
                        participante.TarefaProjetoId = tarefa.Id;
                        db.ParticipantesTarefaProjeto.Add(participante);
                        db.SaveChanges();

                    }

                }
               

                var result = tarefa;
                return Request.CreateResponse(HttpStatusCode.Created, tarefa);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção da tarefa");
            }

        }
        //[Authorize()]
        //[HttpPatch]
        //[Route("usuarios")]
        //public HttpResponseMessage PathUsuario(Usuario usuario)
        //{
        //    if (usuario == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }

        //    try
        //    {
        //        db.Entry<Usuario>(usuario).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //        var result = usuario;
        //        return Request.CreateResponse(HttpStatusCode.OK, "Usuario alterado com sucesso");

        //    }
        //    catch
        //    {

        //        return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha durante a alteração do cadastro e usuario");
        //    }

        //}
        [Authorize()]
        [HttpPut]
        [Route("tarefas")]
        public HttpResponseMessage PutTarefa(TarefaProjetoView tarefaview)
        {
            if (tarefaview == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possível alterar a tarefa. Informações incompletas");
            }

            try
            {

                db.Entry<TarefaProjeto>(tarefaview.TarefaProjeto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                //usuario.Senha = encrypt.Encryptword(usuario.Senha);
                var result = tarefaview.TarefaProjeto;
                return Request.CreateResponse(HttpStatusCode.OK, "Tarefa alterada com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro da tarefa");
            }
        }

        [Authorize()]
        [HttpDelete]
        [Route("atividade/{id:int}")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                var participantes = db.ParticipantesTarefaProjeto.Where(x => x.TarefaProjetoId == id).ToList();
                if (participantes.Count > 0)
                {
                    foreach (ParticipanteTarefaProjeto participante in participantes)
                    {
                        db.ParticipantesTarefaProjeto.Remove(db.ParticipantesTarefaProjeto.Find(participante.Id));
                        db.SaveChanges();
                    }
                }
                db.TarefasProjeto.Remove(db.TarefasProjeto.Find(id));
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Tarefa excluída com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir tarefa");
            }

        }
    }
}
