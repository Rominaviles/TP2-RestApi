using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Interfaces.ProProposal
{
    public interface IProjectProposalCommand
    {
        Task<bool> CreateProjectProposal(ProjectProposal proposal);
        Task<bool> UpdateProjectProposalAsync(ProjectProposal proposal);
    }
}
