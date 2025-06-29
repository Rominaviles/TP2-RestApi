using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modal.Request
{
    public class DecisionStepRequest
    {
        public int Status { get; set; }
        public int User { get; set; }
        public string? Observation { get; set; }
    }
}
