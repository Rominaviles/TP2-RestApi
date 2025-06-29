using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ProProposal
{
    public interface IProjectProposalQuery //Lectura
    {
        //Existencia de propuesta
        Task<bool> ProjectProposalExistAsync(Guid proposalId);
        //Obtener propuesta por id(con relaciones)
        Task<ProjectProposal> GetProjectProposalByIdAsync(Guid proposalId);
        //Obtener todas las propuestas por id especifico
        Task<List<ProjectProposal>> GetAllProjectProposalsByUserIdAsync(int userId);
        //Consulta el estado
        Task<int> GetProjectProposalStatusAsync(Guid proposalId);
        //Existencia de titulo
        Task<bool> TitleExistAsync(string title, Guid? excludeId = null);
        //Obtener todas las propuestas filtradas
        Task<List<ProjectProposal>> GetFilteredProjectsAsync(string? title, int? status, int? applicant, int? approvalUser);

    }
}
