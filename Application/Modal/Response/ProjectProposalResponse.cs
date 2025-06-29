using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modal.Response
{
    public class ProjectProposalResponse
    {//Muestra de informacion
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public decimal amount { get; set; }
        public int duration { get; set; }
        public GenericResponse area { get; set; }
        public GenericResponse status { get; set; }
        public GenericResponse type { get; set; }
        public UserResponseDTO user { get; set; }
        public List<ProjectApprovalStepResponse> steps { get; set; }
    }
}
