namespace Ofuscator
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.dataGridCsvInformation = new System.Windows.Forms.DataGridView();
            this.btnSelect = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.comboTableNames = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbFakeData = new System.Windows.Forms.ComboBox();
            this.comboField = new System.Windows.Forms.ComboBox();
            this.fileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSourceInformationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCsvInformation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSourceInformationBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(715, 488);
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
            this.columnSelector.Location = new System.Drawing.Point(476, 31);
            this.columnSelector.Name = "columnSelector";
            this.columnSelector.Size = new System.Drawing.Size(312, 156);
            this.columnSelector.TabIndex = 1;
            // 
            // chkHasHeaders
            // 
            this.chkHasHeaders.AutoSize = true;
            this.chkHasHeaders.Location = new System.Drawing.Point(476, 12);
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
            this.label8.Location = new System.Drawing.Point(592, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "SELECT COLUMN";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 197);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "SQL Connection String";
            // 
            // txtSqlConnectionString
            // 
            this.txtSqlConnectionString.Location = new System.Drawing.Point(12, 213);
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
            this.groupBox2.Location = new System.Drawing.Point(12, 268);
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
            this.btnConnect.Location = new System.Drawing.Point(12, 239);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // dataGridCsvInformation
            // 
            this.dataGridCsvInformation.AutoGenerateColumns = false;
            this.dataGridCsvInformation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCsvInformation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileNameDataGridViewTextBoxColumn,
            this.columnIndexDataGridViewTextBoxColumn,
            this.columnNameDataGridViewTextBoxColumn,
            this.btnSelect});
            this.dataGridCsvInformation.DataSource = this.dataSourceInformationBindingSource;
            this.dataGridCsvInformation.Location = new System.Drawing.Point(12, 12);
            this.dataGridCsvInformation.Name = "dataGridCsvInformation";
            this.dataGridCsvInformation.Size = new System.Drawing.Size(458, 175);
            this.dataGridCsvInformation.TabIndex = 19;
            this.dataGridCsvInformation.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridCell_Click);
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
            this.btnAddNew.Location = new System.Drawing.Point(13, 13);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(40, 20);
            this.btnAddNew.TabIndex = 9;
            this.btnAddNew.Text = "ADD";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.BtnAddNew_Click);
            // 
            // comboTableNames
            // 
            this.comboTableNames.FormattingEnabled = true;
            this.comboTableNames.Location = new System.Drawing.Point(93, 241);
            this.comboTableNames.Name = "comboTableNames";
            this.comboTableNames.Size = new System.Drawing.Size(448, 21);
            this.comboTableNames.TabIndex = 20;
            this.comboTableNames.Text = "Select table...";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(547, 281);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "=>";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // cmbFakeData
            // 
            this.cmbFakeData.FormattingEnabled = true;
            this.cmbFakeData.Location = new System.Drawing.Point(194, 283);
            this.cmbFakeData.Name = "cmbFakeData";
            this.cmbFakeData.Size = new System.Drawing.Size(347, 21);
            this.cmbFakeData.TabIndex = 22;
            this.cmbFakeData.Text = "Select origin of fake data...";
            // 
            // comboField
            // 
            this.comboField.FormattingEnabled = true;
            this.comboField.Location = new System.Drawing.Point(547, 241);
            this.comboField.Name = "comboField";
            this.comboField.Size = new System.Drawing.Size(232, 21);
            this.comboField.TabIndex = 22;
            this.comboField.Text = "Select field...";
            // 
            // fileNameDataGridViewTextBoxColumn
            // 
            this.fileNameDataGridViewTextBoxColumn.DataPropertyName = "FileName";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.fileNameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.fileNameDataGridViewTextBoxColumn.HeaderText = "FileName";
            this.fileNameDataGridViewTextBoxColumn.Name = "fileNameDataGridViewTextBoxColumn";
            this.fileNameDataGridViewTextBoxColumn.Width = 160;
            // 
            // columnIndexDataGridViewTextBoxColumn
            // 
            this.columnIndexDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnIndexDataGridViewTextBoxColumn.DataPropertyName = "ColumnIndex";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnIndexDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
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
            this.dataSourceInformationBindingSource.DataSource = typeof(Ofuscator.Entities.CsvInformation);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 322);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(775, 150);
            this.dataGridView1.TabIndex = 23;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 523);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboField);
            this.Controls.Add(this.cmbFakeData);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboTableNames);
            this.Controls.Add(this.btnAddNew);
            this.Controls.Add(this.dataGridCsvInformation);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.groupBox2);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCsvInformation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSourceInformationBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.DataGridView dataGridCsvInformation;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn btnSelect;
        private System.Windows.Forms.ComboBox comboTableNames;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbFakeData;
        private System.Windows.Forms.ComboBox comboField;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

