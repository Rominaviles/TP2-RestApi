using Interfaces.ProProposal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace TP1_ORM
{
    public class CreateProjectFunction
    {
        private readonly IProjectProposalService _projectProposalServi;

        public CreateProjectFunction(IProjectProposalService projectProposalServi)
        {
            _projectProposalServi = projectProposalServi;
        }

        public async Task CreateProject(int userId)
        {
            try
            {
                Console.Clear();
                HeadTitle.ShowTitle("======== SOLICITUD DE PROYECTO ======== ");

                string title;
                while (true)
                {
                    Console.WriteLine("\nIngrese el título del proyecto por favor: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    title = Console.ReadLine().ToLower();
                    Console.ResetColor();


                    if (string.IsNullOrWhiteSpace(title) || title.All(char.IsDigit))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\tEl título ingresado no es válido. Intente nuevamente.");
                        Console.ResetColor();

                    }
                    else break;
                }

                string description;
                while (true)
                {
                    Console.WriteLine("\nIngrese la descripción del proyecto por favor: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    description = Console.ReadLine().ToLower();
                    Console.ResetColor();
                    if (string.IsNullOrEmpty(description) || description.All(char.IsDigit))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\tLa descripción no es valida, por favor vuelva a intentar.");
                        Console.ResetColor();
                    }
                    else break;
                }



                int areaId;
                while (true)
                {
                    Console.WriteLine("\nIngrese el número de área, siendo estos los siguientes: ");
                    Console.WriteLine("\t 1. Finanzas");
                    Console.WriteLine("\t 2. Tecnología");
                    Console.WriteLine("\t 3. Recursos Humanos");
                    Console.WriteLine("\t 4. Operaciones");

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    string inputArea = Console.ReadLine();
                    Console.ResetColor();

                    if (!int.TryParse(inputArea, out areaId) || areaId < 1 || areaId > 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\tEl área ingresada no es válida. Intente nuevamente.");
                        Console.ResetColor();
                    }
                    else break;
                }

                int typeId;
                while (true)
                {
                    Console.WriteLine("\nIngrese el número de tipo de proyecto, siendo estos los siguientes: ");
                    Console.WriteLine("\t 1. Mejora de procesos");
                    Console.WriteLine("\t 2. Innovación y Desarrollo");
                    Console.WriteLine("\t 3. Infraestructura");
                    Console.WriteLine("\t 4. Capacitación Interna");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    string inputType = Console.ReadLine();
                    Console.ResetColor();
                    if (!int.TryParse(inputType, out typeId) || typeId < 1 || typeId > 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\tEl tipo de proyecto no es válido. Intente nuevamente.");
                        Console.ResetColor();
                    }
                    else break;
                }

                decimal estimatedAmount;
                while (true)
                {
                    Console.WriteLine("\nIngrese el monto del proyecto por favor: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    string inputAmount = Console.ReadLine();
                    Console.ResetColor();
                    if (!decimal.TryParse(inputAmount, out estimatedAmount))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\tEl monto no es válido. Intente nuevamente.");
                        Console.ResetColor();
                    }
                    else if (estimatedAmount < 0 || estimatedAmount > 20000000000)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\tEl monto no puede ser negativo. Intente nuevamente.");
                        Console.ResetColor();
                    }
                    else break;
                }

                int estimatedDuration;
                while (true)
                {
                    Console.WriteLine("\nIngrese la duracion estimada en dias por favor: ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    string inputDuration = Console.ReadLine();
                    Console.ResetColor();

                    if (!int.TryParse(inputDuration, out estimatedDuration))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\tLa duración estimada no es válida. Intente nuevamente.");
                        Console.ResetColor();
                    }
                    else if (estimatedDuration <= 0 || estimatedDuration >= 720)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\tLa duración del proyecto debe ser mayor a 0 y menor a 2 años(720 dias). Intente nuevamente.");
                        Console.ResetColor();
                    }

                    else break;
                }


                var projectProposalDTO = new ProjectProposalDTO
                {
                    ProjectTitle = title,
                    ProjectDescription = description,
                    AreaId = areaId,
                    ProjectTypeId = typeId,
                    EstimatedAmount = estimatedAmount,
                    EstimatedDuration = estimatedDuration
                };

                var result = await _projectProposalServi.CreateProjectProposal(projectProposalDTO, userId);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\tSolicitud de proyecto creada con éxito.");
                Console.ResetColor();

            }
            catch (FormatException ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            catch (ArgumentException ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }

            Console.WriteLine("\n   Presione cualquier tecla para continuar...");
            Console.ReadKey(true);

        }

    }
}
