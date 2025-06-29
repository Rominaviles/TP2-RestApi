using Domain.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Querys;
using Infrastructure.Command;
using Application.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Numerics;




namespace TP1_ORM
{
    internal class Program
    {
        static User LoggedUser;
        static async Task Main(string[] args)
        {
            using (var Ctx = new ApprovalProjectDB())
            {
                bool exitApp = false;
                while (!exitApp)
                {
                    LoggedUser = await LogIn(Ctx);

                    bool logging = true;

                    while (logging)
                    {
                        try
                        {
                            Console.Clear();
                            HeadTitle.ShowTitle("--------------------------------------------");
                            HeadTitle.ShowTitle("BIENVENIDO AL GESTOR DE PROYECTOS");
                            HeadTitle.ShowTitle("--------------------------------------------");

                            //Menu
                            Console.WriteLine("Seleccione una opción por favor: \n");
                            Console.WriteLine("\t 1. Crear solicitud de proyecto");
                            Console.WriteLine("\t 2. Aprobar/Rechazar paso de proyecto");
                            Console.WriteLine("\t 3. Ver estado de una solicitud");
                            Console.WriteLine("\t 4. Cerrar sesión");
                            Console.WriteLine("\t 0. Salir \n");

                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            int opcion;
                            if (!int.TryParse(Console.ReadLine(), out opcion))
                            {
                                throw new FormatException("Por favor ingrese un numero.\n");

                            }
                            Console.ResetColor();

                            var userService = new UserService(
                                new UserQuery(Ctx),
                                new UserCommand(Ctx)
                                );

                            var projectProposalServi = new ProjectProposalService(
                                new ProjectProposalCommand(Ctx),
                                new ProjectProposalQuery(Ctx),
                                new ApprovalRuleQuery(Ctx),
                                new ApprovalStepCommand(Ctx),
                                new ApprovalStepQuery(Ctx),
                                new UserQuery(Ctx)
                                );

                            var approvalStepServi = new ProjectApprovalStepService(
                                new ApprovalStepCommand(Ctx),
                                new ApprovalStepQuery(Ctx)
                                );
                            var approvalDecisionFunction = new ApprovalDecisionFunction(approvalStepServi, userService);

                            switch (opcion)
                            {
                                case 1:
                                    var createProjectFunction = new CreateProjectFunction(projectProposalServi);
                                    await createProjectFunction.CreateProject(LoggedUser.Id);
                                    break;

                                case 2:
                                    await approvalDecisionFunction.ApprovalDecision(LoggedUser.Id);
                                    break;

                                case 3:
                                    var viewProposalStatus = new StatusFunction(projectProposalServi, userService);
                                    await viewProposalStatus.ViewProposalStatus(LoggedUser.Id);
                                    break;

                                case 4:
                                    logging = false;
                                    break;

                                case 0:
                                    logging = false;
                                    exitApp = true;
                                    break;

                                default:
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("Opción invalida, intente nuevamente por favor");
                                    Console.ResetColor();
                                    Console.WriteLine("\n   Presione cualquier tecla para continuar...");
                                    Console.ReadKey(true);
                                    break;

                            }

                        }
                        catch (NullReferenceException)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Se ha producido en un error inesperado.\n");
                            Console.ResetColor();
                            Console.WriteLine("\n   Presione cualquier tecla para continuar...");
                            Console.ReadKey(true);
                        }
                        catch (OverflowException)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Se han ingresado demasiados datos, error.\n");
                            Console.ResetColor();
                            Console.WriteLine("\n   Presione cualquier tecla para continuar...");
                            Console.ReadKey(true);
                        }
                        catch (Exception Ex)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine(Ex.Message);
                            Console.ResetColor();
                            Console.WriteLine("\n   Presione cualquier tecla para continuar...");
                            Console.ReadKey(true);
                        }

                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("   Gracias por utilizar nuestros servicios, hasta la próxima...");
                    Console.ResetColor();
                }
            }
        }
                

        private static  async Task<User> LogIn(ApprovalProjectDB context)
        {
            while (true)
            {
                Console.Clear();
                HeadTitle.ShowTitle("======= LOGIN DE USUARIO DEL GESTOR DE PROYECTOS =======");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n¡Bienvenido!");
                Console.ResetColor();
                Console.WriteLine("\nPara utilizar nuestros servicios es necesario ingresar el mail, si desea cerrar la aplicacion, escriba 'salir'");
                Console.Write("\nIngrese su email: ");
                

                Console.ForegroundColor = ConsoleColor.DarkGray;
                string email = Console.ReadLine();
                Console.ResetColor();

                if (email?.Trim().ToLower() == "salir")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("   \nGracias por utilizar nuestros servicios, hasta la próxima...");
                    Console.ResetColor();
                    Console.WriteLine("Presione una tecla para continuar ...");
                    Console.ReadKey(true);
                    Environment.Exit(0); 
                }

                Console.WriteLine("Buscando usuario...");

                var user = await context.User
                    .FirstOrDefaultAsync(u => u.Email== email);

                if (user != null)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\t¡Bienvenido {user.Name}!\n");
                    Console.ResetColor();
                    Console.WriteLine("Presione una tecla para continuar ...");
                    Console.ReadKey(true);
                    return user;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nUsuario no encontrado. Intente nuevamente...");
                    Console.ResetColor();
                    Console.WriteLine("\n   Presione cualquier tecla para reintentar...");
                    Console.ReadKey(true);
                }
            }
        }

    }
}
