namespace GUI
{
    partial class ProductosForm
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
            gridProductos = new System.Windows.Forms.DataGridView();
            lblAgregarProductos = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            lblTablaProductos = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            textBox2 = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            textBox3 = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            textBox4 = new System.Windows.Forms.TextBox();
            btnAgregarProducto = new System.Windows.Forms.Button();
            btnDetallesProducto = new System.Windows.Forms.Button();
            btnEliminarProductos = new System.Windows.Forms.Button();
            lblBuscarPorId = new System.Windows.Forms.Label();
            textBox5 = new System.Windows.Forms.TextBox();
            btnBuscarPorCodigo = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            textBox6 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)gridProductos).BeginInit();
            SuspendLayout();
            // 
            // gridProductos
            // 
            gridProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridProductos.Location = new System.Drawing.Point(370, 58);
            gridProductos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gridProductos.Name = "gridProductos";
            gridProductos.Size = new System.Drawing.Size(476, 317);
            gridProductos.TabIndex = 0;
            // 
            // lblAgregarProductos
            // 
            lblAgregarProductos.AutoSize = true;
            lblAgregarProductos.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblAgregarProductos.Location = new System.Drawing.Point(50, 36);
            lblAgregarProductos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblAgregarProductos.Name = "lblAgregarProductos";
            lblAgregarProductos.Size = new System.Drawing.Size(119, 17);
            lblAgregarProductos.TabIndex = 1;
            lblAgregarProductos.Text = "Agregar Productos";
            // 
            // textBox1
            // 
            textBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            textBox1.Location = new System.Drawing.Point(135, 114);
            textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(186, 25);
            textBox1.TabIndex = 2;
            // 
            // lblTablaProductos
            // 
            lblTablaProductos.AutoSize = true;
            lblTablaProductos.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblTablaProductos.Location = new System.Drawing.Point(370, 22);
            lblTablaProductos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTablaProductos.Name = "lblTablaProductos";
            lblTablaProductos.Size = new System.Drawing.Size(128, 17);
            lblTablaProductos.TabIndex = 3;
            lblTablaProductos.Text = "Productos Existentes";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(46, 121);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(48, 13);
            label3.TabIndex = 4;
            label3.Text = "Nombre";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label4.Location = new System.Drawing.Point(46, 157);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(67, 13);
            label4.TabIndex = 6;
            label4.Text = "Laboratorio";
            // 
            // textBox2
            // 
            textBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            textBox2.Location = new System.Drawing.Point(135, 150);
            textBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(186, 25);
            textBox2.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label5.Location = new System.Drawing.Point(46, 193);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(64, 13);
            label5.TabIndex = 8;
            label5.Text = "Precio Base";
            // 
            // textBox3
            // 
            textBox3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            textBox3.Location = new System.Drawing.Point(135, 186);
            textBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(186, 25);
            textBox3.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label6.Location = new System.Drawing.Point(46, 228);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(35, 13);
            label6.TabIndex = 10;
            label6.Text = "Stock";
            // 
            // textBox4
            // 
            textBox4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            textBox4.Location = new System.Drawing.Point(135, 222);
            textBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(186, 25);
            textBox4.TabIndex = 9;
            // 
            // btnAgregarProducto
            // 
            btnAgregarProducto.FlatAppearance.BorderSize = 0;
            btnAgregarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAgregarProducto.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnAgregarProducto.Location = new System.Drawing.Point(169, 257);
            btnAgregarProducto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnAgregarProducto.Name = "btnAgregarProducto";
            btnAgregarProducto.Size = new System.Drawing.Size(125, 30);
            btnAgregarProducto.TabIndex = 11;
            btnAgregarProducto.Text = "Agregar";
            btnAgregarProducto.UseVisualStyleBackColor = true;
            // 
            // btnDetallesProducto
            // 
            btnDetallesProducto.FlatAppearance.BorderSize = 0;
            btnDetallesProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDetallesProducto.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnDetallesProducto.Location = new System.Drawing.Point(449, 399);
            btnDetallesProducto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnDetallesProducto.Name = "btnDetallesProducto";
            btnDetallesProducto.Size = new System.Drawing.Size(125, 45);
            btnDetallesProducto.TabIndex = 12;
            btnDetallesProducto.Text = "Ver Detalles";
            btnDetallesProducto.UseVisualStyleBackColor = true;
            btnDetallesProducto.Click += btnDetallesProducto_Click;
            // 
            // btnEliminarProductos
            // 
            btnEliminarProductos.FlatAppearance.BorderSize = 0;
            btnEliminarProductos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEliminarProductos.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnEliminarProductos.Location = new System.Drawing.Point(656, 399);
            btnEliminarProductos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnEliminarProductos.Name = "btnEliminarProductos";
            btnEliminarProductos.Size = new System.Drawing.Size(125, 45);
            btnEliminarProductos.TabIndex = 13;
            btnEliminarProductos.Text = "Eliminar";
            btnEliminarProductos.UseVisualStyleBackColor = true;
            // 
            // lblBuscarPorId
            // 
            lblBuscarPorId.AutoSize = true;
            lblBuscarPorId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblBuscarPorId.Location = new System.Drawing.Point(37, 355);
            lblBuscarPorId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblBuscarPorId.Name = "lblBuscarPorId";
            lblBuscarPorId.Size = new System.Drawing.Size(117, 17);
            lblBuscarPorId.TabIndex = 14;
            lblBuscarPorId.Text = "Buscar Por Codigo";
            // 
            // textBox5
            // 
            textBox5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            textBox5.Location = new System.Drawing.Point(41, 399);
            textBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox5.Name = "textBox5";
            textBox5.Size = new System.Drawing.Size(186, 25);
            textBox5.TabIndex = 15;
            // 
            // btnBuscarPorCodigo
            // 
            btnBuscarPorCodigo.FlatAppearance.BorderSize = 0;
            btnBuscarPorCodigo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBuscarPorCodigo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnBuscarPorCodigo.Location = new System.Drawing.Point(261, 397);
            btnBuscarPorCodigo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnBuscarPorCodigo.Name = "btnBuscarPorCodigo";
            btnBuscarPorCodigo.Size = new System.Drawing.Size(88, 27);
            btnBuscarPorCodigo.TabIndex = 16;
            btnBuscarPorCodigo.Text = "Buscar";
            btnBuscarPorCodigo.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(46, 85);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(45, 13);
            label1.TabIndex = 17;
            label1.Text = "Codigo";
            // 
            // textBox6
            // 
            textBox6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            textBox6.Location = new System.Drawing.Point(135, 78);
            textBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox6.Name = "textBox6";
            textBox6.Size = new System.Drawing.Size(186, 25);
            textBox6.TabIndex = 18;
            // 
            // ProductosForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlLightLight;
            ClientSize = new System.Drawing.Size(860, 470);
            Controls.Add(textBox6);
            Controls.Add(label1);
            Controls.Add(btnBuscarPorCodigo);
            Controls.Add(textBox5);
            Controls.Add(lblBuscarPorId);
            Controls.Add(btnEliminarProductos);
            Controls.Add(btnDetallesProducto);
            Controls.Add(btnAgregarProducto);
            Controls.Add(label6);
            Controls.Add(textBox4);
            Controls.Add(label5);
            Controls.Add(textBox3);
            Controls.Add(label4);
            Controls.Add(textBox2);
            Controls.Add(label3);
            Controls.Add(lblTablaProductos);
            Controls.Add(textBox1);
            Controls.Add(lblAgregarProductos);
            Controls.Add(gridProductos);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ProductosForm";
            Text = "ProductosForm";
            ((System.ComponentModel.ISupportInitialize)gridProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridProductos;
        private System.Windows.Forms.Label lblAgregarProductos;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblTablaProductos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button btnAgregarProducto;
        private System.Windows.Forms.Button btnDetallesProducto;
        private System.Windows.Forms.Button btnEliminarProductos;
        private System.Windows.Forms.Label lblBuscarPorId;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button btnBuscarPorCodigo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox6;
    }
}