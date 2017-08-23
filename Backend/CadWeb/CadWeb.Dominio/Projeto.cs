using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadWeb.Dominio
{
    public class Projeto
    {
        public Projeto()
        {
            this.Data_Inicio = DateTime.Now;
            this.Data_fim = DateTime.Now;
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Semestre { get; set; }
        public DateTime Data_Inicio { get; set; }
        public DateTime Data_fim { get; set; }
        
        //status
        //0 = Planejamento
        //1 = Execução
        //2 = Finalização
        //3 = Finalizado
        public int Status { get; set; }
        //Tipo
        //0 = Pesquisa
        //1 = Extensão
        public int Tipo { get; set; }
    }
}
