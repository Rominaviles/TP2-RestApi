﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApproverRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Relations
        public ICollection<ApprovalRule> ApprovalRules { get; set; }
        public ICollection<ProjectApprovalStep> ProjectApprovalSteps { get; set; }
        public ICollection<User> User { get; set; } 
    }

}
