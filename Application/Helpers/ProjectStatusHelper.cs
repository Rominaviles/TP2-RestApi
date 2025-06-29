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
        public static ApprovalStatusEnum CalcularEstadoProyecto(List<ProjectApprovalStep> pasos)
        {
            // Verificar si hay algún paso rechazado
            if (pasos.Any(p => p.Status == (int)ApprovalStatusEnum.Rejected))
                return ApprovalStatusEnum.Rejected;

            // Verificar si hay algún paso observado
            if (pasos.Any(p => p.Status == (int)ApprovalStatusEnum.Observed))
                return ApprovalStatusEnum.Observed;

            // Verificar si todos están aprobados
            if (pasos.All(p => p.Status == (int)ApprovalStatusEnum.Approved))
                return ApprovalStatusEnum.Approved;

            // Verificar el flujo secuencial para pasos pendientes
            var pasosOrdenados = pasos.OrderBy(p => p.StepOrder).ToList();

            for (int i = 0; i < pasosOrdenados.Count; i++)
            {
                var actual = pasosOrdenados[i];

                // Si encontramos un paso pendiente
                if (actual.Status == (int)ApprovalStatusEnum.Pending)
                {
                    // El primer paso siempre puede estar pendiente
                    // O si el paso anterior está aprobado
                    if (i == 0 || pasosOrdenados[i - 1].Status == (int)ApprovalStatusEnum.Approved)
                        return ApprovalStatusEnum.Pending;
                }
            }

            // Por defecto, pendiente
            return ApprovalStatusEnum.Pending;

        }
    }
}
