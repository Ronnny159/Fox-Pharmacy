namespace GUI
{
    partial class InventarioForm
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
            lblCodigoLote = new System.Windows.Forms.Label();
            lblCodigoProducto = new System.Windows.Forms.Label();
            lblCantidadLote = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            textBox2 = new System.Windows.Forms.TextBox();
            comboBox1 = new System.Windows.Forms.ComboBox();
            btnModificarLote = new System.Windows.Forms.Button();
            listExistencias = new System.Windows.Forms.ListView();
            listVencimientos = new System.Windows.Forms.ListView();
            lblVencimientos = new System.Windows.Forms.Label();
            lblExistencias = new System.Windows.Forms.Label();
            grdLotes = new System.Windows.Forms.DataGridView();
            btnBuscarLote = new System.Windows.Forms.Button();
            btnEliminarLote = new System.Windows.Forms.Button();
            lblRegistroLote = new System.Windows.Forms.Label();
            btnAgregarLote = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)grdLotes).BeginInit();
            SuspendLayout();
            // 
            // lblCodigoLote
            // 
            lblCodigoLote.AutoSize = true;
            lblCodigoLote.Location = new System.Drawing.Point(109, 54);
            lblCodigoLote.Name = "lblCodigoLote";
            lblCodigoLote.Size = new System.Drawing.Size(88, 15);
            lblCodigoLote.TabIndex = 0;
            lblCodigoLote.Text = "Código del lote";
            // 
            // lblCodigoProducto
            // 
            lblCodigoProducto.AutoSize = true;
            lblCodigoProducto.Location = new System.Drawing.Point(109, 89);
            lblCodigoProducto.Name = "lblCodigoProducto";
            lblCodigoProducto.Size = new System.Drawing.Size(117, 15);
            lblCodigoProducto.TabIndex = 1;
            lblCodigoProducto.Text = "Código del producto";
            // 
            // lblCantidadLote
            // 
            lblCantidadLote.AutoSize = true;
            lblCantidadLote.Location = new System.Drawing.Point(110, 123);
            lblCantidadLote.Name = "lblCantidadLote";
            lblCantidadLote.Size = new System.Drawing.Size(55, 15);
            lblCantidadLote.TabIndex = 2;
            lblCantidadLote.Text = "Cantidad";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(251, 51);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(194, 23);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(251, 120);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(194, 23);
            textBox2.TabIndex = 4;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(251, 86);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(194, 23);
            comboBox1.TabIndex = 5;
            // 
            // btnModificarLote
            // 
            btnModificarLote.FlatAppearance.BorderSize = 0;
            btnModificarLote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnModificarLote.Location = new System.Drawing.Point(58, 430);
            btnModificarLote.Name = "btnModificarLote";
            btnModificarLote.Size = new System.Drawing.Size(102, 23);
            btnModificarLote.TabIndex = 6;
            btnModificarLote.Text = "Modificar";
            btnModificarLote.UseVisualStyleBackColor = true;
            // 
            // listExistencias
            // 
            listExistencias.Location = new System.Drawing.Point(558, 49);
            listExistencias.Name = "listExistencias";
            listExistencias.Size = new System.Drawing.Size(271, 169);
            listExistencias.TabIndex = 7;
            listExistencias.UseCompatibleStateImageBehavior = false;
            // 
            // listVencimientos
            // 
            listVencimientos.Location = new System.Drawing.Point(558, 269);
            listVencimientos.Name = "listVencimientos";
            listVencimientos.Size = new System.Drawing.Size(271, 169);
            listVencimientos.TabIndex = 8;
            listVencimientos.UseCompatibleStateImageBehavior = false;
            // 
            // lblVencimientos
            // 
            lblVencimientos.AutoSize = true;
            lblVencimientos.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblVencimientos.Location = new System.Drawing.Point(558, 240);
            lblVencimientos.Name = "lblVencimientos";
            lblVencimientos.Size = new System.Drawing.Size(84, 17);
            lblVencimientos.TabIndex = 9;
            lblVencimientos.Text = "Vencimientos";
            // 
            // lblExistencias
            // 
            lblExistencias.AutoSize = true;
            lblExistencias.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblExistencias.Location = new System.Drawing.Point(558, 20);
            lblExistencias.Name = "lblExistencias";
            lblExistencias.Size = new System.Drawing.Size(70, 17);
            lblExistencias.TabIndex = 10;
            lblExistencias.Text = "Existencias";
            // 
            // grdLotes
            // 
            grdLotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdLotes.Location = new System.Drawing.Point(58, 182);
            grdLotes.Name = "grdLotes";
            grdLotes.Size = new System.Drawing.Size(442, 234);
            grdLotes.TabIndex = 11;
            // 
            // btnBuscarLote
            // 
            btnBuscarLote.FlatAppearance.BorderSize = 0;
            btnBuscarLote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBuscarLote.Location = new System.Drawing.Point(227, 430);
            btnBuscarLote.Name = "btnBuscarLote";
            btnBuscarLote.Size = new System.Drawing.Size(102, 23);
            btnBuscarLote.TabIndex = 12;
            btnBuscarLote.Text = "Buscar por ID";
            btnBuscarLote.UseVisualStyleBackColor = true;
            // 
            // btnEliminarLote
            // 
            btnEliminarLote.FlatAppearance.BorderSize = 0;
            btnEliminarLote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEliminarLote.Location = new System.Drawing.Point(398, 430);
            btnEliminarLote.Name = "btnEliminarLote";
            btnEliminarLote.Size = new System.Drawing.Size(102, 23);
            btnEliminarLote.TabIndex = 13;
            btnEliminarLote.Text = "Eliminar";
            btnEliminarLote.UseVisualStyleBackColor = true;
            // 
            // lblRegistroLote
            // 
            lblRegistroLote.AutoSize = true;
            lblRegistroLote.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lblRegistroLote.Location = new System.Drawing.Point(58, 20);
            lblRegistroLote.Name = "lblRegistroLote";
            lblRegistroLote.Size = new System.Drawing.Size(105, 17);
            lblRegistroLote.TabIndex = 14;
            lblRegistroLote.Text = "Registro de Lote";
            // 
            // btnAgregarLote
            // 
            btnAgregarLote.FlatAppearance.BorderSize = 0;
            btnAgregarLote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAgregarLote.Location = new System.Drawing.Point(291, 152);
            btnAgregarLote.Name = "btnAgregarLote";
            btnAgregarLote.Size = new System.Drawing.Size(112, 23);
            btnAgregarLote.TabIndex = 15;
            btnAgregarLote.Text = "Agregar Lote";
            btnAgregarLote.UseVisualStyleBackColor = true;
            // 
            // InventarioForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlLightLight;
            ClientSize = new System.Drawing.Size(841, 465);
            Controls.Add(btnAgregarLote);
            Controls.Add(lblRegistroLote);
            Controls.Add(btnEliminarLote);
            Controls.Add(btnBuscarLote);
            Controls.Add(grdLotes);
            Controls.Add(lblExistencias);
            Controls.Add(lblVencimientos);
            Controls.Add(listVencimientos);
            Controls.Add(listExistencias);
            Controls.Add(btnModificarLote);
            Controls.Add(comboBox1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(lblCantidadLote);
            Controls.Add(lblCodigoProducto);
            Controls.Add(lblCodigoLote);
            ForeColor = System.Drawing.SystemColors.ControlText;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "InventarioForm";
            Text = "InventarioForm";
            ((System.ComponentModel.ISupportInitialize)grdLotes).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCodigoLote;
        private System.Windows.Forms.Label lblCodigoProducto;
        private System.Windows.Forms.Label lblCantidadLote;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnModificarLote;
        private System.Windows.Forms.ListView listExistencias;
        private System.Windows.Forms.ListView listVencimientos;
        private System.Windows.Forms.Label lblVencimientos;
        private System.Windows.Forms.Label lblExistencias;
        private System.Windows.Forms.DataGridView grdLotes;
        private System.Windows.Forms.Button btnBuscarLote;
        private System.Windows.Forms.Button btnEliminarLote;
        private System.Windows.Forms.Label lblRegistroLote;
        private System.Windows.Forms.Button btnAgregarLote;
    }
}