namespace GUI
{
    partial class VentasForm
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
            listCarrito = new System.Windows.Forms.ListBox();
            lblCarrito = new System.Windows.Forms.Label();
            lblBuscar = new System.Windows.Forms.Label();
            txtBuscarCodigo = new System.Windows.Forms.TextBox();
            btnBuscarCodigo = new System.Windows.Forms.Button();
            txtBuscaNombre = new System.Windows.Forms.TextBox();
            btnBuscarNombre = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            listBusqueda = new System.Windows.Forms.ListBox();
            lblTablaProductos = new System.Windows.Forms.Label();
            btnAgregarProd = new System.Windows.Forms.Button();
            btnQuitarProd = new System.Windows.Forms.Button();
            btnVaciarCarrito = new System.Windows.Forms.Button();
            btnCompletarVenta = new System.Windows.Forms.Button();
            btnAsignarCliente = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // listCarrito
            // 
            listCarrito.FormattingEnabled = true;
            listCarrito.ItemHeight = 15;
            listCarrito.Location = new System.Drawing.Point(394, 75);
            listCarrito.Name = "listCarrito";
            listCarrito.Size = new System.Drawing.Size(426, 319);
            listCarrito.TabIndex = 0;
            // 
            // lblCarrito
            // 
            lblCarrito.AutoSize = true;
            lblCarrito.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            lblCarrito.Location = new System.Drawing.Point(394, 33);
            lblCarrito.Name = "lblCarrito";
            lblCarrito.Size = new System.Drawing.Size(48, 17);
            lblCarrito.TabIndex = 1;
            lblCarrito.Text = "Carrito";
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            lblBuscar.Location = new System.Drawing.Point(27, 33);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new System.Drawing.Size(46, 17);
            lblBuscar.TabIndex = 2;
            lblBuscar.Text = "Buscar";
            // 
            // txtBuscarCodigo
            // 
            txtBuscarCodigo.Location = new System.Drawing.Point(61, 93);
            txtBuscarCodigo.Name = "txtBuscarCodigo";
            txtBuscarCodigo.Size = new System.Drawing.Size(190, 23);
            txtBuscarCodigo.TabIndex = 3;
            // 
            // btnBuscarCodigo
            // 
            btnBuscarCodigo.FlatAppearance.BorderSize = 0;
            btnBuscarCodigo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBuscarCodigo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            btnBuscarCodigo.Location = new System.Drawing.Point(288, 86);
            btnBuscarCodigo.Name = "btnBuscarCodigo";
            btnBuscarCodigo.Size = new System.Drawing.Size(75, 32);
            btnBuscarCodigo.TabIndex = 4;
            btnBuscarCodigo.Text = "Buscar";
            btnBuscarCodigo.UseVisualStyleBackColor = true;
            // 
            // txtBuscaNombre
            // 
            txtBuscaNombre.Location = new System.Drawing.Point(61, 160);
            txtBuscaNombre.Name = "txtBuscaNombre";
            txtBuscaNombre.Size = new System.Drawing.Size(190, 23);
            txtBuscaNombre.TabIndex = 5;
            // 
            // btnBuscarNombre
            // 
            btnBuscarNombre.FlatAppearance.BorderSize = 0;
            btnBuscarNombre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBuscarNombre.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            btnBuscarNombre.Location = new System.Drawing.Point(288, 153);
            btnBuscarNombre.Name = "btnBuscarNombre";
            btnBuscarNombre.Size = new System.Drawing.Size(75, 32);
            btnBuscarNombre.TabIndex = 7;
            btnBuscarNombre.Text = "Buscar";
            btnBuscarNombre.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(61, 65);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(67, 15);
            label3.TabIndex = 9;
            label3.Text = "Por Código";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(61, 132);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(72, 15);
            label4.TabIndex = 10;
            label4.Text = "Por Nombre";
            // 
            // listBusqueda
            // 
            listBusqueda.FormattingEnabled = true;
            listBusqueda.ItemHeight = 15;
            listBusqueda.Location = new System.Drawing.Point(61, 239);
            listBusqueda.Name = "listBusqueda";
            listBusqueda.Size = new System.Drawing.Size(302, 154);
            listBusqueda.TabIndex = 12;
            // 
            // lblTablaProductos
            // 
            lblTablaProductos.AutoSize = true;
            lblTablaProductos.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblTablaProductos.Location = new System.Drawing.Point(27, 206);
            lblTablaProductos.Name = "lblTablaProductos";
            lblTablaProductos.Size = new System.Drawing.Size(146, 17);
            lblTablaProductos.TabIndex = 13;
            lblTablaProductos.Text = "Resultado de Búsqueda";
            // 
            // btnAgregarProd
            // 
            btnAgregarProd.FlatAppearance.BorderSize = 0;
            btnAgregarProd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAgregarProd.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            btnAgregarProd.Location = new System.Drawing.Point(91, 409);
            btnAgregarProd.Name = "btnAgregarProd";
            btnAgregarProd.Size = new System.Drawing.Size(104, 35);
            btnAgregarProd.TabIndex = 14;
            btnAgregarProd.Text = "Agregar";
            btnAgregarProd.UseVisualStyleBackColor = true;
            // 
            // btnQuitarProd
            // 
            btnQuitarProd.FlatAppearance.BorderSize = 0;
            btnQuitarProd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnQuitarProd.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            btnQuitarProd.Location = new System.Drawing.Point(221, 409);
            btnQuitarProd.Name = "btnQuitarProd";
            btnQuitarProd.Size = new System.Drawing.Size(104, 35);
            btnQuitarProd.TabIndex = 15;
            btnQuitarProd.Text = "Quitar";
            btnQuitarProd.UseVisualStyleBackColor = true;
            // 
            // btnVaciarCarrito
            // 
            btnVaciarCarrito.FlatAppearance.BorderSize = 0;
            btnVaciarCarrito.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnVaciarCarrito.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            btnVaciarCarrito.Location = new System.Drawing.Point(426, 409);
            btnVaciarCarrito.Name = "btnVaciarCarrito";
            btnVaciarCarrito.Size = new System.Drawing.Size(104, 35);
            btnVaciarCarrito.TabIndex = 16;
            btnVaciarCarrito.Text = "Vaciar Carro";
            btnVaciarCarrito.UseVisualStyleBackColor = true;
            // 
            // btnCompletarVenta
            // 
            btnCompletarVenta.FlatAppearance.BorderSize = 0;
            btnCompletarVenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCompletarVenta.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            btnCompletarVenta.Location = new System.Drawing.Point(684, 409);
            btnCompletarVenta.Name = "btnCompletarVenta";
            btnCompletarVenta.Size = new System.Drawing.Size(104, 35);
            btnCompletarVenta.TabIndex = 17;
            btnCompletarVenta.Text = "Terminar Venta";
            btnCompletarVenta.UseVisualStyleBackColor = true;
            // 
            // btnAsignarCliente
            // 
            btnAsignarCliente.FlatAppearance.BorderSize = 0;
            btnAsignarCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAsignarCliente.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            btnAsignarCliente.Location = new System.Drawing.Point(554, 409);
            btnAsignarCliente.Name = "btnAsignarCliente";
            btnAsignarCliente.Size = new System.Drawing.Size(104, 35);
            btnAsignarCliente.TabIndex = 18;
            btnAsignarCliente.Text = "Asignar Venta";
            btnAsignarCliente.UseVisualStyleBackColor = true;
            // 
            // VentasForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlLightLight;
            ClientSize = new System.Drawing.Size(857, 462);
            Controls.Add(btnAsignarCliente);
            Controls.Add(btnCompletarVenta);
            Controls.Add(btnVaciarCarrito);
            Controls.Add(btnQuitarProd);
            Controls.Add(btnAgregarProd);
            Controls.Add(lblTablaProductos);
            Controls.Add(listBusqueda);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(btnBuscarNombre);
            Controls.Add(txtBuscaNombre);
            Controls.Add(btnBuscarCodigo);
            Controls.Add(txtBuscarCodigo);
            Controls.Add(lblBuscar);
            Controls.Add(lblCarrito);
            Controls.Add(listCarrito);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "VentasForm";
            Text = "VentasForm";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listCarrito;
        private System.Windows.Forms.Label lblCarrito;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscarCodigo;
        private System.Windows.Forms.Button btnBuscarCodigo;
        private System.Windows.Forms.TextBox txtBuscaNombre;
        private System.Windows.Forms.Button btnBuscarNombre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBusqueda;
        private System.Windows.Forms.Label lblTablaProductos;
        private System.Windows.Forms.Button btnAgregarProd;
        private System.Windows.Forms.Button btnQuitarProd;
        private System.Windows.Forms.Button btnVaciarCarrito;
        private System.Windows.Forms.Button btnCompletarVenta;
        private System.Windows.Forms.Button btnAsignarCliente;
    }
}