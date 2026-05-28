using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class ClientesForm : Form
    {
        public ClientesForm()
        {
            InitializeComponent();
            CargarColores();
        }

        private void CargarColores()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btn.BackColor = Colores.ColorPrimario;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Colores.ColorSecundario;
                }
            }
            lblRegistroCliente.ForeColor = Colores.ColorSecundario;
            lblTablaClientes.ForeColor = Colores.ColorSecundario;
            btnBuscarCliente.BackColor = Colores.ColorPrimario;
            btnEditarCliente.BackColor = Colores.ColorPrimario;
            btnFidelizacion.BackColor = Colores.ColorPrimario;
            btnHistorialCompras.BackColor = Colores.ColorPrimario;
            btnRegistrarCliente.BackColor = Colores.ColorPrimario;
            btnEliminarCliente.BackColor = Colores.ColorPrimario;


        }
    }
}
