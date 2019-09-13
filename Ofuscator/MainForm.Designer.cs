namespace Obfuscator
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new System.Windows.Forms.Button();
            this.columnSelector = new System.Windows.Forms.Panel();
            this.chkHasHeaders = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSqlConnectionString = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioReplace = new System.Windows.Forms.RadioButton();
            this.radioAdd = new System.Windows.Forms.RadioButton();
            this.btnConnect = new System.Windows.Forms.Button();
            this.gridCsvInformation = new System.Windows.Forms.DataGridView();
            this.btnSelect = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.comboDbTableNames = new System.Windows.Forms.ComboBox();
            this.btnCreateObuscationOperation = new System.Windows.Forms.Button();
            this.comboDbField = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbObfuscationOps = new System.Windows.Forms.ListBox();
            this.btnClearOps = new System.Windows.Forms.Button();
            this.btnOpenOps = new System.Windows.Forms.Button();
            this.btnSaveOps = new System.Windows.Forms.Button();
            this.btnRunObfuscationOps = new System.Windows.Forms.Button();
            this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSourceInformationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCsvInformation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSourceInformationBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(715, 583);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(73, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // columnSelector
            // 
            this.columnSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.columnSelector.AutoScroll = true;
            this.columnSelector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.columnSelector.Location = new System.Drawing.Point(476, 32);
            this.columnSelector.Name = "columnSelector";
            this.columnSelector.Size = new System.Drawing.Size(312, 175);
            this.columnSelector.TabIndex = 1;
            // 
            // chkHasHeaders
            // 
            this.chkHasHeaders.AutoSize = true;
            this.chkHasHeaders.Location = new System.Drawing.Point(475, 15);
            this.chkHasHeaders.Name = "chkHasHeaders";
            this.chkHasHeaders.Size = new System.Drawing.Size(86, 17);
            this.chkHasHeaders.TabIndex = 17;
            this.chkHasHeaders.Text = "Has headers";
            this.chkHasHeaders.UseVisualStyleBackColor = true;
            this.chkHasHeaders.CheckedChanged += new System.EventHandler(this.ChkHasHeaders_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(591, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "SELECT COLUMN";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 217);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "SQL Connection String";
            // 
            // txtSqlConnectionString
            // 
            this.txtSqlConnectionString.Location = new System.Drawing.Point(12, 233);
            this.txtSqlConnectionString.Name = "txtSqlConnectionString";
            this.txtSqlConnectionString.Size = new System.Drawing.Size(767, 20);
            this.txtSqlConnectionString.TabIndex = 2;
            this.txtSqlConnectionString.Text = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=testing;Integrated Security=Tr" +
    "ue;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationInte" +
    "nt=ReadWrite;MultiSubnetFailover=False";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioReplace);
            this.groupBox2.Controls.Add(this.radioAdd);
            this.groupBox2.Location = new System.Drawing.Point(12, 288);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(176, 47);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Database update mode";
            // 
            // radioReplace
            // 
            this.radioReplace.AutoSize = true;
            this.radioReplace.Checked = true;
            this.radioReplace.Location = new System.Drawing.Point(6, 19);
            this.radioReplace.Name = "radioReplace";
            this.radioReplace.Size = new System.Drawing.Size(89, 17);
            this.radioReplace.TabIndex = 0;
            this.radioReplace.TabStop = true;
            this.radioReplace.Text = "Replace data";
            this.radioReplace.UseVisualStyleBackColor = true;
            // 
            // radioAdd
            // 
            this.radioAdd.AutoSize = true;
            this.radioAdd.Location = new System.Drawing.Point(101, 19);
            this.radioAdd.Name = "radioAdd";
            this.radioAdd.Size = new System.Drawing.Size(68, 17);
            this.radioAdd.TabIndex = 1;
            this.radioAdd.Text = "Add data";
            this.radioAdd.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 259);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // gridCsvInformation
            // 
            this.gridCsvInformation.AutoGenerateColumns = false;
            this.gridCsvInformation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCsvInformation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileNameDataGridViewTextBoxColumn,
            this.columnIndexDataGridViewTextBoxColumn,
            this.columnNameDataGridViewTextBoxColumn,
            this.btnSelect});
            this.gridCsvInformation.DataSource = this.dataSourceInformationBindingSource;
            this.gridCsvInformation.Location = new System.Drawing.Point(12, 32);
            this.gridCsvInformation.Name = "gridCsvInformation";
            this.gridCsvInformation.Size = new System.Drawing.Size(458, 175);
            this.gridCsvInformation.TabIndex = 19;
            this.gridCsvInformation.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridCell_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.btnSelect.HeaderText = "SELECT";
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Text = "...";
            this.btnSelect.ToolTipText = "Select a different file and/or column";
            this.btnSelect.UseColumnTextForButtonValue = true;
            this.btnSelect.Width = 54;
            // 
            // btnAddNew
            // 
            this.btnAddNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNew.Location = new System.Drawing.Point(13, 33);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(40, 20);
            this.btnAddNew.TabIndex = 9;
            this.btnAddNew.Text = "ADD";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.BtnAddNew_Click);
            // 
            // comboDbTableNames
            // 
            this.comboDbTableNames.FormattingEnabled = true;
            this.comboDbTableNames.Location = new System.Drawing.Point(93, 261);
            this.comboDbTableNames.Name = "comboDbTableNames";
            this.comboDbTableNames.Size = new System.Drawing.Size(448, 21);
            this.comboDbTableNames.TabIndex = 20;
            this.comboDbTableNames.Text = "Select table...";
            // 
            // btnCreateObuscationOperation
            // 
            this.btnCreateObuscationOperation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateObuscationOperation.Location = new System.Drawing.Point(194, 294);
            this.btnCreateObuscationOperation.Name = "btnCreateObuscationOperation";
            this.btnCreateObuscationOperation.Size = new System.Drawing.Size(29, 41);
            this.btnCreateObuscationOperation.TabIndex = 21;
            this.btnCreateObuscationOperation.Text = "=>";
            this.btnCreateObuscationOperation.UseVisualStyleBackColor = true;
            this.btnCreateObuscationOperation.Click += new System.EventHandler(this.BtnCreateObuscationOperation_Click);
            // 
            // comboDbField
            // 
            this.comboDbField.FormattingEnabled = true;
            this.comboDbField.Location = new System.Drawing.Point(547, 261);
            this.comboDbField.Name = "comboDbField";
            this.comboDbField.Size = new System.Drawing.Size(232, 21);
            this.comboDbField.TabIndex = 22;
            this.comboDbField.Text = "Select field...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "CSV FILES - origin of obfuscation data";
            // 
            // lbObfuscationOps
            // 
            this.lbObfuscationOps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbObfuscationOps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbObfuscationOps.FormattingEnabled = true;
            this.lbObfuscationOps.ItemHeight = 15;
            this.lbObfuscationOps.Location = new System.Drawing.Point(12, 342);
            this.lbObfuscationOps.Name = "lbObfuscationOps";
            this.lbObfuscationOps.Size = new System.Drawing.Size(776, 229);
            this.lbObfuscationOps.TabIndex = 23;
            this.lbObfuscationOps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LbObfuscationOps_KeyDown);
            // 
            // btnClearOps
            // 
            this.btnClearOps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearOps.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClearOps.Location = new System.Drawing.Point(88, 583);
            this.btnClearOps.Name = "btnClearOps";
            this.btnClearOps.Size = new System.Drawing.Size(47, 23);
            this.btnClearOps.TabIndex = 24;
            this.btnClearOps.Text = "Clear";
            this.btnClearOps.UseVisualStyleBackColor = true;
            this.btnClearOps.Click += new System.EventHandler(this.BtnClearOps_Click);
            // 
            // btnOpenOps
            // 
            this.btnOpenOps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenOps.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOpenOps.Location = new System.Drawing.Point(141, 583);
            this.btnOpenOps.Name = "btnOpenOps";
            this.btnOpenOps.Size = new System.Drawing.Size(47, 23);
            this.btnOpenOps.TabIndex = 25;
            this.btnOpenOps.Text = "Load";
            this.btnOpenOps.UseVisualStyleBackColor = true;
            // 
            // btnSaveOps
            // 
            this.btnSaveOps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveOps.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSaveOps.Location = new System.Drawing.Point(194, 583);
            this.btnSaveOps.Name = "btnSaveOps";
            this.btnSaveOps.Size = new System.Drawing.Size(47, 23);
            this.btnSaveOps.TabIndex = 26;
            this.btnSaveOps.Text = "Save";
            this.btnSaveOps.UseVisualStyleBackColor = true;
            // 
            // btnRunObfuscationOps
            // 
            this.btnRunObfuscationOps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunObfuscationOps.BackColor = System.Drawing.Color.SandyBrown;
            this.btnRunObfuscationOps.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRunObfuscationOps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunObfuscationOps.Location = new System.Drawing.Point(12, 583);
            this.btnRunObfuscationOps.Name = "btnRunObfuscationOps";
            this.btnRunObfuscationOps.Size = new System.Drawing.Size(47, 23);
            this.btnRunObfuscationOps.TabIndex = 27;
            this.btnRunObfuscationOps.Text = "RUN";
            this.btnRunObfuscationOps.UseVisualStyleBackColor = false;
            // 
            // fileNameDataGridViewTextBoxColumn
            // 
            this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.fileNameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.fileNameDataGridViewTextBoxColumn.HeaderText = "FileName";
            this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
            this.fileNameDataGridViewTextBoxColumn.Width = 160;
            // 
            // columnIndexDataGridViewTextBoxColumn
            // 
            this.columnIndexDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnIndexDataGridViewTextBoxColumn.DataPropertyName = "ColumnIndex";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnIndexDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.columnIndexDataGridViewTextBoxColumn.HeaderText = "ColumnIndex";
            this.columnIndexDataGridViewTextBoxColumn.Name = "columnIndexDataGridViewTextBoxColumn";
            this.columnIndexDataGridViewTextBoxColumn.ReadOnly = true;
            this.columnIndexDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.columnIndexDataGridViewTextBoxColumn.Width = 93;
            // 
            // columnNameDataGridViewTextBoxColumn
            // 
            this.columnNameDataGridViewTextBoxColumn.DataPropertyName = "ColumnName";
            this.columnNameDataGridViewTextBoxColumn.HeaderText = "ColumnName";
            this.columnNameDataGridViewTextBoxColumn.Name = "columnNameDataGridViewTextBoxColumn";
            this.columnNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataSourceInformationBindingSource
            // 
            this.dataSourceInformationBindingSource.DataSource = typeof(Obfuscator.Entities.CsvInformation);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 618);
            this.Controls.Add(this.btnRunObfuscationOps);
            this.Controls.Add(this.btnSaveOps);
            this.Controls.Add(this.btnOpenOps);
            this.Controls.Add(this.btnClearOps);
            this.Controls.Add(this.lbObfuscationOps);
            this.Controls.Add(this.comboDbField);
            this.Controls.Add(this.btnCreateObuscationOperation);
            this.Controls.Add(this.comboDbTableNames);
            this.Controls.Add(this.btnAddNew);
            this.Controls.Add(this.gridCsvInformation);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkHasHeaders);
            this.Controls.Add(this.columnSelector);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtSqlConnectionString);
            this.Controls.Add(this.label9);
            this.Name = "MainForm";
            this.Text = "OFUSCATOR";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCsvInformation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSourceInformationBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel columnSelector;
        private System.Windows.Forms.CheckBox chkHasHeaders;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSqlConnectionString;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioReplace;
        private System.Windows.Forms.RadioButton radioAdd;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.BindingSource dataSourceInformationBindingSource;
        private System.Windows.Forms.DataGridView gridCsvInformation;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn btnSelect;
        private System.Windows.Forms.ComboBox comboDbTableNames;
        private System.Windows.Forms.Button btnCreateObuscationOperation;
        private System.Windows.Forms.ComboBox comboDbField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbObfuscationOps;
        private System.Windows.Forms.Button btnClearOps;
        private System.Windows.Forms.Button btnOpenOps;
        private System.Windows.Forms.Button btnSaveOps;
        private System.Windows.Forms.Button btnRunObfuscationOps;
    }
}

