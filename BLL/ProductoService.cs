using BLL.DTOs;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;

namespace BLL.Services;

/// <summary>
/// Lógica de negocio para la gestión del catálogo maestro de productos.
/// Aplica validaciones de integridad, previene duplicados y protege
/// la eliminación de productos con lotes activos en inventario.
/// </summary>

public class ProductoService : IProductoService
{
    private readonly IProductoDAO _productoDAO;
    private readonly ILoteDAO _loteDAO;

    public ProductoService(IProductoDAO productoDAO, ILoteDAO loteDAO)
    {
        _productoDAO = productoDAO;
        _loteDAO = loteDAO;
    }

    public ResultadoOperacion ObtenerTodos()
    {
        try
        {
            var productos = _productoDAO.ObtenerTodos();
            if (productos.Count == 0) return ResultadoOperacion.Exito("No hay productos en el catálogo.", new List<object>());
            var resultado = productos.Select(p => new
            {
                p.IdProducto, p.Codigo, p.Nombre, p.Descripcion,
                EsControlado = p.EsControlado == 'S',
                p.StockMinimo, p.EtiquetaDescuento,
                DescuentoProximidadVencimiento = p.DescuentoProximidadVencimiento.HasValue ? $"{p.DescuentoProximidadVencimiento.Value:P0}" : "Usa descuento general"
            }).ToList();
            return ResultadoOperacion.Exito($"{resultado.Count} producto(s) encontrados.", resultado);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerPorCodigo(string codigo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(codigo)) return ResultadoOperacion.Fallo("Código obligatorio.");
            var producto = _productoDAO.ObtenerPorCodigo(codigo.Trim().ToUpper());
            if (producto is null) return ResultadoOperacion.Fallo($"Producto con código '{codigo}' no encontrado.");
            return ResultadoOperacion.Exito($"Producto '{producto.Nombre}' encontrado.", producto);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerPorId(int id)
    {
        try
        {
            if (id <= 0) return ResultadoOperacion.Fallo("ID no válido.");
            var producto = _productoDAO.ObtenerPorId(id);
            if (producto is null) return ResultadoOperacion.Fallo($"Producto con ID {id} no encontrado.");
            return ResultadoOperacion.Exito($"Producto '{producto.Nombre}' encontrado.", producto);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion ObtenerPorNombre(string nombre)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombre)) return ResultadoOperacion.Fallo("Nombre obligatorio.");
            var producto = _productoDAO.ObtenerPorNombre(nombre.Trim());
            if (producto is null) return ResultadoOperacion.Fallo($"Producto '{nombre}' no encontrado.");
            return ResultadoOperacion.Exito($"Producto '{producto.Nombre}' encontrado.", producto);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion Insertar(Producto producto)
    {
        try
        {
            if (producto is null) return ResultadoOperacion.Fallo("Datos obligatorios.");
            if (string.IsNullOrWhiteSpace(producto.Codigo)) return ResultadoOperacion.Fallo("Código obligatorio.");
            if (string.IsNullOrWhiteSpace(producto.Nombre)) return ResultadoOperacion.Fallo("Nombre obligatorio.");
            if (string.IsNullOrWhiteSpace(producto.Descripcion)) return ResultadoOperacion.Fallo("Descripción obligatoria.");
            if (producto.StockMinimo < 0) return ResultadoOperacion.Fallo("Stock mínimo no puede ser negativo.");
            if (producto.DescuentoProximidadVencimiento.HasValue && (producto.DescuentoProximidadVencimiento.Value <= 0 || producto.DescuentoProximidadVencimiento.Value > 1))
                return ResultadoOperacion.Fallo("Descuento debe estar entre 0.01 y 1.00.");

            var existente = _productoDAO.ObtenerPorCodigo(producto.Codigo.Trim().ToUpper());
            if (existente is not null) return ResultadoOperacion.Fallo($"Ya existe un producto con código '{producto.Codigo}'.");

            producto.Codigo = producto.Codigo.Trim().ToUpper();
            producto.Nombre = producto.Nombre.Trim();
            producto.Descripcion = producto.Descripcion.Trim();
            _productoDAO.Insertar(producto);
            return ResultadoOperacion.Exito($"Producto '{producto.Nombre}' registrado.", producto);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion Actualizar(Producto producto)
    {
        try
        {
            if (producto is null) return ResultadoOperacion.Fallo("Datos obligatorios.");
            if (producto.IdProducto <= 0) return ResultadoOperacion.Fallo("ID no válido.");
            if (string.IsNullOrWhiteSpace(producto.Nombre)) return ResultadoOperacion.Fallo("Nombre obligatorio.");
            if (string.IsNullOrWhiteSpace(producto.Descripcion)) return ResultadoOperacion.Fallo("Descripción obligatoria.");
            if (producto.StockMinimo < 0) return ResultadoOperacion.Fallo("Stock mínimo no puede ser negativo.");

            var existente = _productoDAO.ObtenerPorId(producto.IdProducto);
            if (existente is null) return ResultadoOperacion.Fallo($"Producto con ID {producto.IdProducto} no encontrado.");

            producto.Nombre = producto.Nombre.Trim();
            producto.Descripcion = producto.Descripcion.Trim();
            _productoDAO.Actualizar(producto);
            return ResultadoOperacion.Exito($"Producto '{producto.Nombre}' actualizado.", producto);
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }

    public ResultadoOperacion Eliminar(int id)
    {
        try
        {
            if (id <= 0) return ResultadoOperacion.Fallo("ID no válido.");
            var producto = _productoDAO.ObtenerPorId(id);
            if (producto is null) return ResultadoOperacion.Fallo($"Producto con ID {id} no encontrado.");

            var lotesActivos = _loteDAO.ObtenerPorProducto(id).Where(l => l.Estado == 'A' && l.CantidadActual > 0).ToList();
            if (lotesActivos.Count > 0)
            {
                int totalUnidades = lotesActivos.Sum(l => l.CantidadActual);
                return ResultadoOperacion.Fallo($"No se puede eliminar '{producto.Nombre}': tiene {lotesActivos.Count} lote(s) activo(s) con {totalUnidades} unidad(es).");
            }

            _productoDAO.Eliminar(id);
            return ResultadoOperacion.Exito($"Producto '{producto.Nombre}' dado de baja.", new { Id = id, producto.Codigo, producto.Nombre });
        }
        catch (Exception ex) { return ResultadoOperacion.Fallo(ex); }
    }
}