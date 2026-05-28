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
    public partial class VentasForm : Form
    {
        public VentasForm()
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
            lblBuscar.ForeColor = Colores.ColorSecundario;
            lblCarrito.ForeColor = Colores.ColorSecundario;
            lblTablaProductos.ForeColor = Colores.ColorSecundario;
            btnAgregarProd.BackColor = Colores.ColorPrimario;
            btnAsignarCliente.BackColor = Colores.ColorPrimario;
            btnBuscarCodigo.BackColor = Colores.ColorPrimario;
            btnBuscarNombre.BackColor = Colores.ColorPrimario;
            btnCompletarVenta.BackColor = Colores.ColorPrimario;
            btnQuitarProd.BackColor = Colores.ColorPrimario;
            btnVaciarCarrito.BackColor = Colores.ColorPrimario;

        }
    }
}
