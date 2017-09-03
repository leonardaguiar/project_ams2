using CadWeb.Dados.DataContexts;
using CadWeb.Dominio;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CadWeb.ApiIIS.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1")]
    public class AnexosProjetoController : ApiController
    {
        private CadWebDataContext db = new CadWebDataContext();
        //[Authorize()]
        [Route("anexosprojeto")]
        public HttpResponseMessage GetAnexosProjeto()
        {

            var results = db.AnexosProjeto.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Authorize()]
        [Route("anexosprojeto/{id:int}")]
        public HttpResponseMessage GetAnexosProjetoById(int id)
        {

            var results = db.AnexosProjeto.Where(x => x.Id == id);

            return Request.CreateResponse(HttpStatusCode.OK, results);

        }

        [Authorize()]
        [HttpPost]
        [Route("anexosprojeto")]
        public HttpResponseMessage PostAnexo(AnexoProjeto anexo)
        {
            try
            {

                db.AnexosProjeto.Add(anexo);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created, anexo);
                //return Request.CreateResponse(HttpStatusCode.Created);

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha na inserção do Anexo");
            }

        }
        //[Authorize()]
        [HttpPost]
        [Route("anexosprojeto/anexo")]
        public Task<HttpResponseMessage> PostFormData()
        {

            // Check if the request contains multipart/form-data.
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/Upload");
            string caminho = HttpContext.Current.Server.MapPath("~/App_Data/Upload");
            var provider = new MultipartFormDataStreamProvider(root);

            //O Arquivo é salvo porem com nome de arquivo gerenciado pela aplicação
            // Read the form data and return an async task.
            var task = request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    //Nome real do arquivo
                    string nameFile = "";
                    //caminho mais nome do arquivo salvo
                    string fileSaved = "";
                    //url disponibilizada para o usuario 
                    string url = "";
                    //localização do arquivo absoluto + o nome do arquivo real postado.
                    string caminhoTemp = "";
                    // Pega o caminho e o nome do arquivo criado, também pega o nome do arquivo postado
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                        Trace.WriteLine("Server file path: " + file.LocalFileName);
                        fileSaved = file.LocalFileName;
                        nameFile = file.Headers.ContentDisposition.FileName;
                        //nameFile = Path.GetFileName(fileSaved);
                    }

                    //o nome do arquivo recebe 10 caracteres gerados aleatoriamente 
                    nameFile = alfanumericoAleatorio(10) + nameFile.Trim('\\', '"').Replace(" ", "_");

                    //Concatena o caminho com o nome real do arquivo
                    //caminho = Rename(fileSaved, nameFile.Trim(), caminho);
                    caminhoTemp = caminho + '\\' + nameFile.Trim('\\', '"');
                    try
                    {   //Renomeia o arquivo para que seja o mesmo postado 
                        File.Move(fileSaved, caminhoTemp);
                    }
                    catch {

                        return Request.CreateResponse(HttpStatusCode.OK, "Não foi possivel anexar o arquivo, erro ao tentar renomear. Localizado " + fileSaved);
                    }
                    if (caminho.Equals(""))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Não foi possivel anexar o arquivo");
                    }
                    else
                    {
                        //Atribui a url para download do arquivo
                        //Local
                        //url = "http://localhost:59584/api/v1/anexosprojeto/anexo/?arquivo=" + nameFile; //EnviarArquivoFTP(caminho, nameFile);
                        //Remoto
                        url = "http://cadweb.us-west-2.elasticbeanstalk.com/api/v1/anexosprojeto/anexo/?arquivo=" + nameFile;
                        if (url.Equals(""))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "Não foi possivel anexar o arquivo");
                        }
                        else return Request.CreateResponse(HttpStatusCode.OK, url);
                    }
                    //return Request.CreateResponse(HttpStatusCode.OK,Rename(fileSaved, nameFile.Trim(),caminho));
                });

            return task;
        }

        [Authorize()]
        [HttpPut]
        [Route("anexosprojeto")]
        public HttpResponseMessage PutSemestreProjeto(AnexoProjeto anexo)
        {
            if (anexo == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Não foi possível alterar o ANEXO. Informações incompletas");
            }

            try
            {

                db.Entry<AnexoProjeto>(anexo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "ANEXO alterado com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.NotModified, "Fallha ao alterar cadastro do ANEXO");
            }
        }

        [Authorize()]
        [HttpDelete]
        [Route("anexosprojeto/{id:int}")]
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.AnexosProjeto.Remove(db.AnexosProjeto.Find(id));
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "ANEXO excluído com sucesso");

            }
            catch
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fallha ao excluir o Anexo");
            }

        }

        
        [HttpGet]
        [Route("anexosprojeto/anexo/{arquivo?}")]
        public HttpResponseMessage Test(string arquivo)
        {

            var path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Upload/" + arquivo); 
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentLength = stream.Length;
            return result;








            //var stream = new MemoryStream();

            //var result = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new ByteArrayContent(stream.GetBuffer())
            //};
            //result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            //{
            //    FileName =HttpContext.Current.Server.MapPath("~/App_Data/Upload/" + arquivo) 
            //};
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            //var response = ResponseMessage(result);

            //return response;
        }

        public string EnviarArquivoFTP(string arquivo, string nameFile)
        {
            try
            {
                string urlRetorno = "http://cadweb.byethost4.com/uploads/" + nameFile.Trim('\\', '"');
                string url= "ftp://ftp.byethost4.com/htdocs/uploads/"+nameFile; string usuario= "b4_20614900"; string senha="agu201";
                FileInfo arquivoInfo = new FileInfo(arquivo);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(url));

                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(usuario, senha);
                request.UseBinary = true;
                request.ContentLength = arquivoInfo.Length;

                using (FileStream fs = arquivoInfo.OpenRead())
                {
                    byte[] buffer = new byte[2048];
                    int bytesSent = 0;
                    int bytes = 0;

                    using (Stream stream = request.GetRequestStream())
                    {
                        while (bytesSent < arquivoInfo.Length)
                        {
                            bytes = fs.Read(buffer, 0, buffer.Length);
                            stream.Write(buffer, 0, bytes);
                            bytesSent += bytes;
                        }
                    }
                }
                return urlRetorno;
            }
            catch (Exception ex)
            {
                //throw ex;
                return "";

            }


        }
        //Renomeia arquivo
        private string Rename(string oldFile, string newFile, string caminho) {
            try
            {                
                caminho = caminho +'\\'+ newFile.Trim('\\','"');
                System.IO.File.Move(oldFile, caminho);
                    return caminho;
            }
            catch(ArgumentException e) {
                Trace.WriteLine(e);

                return "";
            }
        }
        //Caracteres alfanumericos aleatoriamente 
        public string alfanumericoAleatorio(int tamanho)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, tamanho)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

    }
}
