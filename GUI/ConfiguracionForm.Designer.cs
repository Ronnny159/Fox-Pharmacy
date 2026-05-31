namespace GUI
{
    partial class ConfiguracionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtDiasVencimiento = new System.Windows.Forms.TextBox();
            lblCambiarParametros = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            txtDescuentoAuto = new System.Windows.Forms.TextBox();
            txtIVA = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            txtStockMin = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            btnGuardarCambios = new System.Windows.Forms.Button();
            btnRestaurarCambios = new System.Windows.Forms.Button();
            listHistorialCambios = new System.Windows.Forms.ListBox();
            lblHistorialCambios = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // txtDiasVencimiento
            // 
            txtDiasVencimiento.Location = new System.Drawing.Point(72, 100);
            txtDiasVencimiento.Name = "txtDiasVencimiento";
            txtDiasVencimiento.Size = new System.Drawing.Size(280, 23);
            txtDiasVencimiento.TabIndex = 0;
            // 
            // lblCambiarParametros
            // 
            lblCambiarParametros.AutoSize = true;
            lblCambiarParametros.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblCambiarParametros.Location = new System.Drawing.Point(57, 31);
            lblCambiarParametros.Name = "lblCambiarParametros";
            lblCambiarParametros.Size = new System.Drawing.Size(128, 17);
            lblCambiarParametros.TabIndex = 1;
            lblCambiarParametros.Text = "Cambiar Parámetros";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(72, 71);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(162, 15);
            label2.TabIndex = 2;
            label2.Text = "Dias de alerta de vencimiento";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(72, 140);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(155, 15);
            label3.TabIndex = 3;
            label3.Text = "% de descuento automático";
            // 
            // txtDescuentoAuto
            // 
            txtDescuentoAuto.Location = new System.Drawing.Point(72, 170);
            txtDescuentoAuto.Name = "txtDescuentoAuto";
            txtDescuentoAuto.Size = new System.Drawing.Size(280, 23);
            txtDescuentoAuto.TabIndex = 4;
            // 
            // txtIVA
            // 
            txtIVA.Location = new System.Drawing.Point(72, 244);
            txtIVA.Name = "txtIVA";
            txtIVA.Size = new System.Drawing.Size(280, 23);
            txtIVA.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(72, 215);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(56, 15);
            label4.TabIndex = 5;
            label4.Text = "% del IVA";
            // 
            // txtStockMin
            // 
            txtStockMin.Location = new System.Drawing.Point(72, 318);
            txtStockMin.Name = "txtStockMin";
            txtStockMin.Size = new System.Drawing.Size(280, 23);
            txtStockMin.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(72, 289);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(81, 15);
            label5.TabIndex = 7;
            label5.Text = "Stock mínimo";
            // 
            // btnGuardarCambios
            // 
            btnGuardarCambios.FlatAppearance.BorderSize = 0;
            btnGuardarCambios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGuardarCambios.Location = new System.Drawing.Point(113, 383);
            btnGuardarCambios.Name = "btnGuardarCambios";
            btnGuardarCambios.Size = new System.Drawing.Size(72, 33);
            btnGuardarCambios.TabIndex = 9;
            btnGuardarCambios.Text = "Guardar";
            btnGuardarCambios.UseVisualStyleBackColor = true;
            // 
            // btnRestaurarCambios
            // 
            btnRestaurarCambios.FlatAppearance.BorderSize = 0;
            btnRestaurarCambios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRestaurarCambios.Location = new System.Drawing.Point(257, 383);
            btnRestaurarCambios.Name = "btnRestaurarCambios";
            btnRestaurarCambios.Size = new System.Drawing.Size(72, 33);
            btnRestaurarCambios.TabIndex = 10;
            btnRestaurarCambios.Text = "Restaurar";
            btnRestaurarCambios.UseVisualStyleBackColor = true;
            // 
            // listHistorialCambios
            // 
            listHistorialCambios.FormattingEnabled = true;
            listHistorialCambios.ItemHeight = 15;
            listHistorialCambios.Location = new System.Drawing.Point(445, 71);
            listHistorialCambios.Name = "listHistorialCambios";
            listHistorialCambios.Size = new System.Drawing.Size(310, 334);
            listHistorialCambios.TabIndex = 11;
            // 
            // lblHistorialCambios
            // 
            lblHistorialCambios.AutoSize = true;
            lblHistorialCambios.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblHistorialCambios.Location = new System.Drawing.Point(423, 31);
            lblHistorialCambios.Name = "lblHistorialCambios";
            lblHistorialCambios.Size = new System.Drawing.Size(130, 17);
            lblHistorialCambios.TabIndex = 12;
            lblHistorialCambios.Text = "Historial de Cambios";
            // 
            // ConfiguracionForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(lblHistorialCambios);
            Controls.Add(listHistorialCambios);
            Controls.Add(btnRestaurarCambios);
            Controls.Add(btnGuardarCambios);
            Controls.Add(txtStockMin);
            Controls.Add(label5);
            Controls.Add(txtIVA);
            Controls.Add(label4);
            Controls.Add(txtDescuentoAuto);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblCambiarParametros);
            Controls.Add(txtDiasVencimiento);
            Name = "ConfiguracionForm";
            Text = "ConfiguracionForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtDiasVencimiento;
        private System.Windows.Forms.Label lblCambiarParametros;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDescuentoAuto;
        private System.Windows.Forms.TextBox txtIVA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStockMin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnGuardarCambios;
        private System.Windows.Forms.Button btnRestaurarCambios;
        private System.Windows.Forms.ListBox listHistorialCambios;
        private System.Windows.Forms.Label lblHistorialCambios;
    }
}