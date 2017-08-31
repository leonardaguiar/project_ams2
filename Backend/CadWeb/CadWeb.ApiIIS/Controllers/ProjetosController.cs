using CadWeb.Dados.DataContexts;
using CadWeb.Dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CadWeb.ApiIIS.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1")]
    public class ProjetosController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();
        
        //[Authorize()]
        [Route("projetos")]
        public HttpResponseMessage GetProjetos()
        {
                     
            var results = db.Projeto.ToList();
           
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
        //[Authorize()]
        [Route("projetos/{id:int}")]
        public HttpResponseMessage GetProjetosById(int id)
        {

                               
            ProjetoView projetoview = new ProjetoView();
           
            var results = db.Projeto.Where(x => x.Id == id);
            foreach (var result in results) {
                projetoview.Projeto = result;
            }
            //Realiza as consultas das atividades realacionadas com o projeto
            // var tarefas = db.TarefasProjeto.Where(x => x.ProjetoId == id).ToList();
            var tarefas = from trf in db.TarefasProjeto
                          where trf.ProjetoId == id
                          select new {
                              Id = trf.Id,
                              IdGeral = trf.IdGeral,
                              ProjetoId = trf.ProjetoId,
                              Prioridade = trf.Prioridade,
                              Status = trf.Status,
                              Data  = trf.Data,
                              Descricao = trf.Descricao
                              

                          };
            if (tarefas!=null)
            {
                projetoview.Tarefas = new List<TarefaProjeto>();
                foreach (var tarefav in tarefas) {
                    TarefaProjeto tarefaprj = new TarefaProjeto();
                    tarefaprj.Id = tarefav.Id;
                    tarefaprj.IdGeral = tarefav.IdGeral;
                    tarefaprj.Prioridade = tarefav.Prioridade;
                    tarefaprj.Status = tarefav.Status;
                    tarefaprj.ProjetoId = tarefav.ProjetoId;
                    tarefaprj.Descricao = tarefav.Descricao;
                    tarefaprj.Data = tarefav.Data;
                    projetoview.Tarefas.Add(tarefaprj);

                }
            }
            //Realiza a consulta das atividades relacionadas com o projeto
            //var atividades = db.AtividadesProjeto.Where(x => x.ProjetoId == id).ToList();
            var atividades = from atv in db.AtividadesProjeto
                             where atv.ProjetoId == id
                             select new
                             {
                                 atv.Id,
                                 atv.ProjetoId,
                                 atv.Descricao,
                                 atv.Data,
                               
                             };

            if (atividades != null)
            {
                projetoview.Atividades = new List<AtividadeProjeto>();
                foreach (var atvprj in atividades)
                {
                    AtividadeProjeto atividade = new AtividadeProjeto();
                    atividade.Data = atvprj.Data;
                    atividade.Descricao = atvprj.Descricao;
                    atividade.Id = atvprj.Id;
                    atividade.ProjetoId = atvprj.ProjetoId;

                    projetoview.Atividades.Add(atividade);
                }
            }

            //Carrega participantes
            //var participantesprojeto = db.ParticipantesProjeto.Where(x => x.ProjetoId == id).ToList();
            var participantes = from prtp in db.ParticipantesProjeto
                                where prtp.ProjetoId == id
                                select new
                                {
                                   
                                    prtp.Comissao, 
                                    prtp.Coordenador,
                                    prtp.Discente,
                                    prtp.DiscenteId,
                                    prtp.Docente,
                                    prtp.DocenteId,
                                    prtp.Id,
                                    prtp.ProjetoId,
                                    prtp.TipoParticipante
                                };

            if (participantes != null)
            {
                projetoview.ParticipantesProjeto = new List<ParticipanteProjeto>();
                foreach (var prtp in participantes) {
                    ParticipanteProjeto participante = new ParticipanteProjeto();
                    participante.Comissao = prtp.Comissao;
                    participante.Coordenador = prtp.Coordenador;
                    participante.DiscenteId = prtp.DiscenteId;
                    participante.DocenteId = prtp.DocenteId;
                    participante.Id = prtp.Id;
                    participante.ProjetoId = prtp.ProjetoId;
                    participante.TipoParticipante = prtp.TipoParticipante;
                    participante.Docente = prtp.Docente;
                    participante.Discente = prtp.Discente;

                    projetoview.ParticipantesProjeto.Add(participante);
                }
            }

            //Carrega semestres
            //var semestres = db.SemestresProjeto.Where(x => x.ProjetoId == id).ToList();
            //if (semestres.Count > 0)
            //{
            //    projetoview.Semestres = semestres;
            //}

            return Request.CreateResponse(HttpStatusCode.OK, projetoview);


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
        [Route("projetos")]
        public HttpResponseMessage PostProjeto(ProjetoView projetoview)
        {

            if (projetoview == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possivel gravar os dados, as informações estão incompletas");
            }
         
            try
            {

                Projeto projeto = projetoview.Projeto;
                db.Projeto.Add(projeto);
                db.SaveChanges();
                if (projetoview.Tarefas != null) {
                    foreach (TarefaProjeto tarefa in projetoview.Tarefas) {
                        tarefa.ProjetoId = projeto.Id;
                        db.TarefasProjeto.Add(tarefa);
                        db.SaveChanges();

                    }

                }
                if (projetoview.Atividades != null)
                {
                    foreach (AtividadeProjeto atividade in projetoview.Atividades)
                    {
                        atividade.ProjetoId = projeto.Id;
                        db.AtividadesProjeto.Add(atividade);
                        db.SaveChanges();

                    }

                }
                if (projetoview.ParticipantesProjeto != null)
                {
                    foreach (AtividadeProjeto atividade in projetoview.Atividades)
                    {
                        atividade.ProjetoId = projeto.Id;
                        db.AtividadesProjeto.Add(atividade);
                        db.SaveChanges();

                    }
                }

                    //if (projetoview.Semestres != null)
                    //{
                    //    foreach (SemestreProjeto semestre in projetoview.Semestres)
                    //    {
                    //        semestre.ProjetoId = projeto.Id;
                    //        db.SemestresProjeto.Add(semestre);
                    //        db.SaveChanges();

                    //    }

                    //}

                var result = projetoview.Projeto;
                return Request.CreateResponse(HttpStatusCode.Created, result);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção do projeto");
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
        [Route("projetos")]
        public HttpResponseMessage PutProjeto(Projeto projeto)
        {
            if (projeto == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possível alterar o projeto. Informações incompletas");
            }

            try
            {

                db.Entry<Projeto>(projeto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                
                var result = projeto;
                return Request.CreateResponse(HttpStatusCode.OK, "Projeto alterado com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro do projeto");
            }
        }
            
            //[Authorize()]
            //[HttpDelete]
            //[Route("usuarios")]
            //public HttpResponseMessage Delete(int id)
            //{
            //    if (id <= 0)
            //    {
            //        return Request.CreateResponse(HttpStatusCode.BadRequest);
            //    }

            //    try
            //    {
            //        db.Usuarios.Remove(db.Usuarios.Find(id));
            //        db.SaveChanges();

            //        return Request.CreateResponse(HttpStatusCode.OK, "Usuario excluido");

            //    }
            //    catch
            //    {

            //        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir usuario");
            //    }

            //}


        }
}
