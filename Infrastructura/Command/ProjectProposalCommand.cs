using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.ProProposal;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command
{
    public class ProjectProposalCommand : IProjectProposalCommand
    {
        private readonly ApprovalProjectDB context;

        public ProjectProposalCommand(ApprovalProjectDB context)
        {
            this.context = context;
        }

        public async Task<bool> CreateProjectProposal(ProjectProposal proposal)
        {
           context.ProjectProposal.Add(proposal);
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateProjectProposalAsync(ProjectProposal proposal)
        {
            context.ProjectProposal.Update(proposal);
            return await context.SaveChangesAsync() >0;

        }


    }
}
