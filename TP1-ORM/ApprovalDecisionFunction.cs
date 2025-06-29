using Application.Interfaces.Users;
using Domain.Enums;
using Interfaces.ProApprovalStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_ORM
{
    public class ApprovalDecisionFunction
    {
        private readonly IApprovalStepService _stepService;
        private readonly IUserService _userService;

        public ApprovalDecisionFunction (IApprovalStepService stepService, IUserService userService)
        {
            _stepService = stepService;
            _userService = userService;
        }

        public async Task ApprovalDecision(int userId)
        {    
            Console.Clear();
            HeadTitle.ShowTitle("======= REVISIÓN DEL PROYECTO =======");

            var pendingSteps = await _stepService.GetPendingApprovalStepByUserIdAsync(userId);

            if (pendingSteps == null || !pendingSteps.Any())
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\tNo existen pasos pendientes de aprobación");
                Console.WriteLine("\nPresione cualquier tecla para volver...");
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Proyectos pendientes: ");
            Console.ResetColor();

            for (int i = 0; i < pendingSteps.Count; i++) 
            {
                Console.WriteLine($"\n\t{i + 1}. Proyecto: {pendingSteps[i].ProjectProposal?.Title?? "Sin titulo"} | Orden: {pendingSteps[i].StepOrder}");

            }

            Console.WriteLine("\nSeleccione el numero del paso a gestionar o marque 0 para volver: ");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (!int.TryParse(Console.ReadLine(), out int opcion) || opcion < 1|| opcion >pendingSteps.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opcion inválida.");
                Console.ResetColor();
                return;
            }
            Console.ResetColor();

            if (opcion == 0)
            {
                Console.WriteLine("Volviendo al menu principal...");
                return;
            }

            Console.Clear();
            var selectedStep = pendingSteps[opcion - 1];
            var proposal = selectedStep.ProjectProposal;
            var creator = await _userService.GetUserByIdAsync(proposal.CreatedBy);

            HeadTitle.ShowTitle("======= REVISIÓN DEL PROYECTO =======");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\nTítulo: {proposal?.Title}");
            Console.ResetColor();
            Console.WriteLine($"\n\tDescripción: {proposal?.Description}");
            Console.WriteLine($"\tFecha de creación: {proposal?.CreatedAt.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"\tMonto solicitado: ${proposal?.EstimatedAmount ?? 0}");
            Console.WriteLine($"\tÁrea: {proposal?.Areas?.Name ?? "N/A"}");
            Console.WriteLine($"\tTipo de proyecto: {proposal?.ProjectType?.Name ?? "N/A"}");
            Console.WriteLine($"\tCreado por: {creator.Name}");
            Console.WriteLine($"\tOrden de aprobación actual: {selectedStep.StepOrder}");
            Console.WriteLine();

            Console.WriteLine("\nSeleccione la opcion concorde a su veredicto sobre el proyecto: ");
            Console.WriteLine("\t 1. Aprobar");
            Console.WriteLine("\t 2. Rechazar");
            Console.WriteLine("\t 3. Salir");

            var election = Console.ReadLine();

            string comment = "";

            if(election == "1" || election == "2")
{
                string input = "";
                while (input != "s" && input != "n")
                {
                    Console.WriteLine("¿Desea agregar un comentario sobre el proyecto? (s/n)");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    input = Console.ReadLine()?.Trim().ToLower();
                    Console.ResetColor();

                    if (input != "s" && input != "n")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opción inválida. Intente nuevamente.");
                        Console.ResetColor();
                    }
                }

                if (input == "s")
                {
                    bool validComment = false;
                    while (!validComment)
                    {
                        Console.WriteLine("Ingrese su comentario:");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        comment = Console.ReadLine();
                        Console.ResetColor();

                        if (string.IsNullOrWhiteSpace(comment) || comment.Trim().Length < 5)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Comentario inválido. Debe contener al menos 5 caracteres y no puede estar vacío.");
                            Console.ResetColor();
                        }
                        else
                        {
                            validComment = true;
                        }
                    }
                }
            }


            bool result = false;

            switch (election)
            {
                case "1":
                    result = await _stepService.ApproveStepAsync(selectedStep.Id, userId, true, comment);
                    break;
                case "2":
                    result = await _stepService.ApproveStepAsync(selectedStep.Id, userId, false, comment);
                    break;
                case "3":

                    selectedStep.ApproverUserId = userId;
                    result = await _stepService.ObserveAsync(selectedStep, "Sin comentarios...");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opción inválida. Presione una tecla para continuar...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
            }
            if (result)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("El estado de la operacion fue actualizada correctamente.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No se pudo gestionar el paso.");
                Console.ResetColor();
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
            

        }
    }
}
