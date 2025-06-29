﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApprovalStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Relations
        public ICollection<ProjectProposal> ProjectProposals { get; set; }
        public ICollection<ProjectApprovalStep> ProjectApprovalSteps { get; set; }

    }
}
