# 📝 Sistema de Aprobación de Proyectos - Backend

Este proyecto es un sistema de gestión y aprobación de propuestas de proyectos. Permite crear propuestas, definir flujos de aprobación según reglas, y registrar decisiones de los usuarios responsables (aprobar, rechazar u observar).

---

## 🚀 Tecnologías Utilizadas

- **Backend**: ASP.NET Core Web API
- **ORM**: Entity Framework Core (Code First)
- **Base de datos**: SQL Server
- **Arquitectura**: Hexagonal (Ports and Adapters)

---

## 🧠 Funcionalidades principales

- Crear y listar propuestas de proyecto.
- Asignar automáticamente pasos de aprobación en base a reglas definidas por:
  - Área
  - Tipo de proyecto
  - Monto estimado
- Registrar decisiones de los responsables:
  - ✅ Aprobar
  - ❌ Rechazar
  - 👀 Observar
- Estado general del proyecto actualizado dinámicamente:
  - ✅ Aprobado si todos los pasos están aprobados
  - ❌ Rechazado si al menos un paso es rechazado
  - 👀 Observado si algún paso está observado
  - ⏳ Pendiente si hay pasos en espera

---

## 🌐 Proyecto relacionado

👉 Este backend se conecta con el frontend desarrollado en el [TP3-Front](https://github.com/Rominaviles/TP3-Front) para permitir la interacción visual con el sistema de aprobación.

---

## 🧪 API REST

El backend expone una API RESTful para ser consumida desde cualquier cliente (frontend web, mobile, etc). Algunos endpoints clave:

- `POST /api/Project` — Crear propuesta
- `GET /api/Project/filter` — Obtener propuestas filtradas
- `GET /api/Project/{id}` — Obtener detalles
- `PATCH /api/Project/{id}` — Editar propuesta
- `PATCH /api/Project/{id}/decision` — Tomar decisión (aprobación/rechazo/observación)
