﻿using CadWeb.Dados.DataContexts;
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
    public class AtividadesController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();

        [Authorize()]
        [Route("atividades")]
        public HttpResponseMessage GetAtividades()
        {

            var results = db.AtividadesProjeto.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Authorize()]
        [Route("atividades/{id:int}")]
        public HttpResponseMessage GetAtividadeById(int id)
        {
            AtividadeProjetoView atividadeview = new AtividadeProjetoView();
            var results = db.AtividadesProjeto.Where(x => x.Id == id);
            foreach (var result in results)
            {
                atividadeview.AtividadeProjeto = result;
            }
            var participantes = from prtf in db.ParticipantesAtividadeProjeto
                                where prtf.AtividadeProjetoId == id
                                select new
                                {
                                    prtf.DiscenteId,
                                    prtf.Discente,
                                    prtf.DocenteId,
                                    prtf.Docente,
                                    prtf.Id,
                                    prtf.AtividadeProjetoId,
                                    prtf.TipoParticipante
                                };

            if (participantes != null)
            {
                atividadeview.ParticipantesAtividade = new List<ParticipanteAtividadeProjeto>();
                foreach (var prtp in participantes)
                {
                    ParticipanteAtividadeProjeto participante = new ParticipanteAtividadeProjeto();
                    participante.Discente = prtp.Discente;
                    participante.DiscenteId = prtp.DiscenteId;
                    participante.Docente = prtp.Docente;
                    participante.DocenteId = prtp.DocenteId;
                    participante.Id = prtp.Id;
                    participante.AtividadeProjetoId = prtp.AtividadeProjetoId;
                    participante.TipoParticipante = prtp.TipoParticipante;

                    atividadeview.ParticipantesAtividade.Add(participante);
                }
            }


            return Request.CreateResponse(HttpStatusCode.OK, atividadeview);

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
        [Route("atividades")]
        public HttpResponseMessage PostAtividade(AtividadeProjetoView atividadeview)
        {

            if (atividadeview == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possivel gravar os dados, as informações estão incompletas");
            }

            try
            {

                AtividadeProjeto atividade = atividadeview.AtividadeProjeto;
                db.AtividadesProjeto.Add(atividade);
                db.SaveChanges();
                if (atividadeview.ParticipantesAtividade != null)
                {
                    foreach (ParticipanteAtividadeProjeto participante in atividadeview.ParticipantesAtividade)
                    {
                        participante.AtividadeProjetoId = atividade.Id;
                        db.ParticipantesAtividadeProjeto.Add(participante);
                        db.SaveChanges();

                    }

                }


                var result = atividade;
                return Request.CreateResponse(HttpStatusCode.Created, atividade);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção da atividade");
            }

        }
      
        [Authorize()]
        [HttpPut]
        [Route("atividades")]
        public HttpResponseMessage PutAtividades(AtividadeProjetoView atividadeview)
        {
            if (atividadeview.AtividadeProjeto == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possível alterar a tarefa. Informações incompletas");
            }

            try
            {
                AtividadeProjeto atividade = atividadeview.AtividadeProjeto;
                db.Entry<AtividadeProjeto>(atividade).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
               
                var result = atividadeview;
                return Request.CreateResponse(HttpStatusCode.OK, "Atividade alterada com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro da Atividade");
            }
        }

        [Authorize()]
        [HttpDelete]
        [Route("atividades/{id:int}")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                var participantes =  db.ParticipantesAtividadeProjeto.Where(x => x.AtividadeProjetoId == id).ToList();
                if (participantes.Count > 0)
                {
                    foreach (ParticipanteAtividadeProjeto participante in participantes)
                    {
                        db.ParticipantesAtividadeProjeto.Remove(db.ParticipantesAtividadeProjeto.Find(participante.Id));
                        db.SaveChanges();
                    }
                }   
                            
                db.AtividadesProjeto.Remove(db.AtividadesProjeto.Find(id));
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Atividade excluída com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir atividade");
            }

        }

    }
}
