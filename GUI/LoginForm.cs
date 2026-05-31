using BLL.Interfaces;
using BLL.DTOs;
using BLL.Services;
using DAL.DAO;
using DAL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    [SupportedOSPlatform("windows")]
    public partial class LoginForm : Form
    {
        private readonly IUsuarioService _usuarioService;

        public LoginForm()
        {
            InitializeComponent();
            var usuarioDAO = new UsuarioDAO(OracleConnectionManager.Instancia);
            _usuarioService = new UsuarioService(usuarioDAO);
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

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
           
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            ResultadoOperacion resultado = _usuarioService.Autenticar(
                txtUsuario.Text,
                txtContraseña.Text);

            if (!resultado.Exitoso)
            {
                MessageBox.Show(resultado.Mensaje, "Error de acceso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AbrirMainForm(this);
        }

        private void AbrirMainForm(Form form)
        {
            MainForm main = new MainForm();
            main.Show();
            this.Hide();
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt1erNombre_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
