using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modal.Request
{
    public class ProjectProposalUpdateRequest
    {
        public string ProjectTitle { get; set; }
        public string ProjectDescription { get; set; }
        public int EstimatedDuration { get; set; }
    }
}
