using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class ProjectStatusHelper
    {
        public static int CalcularEstadoProyecto(List<ProjectApprovalStep> pasos)
        {
            if (pasos.Any(p => p.Status == 3)) // Rechazado
                return 3;

            if (pasos.Any(p => p.Status == 4)) // Observado
                return 4;

            if (pasos.All(p => p.Status == 2)) // Todos aprobados
                return 2;

            // Si hay alguno pendiente (1) y el anterior está aprobado
            var pasosOrdenados = pasos.OrderBy(p => p.StepOrder).ToList();
            for (int i = 0; i < pasosOrdenados.Count; i++)
            {
                var actual = pasosOrdenados[i];
                if (actual.Status == 1)
                {
                    if (i == 0 || pasosOrdenados[i - 1].Status == 2)
                        return 1; // Pendiente
                }
            }

            return 1; // Por defecto, pendiente

        }

    }
}
