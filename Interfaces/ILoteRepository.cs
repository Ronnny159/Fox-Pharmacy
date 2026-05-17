using System;
using System.Collections.Generic;
using Entity;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad Lote.
/// Incluye el método crítico FEFO.
/// </summary>
public interface ILoteRepository
{
    Lote? ObtenerPorId(int id);
    IEnumerable<Lote> ObtenerPorProducto(int productoId);
    IEnumerable<Lote> ObtenerTodosActivos();
    void Insertar(Lote lote);
    void Actualizar(Lote lote);
    void ActualizarStock(int loteId, int nuevaCantidad, EstadoLote estado);

    /// <summary>
    /// Aplica el algoritmo FEFO: selecciona el lote activo con fecha de vencimiento
    /// más próxima para un producto dado.
    /// Criterio de desempate: menor precio de compra.
    /// </summary>
    Lote? SeleccionarLoteFEFO(int productoId);
}