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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

        }


        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtUsuario_Click(object sender, EventArgs e)
        {
            txtUsuario.BackColor = Color.White;
            panelUsuario.BackColor = Color.White;
            panelContraseña.BackColor = Color.GhostWhite;
            txtContraseña.BackColor = Color.GhostWhite;
        }

        private void txtContraseña_Click(object sender, EventArgs e)
        {
            txtUsuario.BackColor = Color.GhostWhite;
            panelUsuario.BackColor = Color.GhostWhite;
            panelContraseña.BackColor = Color.White;
            txtContraseña.BackColor = Color.White;
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            txtContraseña.UseSystemPasswordChar = false;
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            txtContraseña.UseSystemPasswordChar = true;
        }
    }
}
