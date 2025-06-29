using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modal.Response
{
    public class ProjectApprovalStepResponse
    {
        public int id { get; set; }
        public int stepOrder { get; set; }
        public DateTime? decisionDate { get; set; }
        public string? observations { get; set; }

        public UserResponseDTO approverUser { get; set; }
        public GenericResponse approverRole { get; set; }
        public GenericResponse status { get; set; }
    }

}
