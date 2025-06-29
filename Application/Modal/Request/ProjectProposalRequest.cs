using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Modal.Request
{
    public class ProjectProposalRequest //Escritura
    {//Datos cargados por el usuario
        public string title { get; set; }
        public string description { get; set; }
        public decimal amount { get; set; }
        public int duration { get; set; }
        public int area { get; set; }
        public int user { get; set; }
        public int type { get; set; }
    }
}
