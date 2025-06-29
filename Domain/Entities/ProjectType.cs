﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProjectType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Relations
        public ICollection<ApprovalRule> ApprovalRules { get; set; }
        public ICollection<ProjectProposal> ProjectProposals { get; set; }
    }
}
