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
    public partial class InventarioForm : Form
    {
        public InventarioForm()
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

            lblRegistroLote.ForeColor = Colores.ColorSecundario;
            lblExistencias.ForeColor = Colores.ColorSecundario;
            lblVencimientos.ForeColor = Colores.ColorSecundario;
            btnAgregarLote.BackColor = Colores.ColorPrimario;
            btnBuscarLote.BackColor = Colores.ColorPrimario;
            btnModificarLote.BackColor = Colores.ColorPrimario;
            btnEliminarLote.BackColor = Colores.ColorPrimario;

        }

    }
}
