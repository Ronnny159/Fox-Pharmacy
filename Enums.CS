using System;

namespace Entity;

/// <summary>
/// Estados posibles de un lote en el inventario farmacéutico.
/// </summary>
public enum EstadoLote
{
    Activo = 1,
    Agotado = 2,
    Vencido = 3,
    EnCuarentena = 4
}

/// <summary>
/// Tipos de ajuste de inventario según reglamentación sanitaria colombiana.
/// </summary>
public enum TipoAjuste
{
    Averia = 1,
    Vencido = 2,
    RetiroLegal = 3,
    ConteoCiclico = 4
}

/// <summary>
/// Roles de usuario para control de acceso y permisos.
/// </summary>
public enum RolUsuario
{
    Administrador = 1,
    Cajero = 2,
    Farmaceutico = 3,
    Auditor = 4
}