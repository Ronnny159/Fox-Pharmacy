using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.Repositories;

public class ClienteRepository : BaseRepository, IClienteRepository
{
    public ClienteRepository(IOracleConnectionFactory conexionFactory)
        : base(conexionFactory) { }

    public Cliente? ObtenerPorDocumento(string documento)
    {
        const string sql = @"
            SELECT ID, DOCUMENTO, NOMBRE_COMPLETO, TELEFONO, CORREO, CHAT_ID, MEDICAMENTO_RECURRENTE,
                   ACTIVO, FECHA_CREACION
            FROM CLIENTE 
            WHERE DOCUMENTO = :doc AND ACTIVO = 1";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("doc", OracleDbType.Varchar2).Value = documento,
            MapearCliente);
    }

    public Cliente? ObtenerPorId(int id)
    {
        const string sql = @"
            SELECT ID, DOCUMENTO, NOMBRE_COMPLETO, TELEFONO, CORREO, CHAT_ID, MEDICAMENTO_RECURRENTE,
                   ACTIVO, FECHA_CREACION
            FROM CLIENTE 
            WHERE ID = :id";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("id", OracleDbType.Int32).Value = id,
            MapearCliente);
    }

    public Cliente? ObtenerPorChatId(string chatId)
    {
        const string sql = @"
            SELECT ID, DOCUMENTO, NOMBRE_COMPLETO, TELEFONO, CORREO, CHAT_ID, MEDICAMENTO_RECURRENTE,
                   ACTIVO, FECHA_CREACION
            FROM CLIENTE 
            WHERE CHAT_ID = :chatId AND ACTIVO = 1";

        return EjecutarConsulta(sql,
            cmd => cmd.Parameters.Add("chatId", OracleDbType.Varchar2).Value = chatId,
            MapearCliente);
    }

    public IEnumerable<Cliente> ObtenerTodosFidelizacion()
    {
        const string sql = @"
            SELECT ID, DOCUMENTO, NOMBRE_COMPLETO, TELEFONO, CORREO, CHAT_ID, MEDICAMENTO_RECURRENTE,
                   ACTIVO, FECHA_CREACION
            FROM CLIENTE 
            WHERE ACTIVO = 1 AND CHAT_ID IS NOT NULL
            ORDER BY NOMBRE_COMPLETO";

        return EjecutarConsultaLista(sql, cmd => { }, MapearCliente);
    }

    public void Insertar(Cliente cliente)
    {
        const string sql = @"
            INSERT INTO CLIENTE (DOCUMENTO, NOMBRE_COMPLETO, TELEFONO, CORREO, CHAT_ID, MEDICAMENTO_RECURRENTE)
            VALUES (:doc, :nombre, :tel, :correo, :chatId, :medRecurrente)";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("doc", OracleDbType.Varchar2).Value = cliente.Documento;
            cmd.Parameters.Add("nombre", OracleDbType.Varchar2).Value = cliente.NombreCompleto;
            cmd.Parameters.Add("tel", OracleDbType.Varchar2).Value = cliente.Telefono;
            cmd.Parameters.Add("correo", OracleDbType.Varchar2).Value = cliente.Correo ?? (object)DBNull.Value;
            cmd.Parameters.Add("chatId", OracleDbType.Varchar2).Value = cliente.ChatId ?? (object)DBNull.Value;
            cmd.Parameters.Add("medRecurrente", OracleDbType.Varchar2).Value = cliente.MedicamentoRecurrente ?? (object)DBNull.Value;
        });
    }

    public void Actualizar(Cliente cliente)
    {
        const string sql = @"
            UPDATE CLIENTE 
            SET NOMBRE_COMPLETO = :nombre, TELEFONO = :tel, CORREO = :correo,
                CHAT_ID = :chatId, MEDICAMENTO_RECURRENTE = :medRecurrente
            WHERE ID = :id";

        EjecutarComando(sql, cmd =>
        {
            cmd.Parameters.Add("id", OracleDbType.Int32).Value = cliente.Id;
            cmd.Parameters.Add("nombre", OracleDbType.Varchar2).Value = cliente.NombreCompleto;
            cmd.Parameters.Add("tel", OracleDbType.Varchar2).Value = cliente.Telefono;
            cmd.Parameters.Add("correo", OracleDbType.Varchar2).Value = cliente.Correo ?? (object)DBNull.Value;
            cmd.Parameters.Add("chatId", OracleDbType.Varchar2).Value = cliente.ChatId ?? (object)DBNull.Value;
            cmd.Parameters.Add("medRecurrente", OracleDbType.Varchar2).Value = cliente.MedicamentoRecurrente ?? (object)DBNull.Value;
        });
    }

    private Cliente MapearCliente(OracleDataReader reader)
    {
        return new Cliente
        {
            Id = reader.GetInt32(reader.GetOrdinal("ID")),
            Documento = LeerString(reader, "DOCUMENTO"),
            NombreCompleto = LeerString(reader, "NOMBRE_COMPLETO"),
            Telefono = LeerString(reader, "TELEFONO"),
            Correo = LeerString(reader, "CORREO"),
            ChatId = LeerString(reader, "CHAT_ID"),
            MedicamentoRecurrente = LeerString(reader, "MEDICAMENTO_RECURRENTE"),
            Activo = LeerBooleano(reader, "ACTIVO"),
            FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FECHA_CREACION"))
        };
    }
}