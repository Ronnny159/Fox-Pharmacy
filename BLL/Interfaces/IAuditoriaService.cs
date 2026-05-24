using System;
using BLL.DTOs;

namespace BLL.Interfaces;

/// <summary>
/// Contrato para la lógica de negocio relacionada con auditoría y trazabilidad.
/// </summary>
public interface IAuditoriaService
{
    ResultadoOperacion ObtenerHistorialParametros(string clave);
    ResultadoOperacion ObtenerHistorialDescuentosProducto(int productoId);
    ResultadoOperacion ObtenerResumenAuditoria();
}