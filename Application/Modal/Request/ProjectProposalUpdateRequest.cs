using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modal.Request
{
    public class ProjectProposalUpdateRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        public int duration { get; set; }
    }
}
