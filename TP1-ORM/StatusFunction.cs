using Application.Interfaces.Users;
using Domain.Enums;
using Interfaces.ProProposal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_ORM
{
    public class StatusFunction
    {
        private readonly IProjectProposalService _projectProposalServi;
        private readonly IUserService _userService;

        public StatusFunction(IProjectProposalService projectProposalServi, IUserService userService)
        {
            _projectProposalServi = projectProposalServi;
            _userService = userService;
        }

        public async Task ViewProposalStatus(int userId)
        {
            while (true)
            {
                Console.Clear();
                HeadTitle.ShowTitle("======== ESTADOS DE LOS PROYECTOS ========");

                var projectProposals = await _projectProposalServi.GetAllProjectProposalsByUserAsync(userId);

                if (projectProposals == null || projectProposals.Count == 0)
                {
                    Console.WriteLine("\n\tNo hay solicitudes de proyectos para mostrar.");
                    Console.WriteLine("\nPresione cualquier tecl para volver al menu...");
                    Console.ReadKey();
                    return;
                }

                for (int i = 0; i < projectProposals.Count; i++)
                {
                    var estado = (ApprovalStatusEnum)projectProposals[i].Status;
                    Console.WriteLine($"\n\t{i + 1}. Título: {projectProposals[i].ProjectTitle} - Estado: {estado}");
                }

                Console.WriteLine("\nIngrese el número de la solicitud para ver los detalles o 0 para salir:");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                if (!int.TryParse(Console.ReadLine(), out int opcion) || opcion < 0 || opcion > projectProposals.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\tOpción no válida. Presione cualquier tecla para intentar nuevamente.");
                    Console.ResetColor();
                    Console.ReadKey();
                    continue;
                }
                Console.ResetColor();

                if (opcion == 0)
                    return;

                var selectedProposal = projectProposals[opcion - 1];
                var proposalDetail = await _projectProposalServi.GetProjectProposalDetailAsync(selectedProposal.id);

                Console.Clear();
                HeadTitle.ShowTitle("======== ESTADOS DE LOS PROYECTOS ========");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\nTítulo: {proposalDetail.ProjectTitle}");
                Console.ResetColor();

                Console.WriteLine($"\n\tDescripción: {proposalDetail.ProjectDescription}");
                Console.WriteLine($"\tÁrea: {proposalDetail.AreaName}");
                Console.WriteLine($"\tTipo de Proyecto: {proposalDetail.ProjectTypeName}");
                Console.WriteLine($"\tEstado: {(ApprovalStatusEnum)proposalDetail.Status}");
                Console.WriteLine($"\tMonto estimado: ${proposalDetail.EstimatedAmount}");
                Console.WriteLine($"\tDuración estimada: {proposalDetail.EstimatedDuration} días");
                Console.WriteLine($"\tFecha de creación: {proposalDetail.CreatedAt.ToShortDateString()}");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nPasos de aprobación:");
                Console.ResetColor();

                if (proposalDetail.ApprovalSteps == null || !proposalDetail.ApprovalSteps.Any())
                {
                    Console.WriteLine("\t\nNo hay pasos de aprobación cargados para esta propuesta.");
                }
                else
                {
                    foreach (var step in proposalDetail.ApprovalSteps.OrderBy(s => s.StepOrder))
                    {
                        var status = (ApprovalStatusEnum)step.Status;
                        string userName = "No asignado";
                        if (step.ApproverUserId != 0)
                        {
                            var user = await _userService.GetUserByIdAsync(step.ApproverUserId);
                            userName = user?.Name ?? "No encontrado";
                        }

                        Console.WriteLine($" - {status} | {userName} | {step.Observations}");
                    }
                }

                Console.WriteLine("\nPresione cualquier tecla para volver al listado...");
                Console.ReadKey(true);
            }
        }
    }

}
