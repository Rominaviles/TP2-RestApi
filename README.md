# 📝 Sistema de Aprobación de Proyectos

Este proyecto es un sistema de gestión y aprobación de propuestas de proyectos. Permite crear propuestas, definir flujos de aprobación según reglas, y registrar decisiones de los usuarios responsables (aprobar, rechazar u observar).

---

## 🚀 Tecnologías Utilizadas

- **Backend**: ASP.NET Core
- **ORM**: Entity Framework Core (Code First)
- **Base de datos**: SQL Server
- **Arquitectura**: Hexagonal (Ports and Adapters)

---

## 🧠 Funcionalidades principales

- Crear y listar propuestas de proyecto.
- Asignar automáticamente pasos de aprobación en base a reglas:
  - Área
  - Tipo de proyecto
  - Monto estimado
- Aprobar, rechazar u observar pasos según el orden establecido.
- Estado general del proyecto actualizado dinámicamente:
  - ✅ Aprobado si todos los pasos están aprobados.
  - ❌ Rechazado si al menos un paso es rechazado.
  - 👀 Observado si algún paso está observado.
  - ⏳ Pendiente si hay pasos en espera.
