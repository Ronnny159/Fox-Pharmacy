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
    public partial class ConfiguracionForm : Form
    {
        public ConfiguracionForm()
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
            lblCambiarParametros.ForeColor = Colores.ColorPrimario;
            lblHistorialCambios.ForeColor = Colores.ColorPrimario;
            btnGuardarCambios.BackColor = Colores.ColorSecundario;
            btnRestaurarCambios.BackColor = Colores.ColorSecundario;
        }
    }
}
