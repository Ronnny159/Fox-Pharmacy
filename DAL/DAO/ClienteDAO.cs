using Oracle.ManagedDataAccess.Client;
using DAL.Core;
using DAL.Interfaces;
using Entity;

namespace DAL.DAO;

public class ClienteDAO : BaseDAO, IClienteDAO
{
    public ClienteDAO(IOracleConnectionFactory conexionFactory) : base(conexionFactory) { }

    public Cliente? ObtenerPorDocumento(string documento)
    {
        Cliente? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_CLIENTE_POR_DOCUMENTO",
            cmd => cmd.Parameters.Add("p_doc", OracleDbType.Varchar2).Value = documento,
            reader => { if (reader.Read()) resultado = MapearCliente(reader); });
        return resultado;
    }

    public Cliente? ObtenerPorId(int id)
    {
        Cliente? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_CLIENTE_POR_ID",
            cmd => cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = id,
            reader => { if (reader.Read()) resultado = MapearCliente(reader); });
        return resultado;
    }

    public Cliente? ObtenerPorChatId(string chatId)
    {
        Cliente? resultado = null;
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_CLIENTE_POR_CHAT_ID",
            cmd => cmd.Parameters.Add("p_chat", OracleDbType.Varchar2).Value = chatId,
            reader => { if (reader.Read()) resultado = MapearCliente(reader); });
        return resultado;
    }

    public List<Cliente> ObtenerTodosFidelizacion()
    {
        var lista = new List<Cliente>();
        EjecutarCursor("PKG_PHARMASMART_CONFIG.OBTENER_CLIENTES_FIDELIZACION",
            cmd => { },
            reader => { while (reader.Read()) lista.Add(MapearCliente(reader)); });
        return lista;
    }

    public void Insertar(Cliente cliente)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.INSERTAR_CLIENTE", cmd =>
        {
            cmd.Parameters.Add("p_doc", OracleDbType.Varchar2).Value = cliente.Documento;
            cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = cliente.NombreCompleto;
            cmd.Parameters.Add("p_tel", OracleDbType.Varchar2).Value = cliente.Telefono;
            cmd.Parameters.Add("p_correo", OracleDbType.Varchar2).Value = cliente.Correo ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_chat", OracleDbType.Varchar2).Value = cliente.ChatId ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_med", OracleDbType.Varchar2).Value = cliente.MedicamentoRecurrente ?? (object)DBNull.Value;
        });
    }

    public void Actualizar(Cliente cliente)
    {
        EjecutarProcedimiento("PKG_PHARMASMART_CONFIG.ACTUALIZAR_CLIENTE", cmd =>
        {
            cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = cliente.IdCliente;
            cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = cliente.NombreCompleto;
            cmd.Parameters.Add("p_tel", OracleDbType.Varchar2).Value = cliente.Telefono;
            cmd.Parameters.Add("p_correo", OracleDbType.Varchar2).Value = cliente.Correo ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_chat", OracleDbType.Varchar2).Value = cliente.ChatId ?? (object)DBNull.Value;
            cmd.Parameters.Add("p_med", OracleDbType.Varchar2).Value = cliente.MedicamentoRecurrente ?? (object)DBNull.Value;
        });
    }

    private Cliente MapearCliente(OracleDataReader reader)
    {
        return new Cliente
        {
            IdCliente = reader.GetInt32(reader.GetOrdinal("ID_CLIENTE")),
            Documento = LeerString(reader, "DOCUMENTO"),
            NombreCompleto = LeerString(reader, "NOMBRE_COMPLETO"),
            Telefono = LeerString(reader, "TELEFONO"),
            Correo = LeerString(reader, "CORREO"),
            ChatId = LeerString(reader, "CHAT_ID"),
            MedicamentoRecurrente = LeerString(reader, "MEDICAMENTO_RECURRENTE"),
            Estado = LeerChar(reader, "ESTADO")
        };
    }

    IEnumerable<Cliente> IClienteDAO.ObtenerTodosFidelizacion()
    {
        return ObtenerTodosFidelizacion();
    }
}