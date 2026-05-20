using System;
using System.Collections.Generic;
using Entity;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad DetalleVenta.
/// </summary>
public interface IDetalleVentaRepository
{
    List<DetalleVenta> ObtenerPorVenta(int ventaId);
    List<DetalleVenta> ObtenerPorLote(int loteId);
    void Insertar(DetalleVenta detalle);
}