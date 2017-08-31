﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadWeb.Dominio
{
    public class AnexoProjeto
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public virtual Projeto Projeto { get; set; }
        public string Descricao { get; set; }
        public string Link { get; set; }
        public DateTime Data { get; set; }
      
    }
}