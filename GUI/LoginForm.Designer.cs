namespace GUI
{
    partial class LoginForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            btnSalir = new System.Windows.Forms.Button();
            panelIniciarSesion = new System.Windows.Forms.Panel();
            btnEntrar = new System.Windows.Forms.Button();
            panelContraseña = new System.Windows.Forms.Panel();
            txtContraseña = new System.Windows.Forms.TextBox();
            pictureBox3 = new System.Windows.Forms.PictureBox();
            panelUsuario = new System.Windows.Forms.Panel();
            txtUsuario = new System.Windows.Forms.TextBox();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            label4 = new System.Windows.Forms.Label();
            panelIzquierdo = new System.Windows.Forms.Panel();
            label6 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            panelIniciarSesion.SuspendLayout();
            panelContraseña.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            panelUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panelIzquierdo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnSalir
            // 
            btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnSalir.ForeColor = System.Drawing.Color.CornflowerBlue;
            btnSalir.Location = new System.Drawing.Point(807, 0);
            btnSalir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new System.Drawing.Size(69, 52);
            btnSalir.TabIndex = 12;
            btnSalir.Text = "X";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click_1;
            // 
            // panelIniciarSesion
            // 
            panelIniciarSesion.Controls.Add(btnEntrar);
            panelIniciarSesion.Controls.Add(panelContraseña);
            panelIniciarSesion.Controls.Add(panelUsuario);
            panelIniciarSesion.Controls.Add(label4);
            panelIniciarSesion.Location = new System.Drawing.Point(350, 48);
            panelIniciarSesion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelIniciarSesion.Name = "panelIniciarSesion";
            panelIniciarSesion.Size = new System.Drawing.Size(525, 563);
            panelIniciarSesion.TabIndex = 15;
            // 
            // btnEntrar
            // 
            btnEntrar.BackColor = System.Drawing.SystemColors.MenuHighlight;
            btnEntrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEntrar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnEntrar.ForeColor = System.Drawing.Color.White;
            btnEntrar.Location = new System.Drawing.Point(184, 333);
            btnEntrar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.Size = new System.Drawing.Size(173, 40);
            btnEntrar.TabIndex = 16;
            btnEntrar.Text = "Entrar";
            btnEntrar.UseVisualStyleBackColor = false;
            btnEntrar.Click += btnEntrar_Click;
            // 
            // panelContraseña
            // 
            panelContraseña.Controls.Add(txtContraseña);
            panelContraseña.Controls.Add(pictureBox3);
            panelContraseña.Location = new System.Drawing.Point(0, 225);
            panelContraseña.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelContraseña.Name = "panelContraseña";
            panelContraseña.Size = new System.Drawing.Size(524, 62);
            panelContraseña.TabIndex = 18;
            // 
            // txtContraseña
            // 
            txtContraseña.BackColor = System.Drawing.Color.GhostWhite;
            txtContraseña.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtContraseña.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txtContraseña.Location = new System.Drawing.Point(92, 17);
            txtContraseña.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.Size = new System.Drawing.Size(419, 22);
            txtContraseña.TabIndex = 2;
            txtContraseña.UseSystemPasswordChar = true;
            txtContraseña.Click += txtContraseña_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.PsswrdIcon;
            pictureBox3.Location = new System.Drawing.Point(15, 0);
            pictureBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new System.Drawing.Size(66, 62);
            pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 0;
            pictureBox3.TabStop = false;
            pictureBox3.MouseDown += pictureBox3_MouseDown;
            pictureBox3.MouseUp += pictureBox3_MouseUp;
            // 
            // panelUsuario
            // 
            panelUsuario.BackColor = System.Drawing.Color.White;
            panelUsuario.Controls.Add(txtUsuario);
            panelUsuario.Controls.Add(pictureBox2);
            panelUsuario.Location = new System.Drawing.Point(0, 156);
            panelUsuario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelUsuario.Name = "panelUsuario";
            panelUsuario.Size = new System.Drawing.Size(524, 62);
            panelUsuario.TabIndex = 17;
            // 
            // txtUsuario
            // 
            txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtUsuario.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            txtUsuario.Location = new System.Drawing.Point(92, 18);
            txtUsuario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new System.Drawing.Size(419, 22);
            txtUsuario.TabIndex = 1;
            txtUsuario.Click += txtUsuario_Click;
            txtUsuario.TextChanged += txtUsuario_TextChanged;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.UserIcon;
            pictureBox2.Location = new System.Drawing.Point(15, 0);
            pictureBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(66, 62);
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label4.ForeColor = System.Drawing.Color.Black;
            label4.Location = new System.Drawing.Point(66, 102);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(130, 25);
            label4.TabIndex = 15;
            label4.Text = "Iniciar Sesión";
            // 
            // panelIzquierdo
            // 
            panelIzquierdo.BackColor = System.Drawing.Color.Transparent;
            panelIzquierdo.BackgroundImage = Properties.Resources.OAICF90;
            panelIzquierdo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            panelIzquierdo.Controls.Add(label6);
            panelIzquierdo.Controls.Add(panel1);
            panelIzquierdo.Controls.Add(label3);
            panelIzquierdo.Controls.Add(label2);
            panelIzquierdo.Controls.Add(label1);
            panelIzquierdo.Controls.Add(pictureBox1);
            panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Left;
            panelIzquierdo.Location = new System.Drawing.Point(0, 0);
            panelIzquierdo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Size = new System.Drawing.Size(350, 612);
            panelIzquierdo.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label6.ForeColor = System.Drawing.Color.White;
            label6.Location = new System.Drawing.Point(130, 428);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(101, 17);
            label6.TabIndex = 11;
            label6.Text = "para tu farmacia";
            // 
            // panel1
            // 
            panel1.Location = new System.Drawing.Point(350, 48);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(526, 563);
            panel1.TabIndex = 15;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            label3.Location = new System.Drawing.Point(140, 338);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(80, 17);
            label3.TabIndex = 10;
            label3.Text = "Bienvenido a";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label2.ForeColor = System.Drawing.Color.White;
            label2.Location = new System.Drawing.Point(126, 404);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(111, 17);
            label2.TabIndex = 9;
            label2.Text = "Control inteligente";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(96, 358);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(169, 32);
            label1.TabIndex = 8;
            label1.Text = "PharmaSmart";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.Image = Properties.Resources.logo123;
            pictureBox1.Location = new System.Drawing.Point(97, 152);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(167, 157);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.GhostWhite;
            ClientSize = new System.Drawing.Size(875, 612);
            ControlBox = false;
            Controls.Add(btnSalir);
            Controls.Add(panelIniciarSesion);
            Controls.Add(panelIzquierdo);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Login";
            panelIniciarSesion.ResumeLayout(false);
            panelIniciarSesion.PerformLayout();
            panelContraseña.ResumeLayout(false);
            panelContraseña.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            panelUsuario.ResumeLayout(false);
            panelUsuario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panelIzquierdo.ResumeLayout(false);
            panelIzquierdo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelIzquierdo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelIniciarSesion;
        private System.Windows.Forms.Panel panelUsuario;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelContraseña;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox txtContraseña;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Button btnEntrar;
    }
}

