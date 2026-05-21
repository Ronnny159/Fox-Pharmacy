using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de la entidad Venta.
/// </summary>
public interface IVentaRepository
{
    Venta CrearVenta(Venta venta, List<DetalleVenta> detalles);
    Venta? ObtenerPorId(int id);
    Venta? ObtenerPorNumeroFactura(string numeroFactura);
    IEnumerable<Venta> ObtenerPorUsuario(int usuarioId);
    IEnumerable<Venta> ObtenerPorRangoFechas(DateTime desde, DateTime hasta);
    void AnularVenta(int ventaId, int usuarioId);
}