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
    private readonly IProductoRepository _productoRepository;
    private readonly ILoteRepository _loteRepository;

    public ProductoService(
        IProductoRepository productoRepository,
        ILoteRepository loteRepository)
    {
        _productoRepository = productoRepository;
        _loteRepository = loteRepository;
    }

    /// <summary>
    /// Retorna todos los productos activos del catálogo ordenados por nombre.
    /// </summary>
    public ResultadoOperacion ObtenerTodos()
    {
        try
        {
            var productos = _productoRepository.ObtenerTodos().ToList();

            if (productos.Count == 0)
                return ResultadoOperacion.Exito(
                    "No hay productos registrados en el catálogo.",
                    new List<object>());

            var resultado = productos.Select(p => new
            {
                p.Id,
                p.Codigo,
                p.Nombre,
                p.Descripcion,
                p.EsControlado,
                p.StockMinimo,
                p.EtiquetaDescuento,
                DescuentoProximidadVencimiento = p.DescuentoProximidadVencimiento.HasValue
                    ? $"{p.DescuentoProximidadVencimiento.Value:P0}"
                    : "Usa descuento general"
            }).ToList();

            return ResultadoOperacion.Exito(
                $"Se encontraron {resultado.Count} producto(s) en el catálogo.",
                resultado);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Busca un producto por su código único (registro sanitario o SKU interno).
    /// </summary>
    public ResultadoOperacion ObtenerPorCodigo(string codigo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return ResultadoOperacion.Fallo("El código del producto es obligatorio.");

            var producto = _productoRepository.ObtenerPorCodigo(codigo.Trim().ToUpper());

            if (producto is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún producto con el código '{codigo}'.");

            return ResultadoOperacion.Exito(
                $"Producto '{producto.Nombre}' encontrado.",
                producto);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Busca un producto por su identificador interno.
    /// </summary>
    public ResultadoOperacion ObtenerPorId(int id)
    {
        try
        {
            if (id <= 0)
                return ResultadoOperacion.Fallo("El identificador del producto no es válido.");

            var producto = _productoRepository.ObtenerPorId(id);

            if (producto is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún producto con el ID {id}.");

            return ResultadoOperacion.Exito(
                $"Producto '{producto.Nombre}' encontrado.",
                producto);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Busca un producto por su nombre comercial (búsqueda exacta, sin distinción de mayúsculas).
    /// </summary>
    public ResultadoOperacion ObtenerPorNombre(string nombre)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return ResultadoOperacion.Fallo("El nombre del producto es obligatorio.");

            var producto = _productoRepository.ObtenerPorNombre(nombre.Trim());

            if (producto is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún producto con el nombre '{nombre}'.");

            return ResultadoOperacion.Exito(
                $"Producto '{producto.Nombre}' encontrado.",
                producto);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Registra un nuevo producto en el catálogo maestro.
    /// Valida campos obligatorios, formato del código, stock mínimo
    /// y que el código no esté duplicado en el sistema.
    /// </summary>
    public ResultadoOperacion Insertar(Producto producto)
    {
        try
        {
            if (producto is null)
                return ResultadoOperacion.Fallo("Los datos del producto son obligatorios.");

            if (string.IsNullOrWhiteSpace(producto.Codigo))
                return ResultadoOperacion.Fallo("El código del producto es obligatorio.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                return ResultadoOperacion.Fallo("El nombre del producto es obligatorio.");

            if (string.IsNullOrWhiteSpace(producto.Descripcion))
                return ResultadoOperacion.Fallo("La descripción del producto es obligatoria.");

            if (producto.StockMinimo < 0)
                return ResultadoOperacion.Fallo("El stock mínimo no puede ser negativo.");

            if (producto.DescuentoProximidadVencimiento.HasValue &&
                (producto.DescuentoProximidadVencimiento.Value <= 0 ||
                 producto.DescuentoProximidadVencimiento.Value > 1))
                return ResultadoOperacion.Fallo(
                    "El descuento por proximidad de vencimiento debe estar entre 0.01 (1%) y 1.00 (100%).");

            // Verificar duplicado por código
            var existente = _productoRepository.ObtenerPorCodigo(producto.Codigo.Trim().ToUpper());
            if (existente is not null)
                return ResultadoOperacion.Fallo(
                    $"Ya existe un producto registrado con el código '{producto.Codigo}'. " +
                    $"Los códigos de producto son únicos en el catálogo.");

            // Normalizar datos antes de persistir
            producto.Codigo = producto.Codigo.Trim().ToUpper();
            producto.Nombre = producto.Nombre.Trim();
            producto.Descripcion = producto.Descripcion.Trim();

            _productoRepository.Insertar(producto);

            return ResultadoOperacion.Exito(
                $"Producto '{producto.Nombre}' registrado correctamente en el catálogo.",
                producto);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Actualiza los datos de un producto existente en el catálogo.
    /// El código del producto no puede modificarse una vez registrado;
    /// es un identificador permanente del medicamento.
    /// </summary>
    public ResultadoOperacion Actualizar(Producto producto)
    {
        try
        {
            if (producto is null)
                return ResultadoOperacion.Fallo("Los datos del producto son obligatorios.");

            if (producto.Id <= 0)
                return ResultadoOperacion.Fallo("El identificador del producto no es válido.");

            if (string.IsNullOrWhiteSpace(producto.Nombre))
                return ResultadoOperacion.Fallo("El nombre del producto es obligatorio.");

            if (string.IsNullOrWhiteSpace(producto.Descripcion))
                return ResultadoOperacion.Fallo("La descripción del producto es obligatoria.");

            if (producto.StockMinimo < 0)
                return ResultadoOperacion.Fallo("El stock mínimo no puede ser negativo.");

            if (producto.DescuentoProximidadVencimiento.HasValue &&
                (producto.DescuentoProximidadVencimiento.Value <= 0 ||
                 producto.DescuentoProximidadVencimiento.Value > 1))
                return ResultadoOperacion.Fallo(
                    "El descuento por proximidad de vencimiento debe estar entre 0.01 (1%) y 1.00 (100%).");

            // Verificar que el producto exista
            var existente = _productoRepository.ObtenerPorId(producto.Id);
            if (existente is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún producto con el ID {producto.Id}.");

            // Normalizar datos antes de persistir
            producto.Nombre = producto.Nombre.Trim();
            producto.Descripcion = producto.Descripcion.Trim();

            _productoRepository.Actualizar(producto);

            return ResultadoOperacion.Exito(
                $"Producto '{producto.Nombre}' actualizado correctamente.",
                producto);
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }

    /// <summary>
    /// Realiza una baja lógica del producto (marca como inactivo).
    /// Protege la eliminación si el producto tiene lotes activos con stock disponible,
    /// ya que eliminar un producto con inventario comprometería la trazabilidad
    /// y la integridad de las ventas futuras.
    /// </summary>
    public ResultadoOperacion Eliminar(int id)
    {
        try
        {
            if (id <= 0)
                return ResultadoOperacion.Fallo("El identificador del producto no es válido.");

            var producto = _productoRepository.ObtenerPorId(id);
            if (producto is null)
                return ResultadoOperacion.Fallo(
                    $"No se encontró ningún producto con el ID {id}.");

            // Verificar que no tenga lotes activos con stock antes de eliminar
            var lotesActivos = _loteRepository
                .ObtenerPorProducto(id)
                .Where(l => l.Estado == EstadoLote.Activo && l.CantidadActual > 0)
                .ToList();

            if (lotesActivos.Count > 0)
            {
                int totalUnidades = lotesActivos.Sum(l => l.CantidadActual);
                return ResultadoOperacion.Fallo(
                    $"No se puede eliminar el producto '{producto.Nombre}' porque tiene " +
                    $"{lotesActivos.Count} lote(s) activo(s) con {totalUnidades} unidad(es) en inventario. " +
                    "Agote o ajuste el stock antes de darlo de baja.");
            }

            _productoRepository.Eliminar(id);

            return ResultadoOperacion.Exito(
                $"Producto '{producto.Nombre}' dado de baja correctamente del catálogo.",
                new
                {
                    Id = id,
                    Codigo = producto.Codigo,
                    Nombre = producto.Nombre
                });
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Fallo(ex);
        }
    }
}
