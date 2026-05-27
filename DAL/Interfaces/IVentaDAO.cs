using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces;

/// <summary>
/// Contrato para operaciones de acceso a datos de Venta.
/// Las operaciones transaccionales delegan en SP_CREAR_VENTA y SP_ANULAR_VENTA.
/// </summary>
public interface IVentaDAO
{
    Venta CrearVenta(Venta venta, List<DetalleVenta> detalles);
    Venta? ObtenerPorId(int id);
    Venta? ObtenerPorNumeroFactura(string numeroFactura);
    List<Venta> ObtenerPorUsuario(int usuarioId);
    List<Venta> ObtenerPorRangoFechas(DateTime desde, DateTime hasta);
    void AnularVenta(int ventaId, int usuarioId);
}