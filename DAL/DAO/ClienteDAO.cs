using DAL.Core;
using DAL.Interfaces;
using Entity;
using Oracle.ManagedDataAccess.Client;

namespace DAL.DAO;

public class ClienteDAO : BaseDAO, IClienteDAO
{
    public ClienteDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public Cliente? ObtenerPorDocumento(string documento)
    {
        Cliente? resultado = null;
        EjecutarCursor("SP_OBTENER_CLIENTE_POR_DOCUMENTO",
            cmd => cmd.Parameters.Add("p_documento", OracleDbType.Varchar2).Value = documento,
            reader => { if (reader.Read()) resultado = MapearCliente(reader); });
        return resultado;
    }

    public Cliente? ObtenerPorId(int id)
    {
        Cliente? resultado = null;
        EjecutarCursor("SP_OBTENER_CLIENTE_POR_ID",
            cmd => cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id,
            reader => { if (reader.Read()) resultado = MapearCliente(reader); });
        return resultado;
    }

    public Cliente? ObtenerPorChatId(string chatId)
    {
        Cliente? resultado = null;
        EjecutarCursor("SP_OBTENER_CLIENTE_POR_CHAT_ID",
            cmd => cmd.Parameters.Add("p_chat_id", OracleDbType.Varchar2).Value = chatId,
            reader => { if (reader.Read()) resultado = MapearCliente(reader); });
        return resultado;
    }

    public List<Cliente> ObtenerTodosFidelizacion()
    {
        var lista = new List<Cliente>();
        EjecutarCursor("SP_OBTENER_CLIENTES_FIDELIZACION",
            cmd => { },
            reader => { while (reader.Read()) lista.Add(MapearCliente(reader)); });
        return lista;
    }

    public void Insertar(Cliente cliente)
    {
        EjecutarProcedimiento("SP_INSERTAR_CLIENTE", cmd =>
        {
            cmd.Parameters.Add("p_documento", OracleDbType.Varchar2).Value = cliente.Documento;
            cmd.Parameters.Add("p_nombre_completo", OracleDbType.Varchar2).Value = cliente.NombreCompleto;
            cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = cliente.Telefono;
            cmd.Parameters.Add("p_correo", OracleDbType.Varchar2).Value = cliente.Correo ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_chat_id", OracleDbType.Varchar2).Value = cliente.ChatId ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_medicamento_recurrente", OracleDbType.Varchar2).Value = cliente.MedicamentoRecurrente ?? (object)DBNull.Value;
        });
    }

    public void Actualizar(Cliente cliente)
    {
        EjecutarProcedimiento("SP_ACTUALIZAR_CLIENTE", cmd =>
        {
            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = cliente.Id;
            cmd.Parameters.Add("p_nombre_completo", OracleDbType.Varchar2).Value = cliente.NombreCompleto;
            cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = cliente.Telefono;
            cmd.Parameters.Add("p_correo", OracleDbType.Varchar2).Value = cliente.Correo ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_chat_id", OracleDbType.Varchar2).Value = cliente.ChatId ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_medicamento_recurrente", OracleDbType.Varchar2).Value = cliente.MedicamentoRecurrente ?? (object)DBNull.Value;
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

    IEnumerable<Cliente> IClienteDAO.ObtenerTodosFidelizacion()
    {
        return ObtenerTodosFidelizacion();
    }
}