using BLL;
using DAL.Interfaces;
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
        private readonly IUsuarioDAO _usuarioDAO;

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

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            panelRegistrarse.Visible = true;

        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            panelRegistrarse.Visible = false;
        }

        private void txtNombre_Click(object sender, EventArgs e)
        {
            txt1erNombre.BackColor = Color.White;
            panel1erNombre.BackColor = Color.White;
            txt2doNombre.BackColor = Color.GhostWhite;
            panel2doNombre.BackColor = Color.GhostWhite;
            txt1erApellido.BackColor = Color.GhostWhite;
            panel1erApellido.BackColor = Color.GhostWhite;
            txt2doApellido.BackColor = Color.GhostWhite;
            panel2doApellido.BackColor = Color.GhostWhite;
            txtUsuarioRegistro.BackColor = Color.GhostWhite;
            panelUsuarioRegistro .BackColor = Color.GhostWhite;
            txtContraseñaRegistro .BackColor = Color.GhostWhite;
            panelContraseñaRegistro.BackColor = Color.GhostWhite;
        }

        private void txt2doNombre_Click(object sender, EventArgs e)
        {
            txt1erNombre.BackColor = Color.GhostWhite;
            panel1erNombre.BackColor = Color.GhostWhite;
            txt2doNombre.BackColor = Color.White;
            panel2doNombre.BackColor = Color.White;
            txt1erApellido.BackColor = Color.GhostWhite;
            panel1erApellido.BackColor = Color.GhostWhite;
            txt2doApellido.BackColor = Color.GhostWhite;
            panel2doApellido.BackColor = Color.GhostWhite;
            txtUsuarioRegistro.BackColor = Color.GhostWhite;
            panelUsuarioRegistro.BackColor = Color.GhostWhite;
            txtContraseñaRegistro.BackColor = Color.GhostWhite;
            panelContraseñaRegistro.BackColor = Color.GhostWhite;
        }

        private void txt1erApellido_Click(object sender, EventArgs e)
        {
            txt1erNombre.BackColor = Color.GhostWhite;
            panel1erNombre.BackColor = Color.GhostWhite;
            txt2doNombre.BackColor = Color.GhostWhite;
            panel2doNombre.BackColor = Color.GhostWhite;
            txt1erApellido.BackColor = Color.White;
            panel1erApellido.BackColor = Color.White;
            txt2doApellido.BackColor = Color.GhostWhite;
            panel2doApellido.BackColor = Color.GhostWhite;
            txtUsuarioRegistro.BackColor = Color.GhostWhite;
            panelUsuarioRegistro.BackColor = Color.GhostWhite;
            txtContraseñaRegistro.BackColor = Color.GhostWhite;
            panelContraseñaRegistro.BackColor = Color.GhostWhite;
        }

        private void txt2doApellido_Click(object sender, EventArgs e)
        {
            txt1erNombre.BackColor = Color.GhostWhite;
            panel1erNombre.BackColor = Color.GhostWhite;
            txt2doNombre.BackColor = Color.GhostWhite;
            panel2doNombre.BackColor = Color.GhostWhite;
            txt1erApellido.BackColor = Color.GhostWhite;
            panel1erApellido.BackColor = Color.GhostWhite;
            txt2doApellido.BackColor = Color.White;
            panel2doApellido.BackColor = Color.White;
            txtUsuarioRegistro.BackColor = Color.GhostWhite;
            panelUsuarioRegistro.BackColor = Color.GhostWhite;
            txtContraseñaRegistro.BackColor = Color.GhostWhite;
            panelContraseñaRegistro.BackColor = Color.GhostWhite;
        }

        private void txtUsuarioRegistro_Click(object sender, EventArgs e)
        {
            txt1erNombre.BackColor = Color.GhostWhite;
            panel1erNombre.BackColor = Color.GhostWhite;
            txt2doNombre.BackColor = Color.GhostWhite;
            panel2doNombre.BackColor = Color.GhostWhite;
            txt1erApellido.BackColor = Color.GhostWhite;
            panel1erApellido.BackColor = Color.GhostWhite;
            txt2doApellido.BackColor = Color.GhostWhite;
            panel2doApellido.BackColor = Color.GhostWhite;
            txtUsuarioRegistro.BackColor = Color.White;
            panelUsuarioRegistro.BackColor = Color.White;
            txtContraseñaRegistro.BackColor = Color.GhostWhite;
            panelContraseñaRegistro.BackColor = Color.GhostWhite;
        }

        private void txtContraseñaRegistro_Click(object sender, EventArgs e)
        {
            txt1erNombre.BackColor = Color.GhostWhite;
            panel1erNombre.BackColor = Color.GhostWhite;
            txt2doNombre.BackColor = Color.GhostWhite;
            panel2doNombre.BackColor = Color.GhostWhite;
            txt1erApellido.BackColor = Color.GhostWhite;
            panel1erApellido.BackColor = Color.GhostWhite;
            txt2doApellido.BackColor = Color.GhostWhite;
            panel2doApellido.BackColor = Color.GhostWhite;
            txtUsuarioRegistro.BackColor = Color.GhostWhite;
            panelUsuarioRegistro.BackColor = Color.GhostWhite;
            txtContraseñaRegistro.BackColor = Color.White;
            panelContraseñaRegistro.BackColor = Color.White;
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            AbrirMainForm(this);
        }

        private void btnCrearCuenta_Click(object sender, EventArgs e)
        {
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
    }
}
