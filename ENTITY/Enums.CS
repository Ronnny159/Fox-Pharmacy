using System;

namespace Entity;

public enum EstadoLote
{
    Activo = 1,
    Agotado = 2,
    Vencido = 3,
    EnCuarentena = 4
}

public enum TipoAjuste
{
    Averia = 1,
    Vencido = 2,
    RetiroLegal = 3,
    ConteoCiclico = 4
}

public enum RolUsuario
{
    Administrador = 1,
    Cajero = 2,
    Farmaceutico = 3,
    Auditor = 4
}