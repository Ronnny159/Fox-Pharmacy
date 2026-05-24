using System;
using GUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GUI
{
    public partial class MainForm : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;

        public MainForm()
        {
            InitializeComponent();
            random = new Random();
            btnCerrarFormHijo.Visible = false;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private Color SelectThemeColor()
        {
            int index = random.Next(Colores.ListaColores.Count);
            while (tempIndex == index)
            {
               index = random.Next(Colores.ListaColores.Count);
            }
            tempIndex = index;
            string color = Colores.ListaColores[index];
            return ColorTranslator.FromHtml(color);
        }

        private void ActivateButton(object senderBtn)
        {
            if (senderBtn != null)
            {
                if (currentButton != (Button)senderBtn)
                {
                    DisableButton();
                    Color color = SelectThemeColor();
                    currentButton = (Button)senderBtn;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Segoe UI Semilight", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    panelTitulo.BackColor = color;
                    panelLogo.BackColor = Colores.ChangeColorBrightness(color, -0.2f);
                    Colores.ColorPrimario = color;
                    Colores.ColorSecundario = Colores.ChangeColorBrightness(color, -0.2f);
                    btnCerrarFormHijo.Visible = true;
                }
            }
        }

        private void DisableButton()
        {
            foreach (Control previousBtn in panelMenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Segoe UI Semilight", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }

        }

        private void AbrirFormHijo(Form childForm, object senderBtn)
        {
            if(activeForm != null)
            {
                activeForm.Close();
            }
            ActivateButton(senderBtn);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelInicio.Controls.Add(childForm);
            this.panelInicio.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitulo.Text = childForm.Text;
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            AbrirFormHijo(new GUI.ProductosForm(), sender);
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            AbrirFormHijo(new GUI.CategoriasForm(), sender);
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormHijo(new GUI.ProveedoresForm(), sender);
        }

        private void btnMovimientos_Click(object sender, EventArgs e)
        {
            AbrirFormHijo(new GUI.MovimientosForm(), sender);
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormHijo(new GUI.UsuariosForm(), sender);
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirFormHijo(new GUI.ReportesForm(), sender);
        }

        private void btnCerrarFormHijo_Click(object sender, EventArgs e)
        {
            if(activeForm!=null)
                activeForm.Close();
            Reset();
        }

        private void Reset()
        {
            DisableButton();
            lblTitulo.Text = "INICIO";
            panelTitulo.BackColor = Color.FromArgb(0, 120, 215);
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);
            currentButton = null;
            btnCerrarFormHijo.Visible = false;
        }

        private void panelTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
