using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProjectProposal
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }

        //FK
        public int Area { get; set; }
        public Area?  Areas { get; set; }

        //FK
        public int Type { get; set; }
        public ProjectType ProjectType { get; set; }

        public decimal EstimatedAmount { get; set; }
        public int EstimatedDuration { get; set; }

        //FK
        public int Status { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }

        public DateTime CreateAt { get; set; }

        //FK
        public int CreateBy { get; set; }
        public User User { get; set; }

        //Relations
        public ICollection<ProjectApprovalStep> ProjectApprovalSteps { get; set; }

    }
    
}
