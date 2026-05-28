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
    public partial class ProductosForm : Form
    {
        public ProductosForm()
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

            btnAgregarProducto.BackColor = Colores.ColorPrimario;
            btnDetallesProducto.BackColor = Colores.ColorPrimario;
            btnEliminarProductos.BackColor = Colores.ColorPrimario;
            btnBuscarPorCodigo.BackColor = Colores.ColorPrimario;
            lblAgregarProductos.ForeColor = Colores.ColorSecundario;
            lblTablaProductos.ForeColor = Colores.ColorSecundario;
            lblBuscarPorId.ForeColor = Colores.ColorSecundario;
            
        }

        private void btnDetallesProducto_Click(object sender, EventArgs e)
        {
            DetallesProductoForm main = new DetallesProductoForm();

            main.Show();

            this.Hide();
        }
    }
}
