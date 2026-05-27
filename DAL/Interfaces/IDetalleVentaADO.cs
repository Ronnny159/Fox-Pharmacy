using Entity;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de DetalleVenta.
/// </summary>
public interface IDetalleVentaDAO
{
    List<DetalleVenta> ObtenerPorVenta(int ventaId);
    List<DetalleVenta> ObtenerPorLote(int loteId);
    void Insertar(DetalleVenta detalle);
}