﻿namespace Obfuscator
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
            this.btnClose = new System.Windows.Forms.Button();
            this.columnSelector = new System.Windows.Forms.Panel();
            this.chkHasHeaders = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSqlConnectionString = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.gridCsvInformation = new System.Windows.Forms.DataGridView();
            this.datasourceNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSelect = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnAddCsv = new System.Windows.Forms.Button();
            this.comboDbTableNames = new System.Windows.Forms.ComboBox();
            this.btnCreateObuscationOperation = new System.Windows.Forms.Button();
            this.comboDbField = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbObfuscationOps = new System.Windows.Forms.ListBox();
            this.btnClearOps = new System.Windows.Forms.Button();
            this.btnLoadOps = new System.Windows.Forms.Button();
            this.btnSaveOps = new System.Windows.Forms.Button();
            this.btnRunObfuscationOps = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnNIF = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDNI = new System.Windows.Forms.Button();
            this.btnNIE = new System.Windows.Forms.Button();
            this.btnAddFieldToScramble = new System.Windows.Forms.Button();
            this.btnRemoveFieldFromScramble = new System.Windows.Forms.Button();
            this.btnAddGroupFieldToScramble = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnScramble = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gridColumnDataSourceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnIndexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataSourceInformationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridCsvInformation)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSourceInformationBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(869, 562);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(73, 23);
            this.btnClose.TabIndex = 17;
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
            this.columnSelector.Location = new System.Drawing.Point(545, 32);
            this.columnSelector.Name = "columnSelector";
            this.columnSelector.Size = new System.Drawing.Size(397, 175);
            this.columnSelector.TabIndex = 3;
            // 
            // chkHasHeaders
            // 
            this.chkHasHeaders.AutoSize = true;
            this.chkHasHeaders.Location = new System.Drawing.Point(545, 15);
            this.chkHasHeaders.Name = "chkHasHeaders";
            this.chkHasHeaders.Size = new System.Drawing.Size(86, 17);
            this.chkHasHeaders.TabIndex = 2;
            this.chkHasHeaders.Text = "Has headers";
            this.chkHasHeaders.UseVisualStyleBackColor = true;
            this.chkHasHeaders.CheckedChanged += new System.EventHandler(this.ChkHasHeaders_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(696, 16);
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
            this.txtSqlConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlConnectionString.Location = new System.Drawing.Point(12, 233);
            this.txtSqlConnectionString.Name = "txtSqlConnectionString";
            this.txtSqlConnectionString.Size = new System.Drawing.Size(930, 20);
            this.txtSqlConnectionString.TabIndex = 4;
            this.txtSqlConnectionString.Text = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=testing;Integrated Security=Tr" +
    "ue;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationInte" +
    "nt=ReadWrite;MultiSubnetFailover=False";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 259);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // gridCsvInformation
            // 
            this.gridCsvInformation.AutoGenerateColumns = false;
            this.gridCsvInformation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCsvInformation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridColumnDataSourceType,
            this.datasourceNameDataGridViewTextBoxColumn,
            this.columnIndexDataGridViewTextBoxColumn,
            this.columnNameDataGridViewTextBoxColumn,
            this.btnSelect});
            this.gridCsvInformation.DataSource = this.dataSourceInformationBindingSource;
            this.gridCsvInformation.Location = new System.Drawing.Point(12, 32);
            this.gridCsvInformation.Name = "gridCsvInformation";
            this.gridCsvInformation.Size = new System.Drawing.Size(527, 175);
            this.gridCsvInformation.TabIndex = 1;
            this.gridCsvInformation.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridCell_Click);
            // 
            // datasourceNameDataGridViewTextBoxColumn
            // 
            this.datasourceNameDataGridViewTextBoxColumn.DataPropertyName = "DataSourceName";
            this.datasourceNameDataGridViewTextBoxColumn.HeaderText = "DataSourceName";
            this.datasourceNameDataGridViewTextBoxColumn.Name = "datasourceNameDataGridViewTextBoxColumn";
            this.datasourceNameDataGridViewTextBoxColumn.Width = 160;
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
            // btnAddCsv
            // 
            this.btnAddCsv.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCsv.Location = new System.Drawing.Point(105, 11);
            this.btnAddCsv.Name = "btnAddCsv";
            this.btnAddCsv.Size = new System.Drawing.Size(44, 20);
            this.btnAddCsv.TabIndex = 0;
            this.btnAddCsv.Text = "+CSV";
            this.toolTip1.SetToolTip(this.btnAddCsv, "CSV file");
            this.btnAddCsv.UseVisualStyleBackColor = true;
            this.btnAddCsv.Click += new System.EventHandler(this.BtnAddNew_Click);
            // 
            // comboDbTableNames
            // 
            this.comboDbTableNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboDbTableNames.FormattingEnabled = true;
            this.comboDbTableNames.Location = new System.Drawing.Point(93, 261);
            this.comboDbTableNames.Name = "comboDbTableNames";
            this.comboDbTableNames.Size = new System.Drawing.Size(576, 21);
            this.comboDbTableNames.TabIndex = 6;
            this.comboDbTableNames.Text = "Select table...";
            // 
            // btnCreateObuscationOperation
            // 
            this.btnCreateObuscationOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateObuscationOperation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateObuscationOperation.Location = new System.Drawing.Point(913, 260);
            this.btnCreateObuscationOperation.Name = "btnCreateObuscationOperation";
            this.btnCreateObuscationOperation.Size = new System.Drawing.Size(29, 23);
            this.btnCreateObuscationOperation.TabIndex = 8;
            this.btnCreateObuscationOperation.Text = "=>";
            this.btnCreateObuscationOperation.UseVisualStyleBackColor = true;
            this.btnCreateObuscationOperation.Click += new System.EventHandler(this.BtnCreateObuscationOperation_Click);
            // 
            // comboDbField
            // 
            this.comboDbField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboDbField.FormattingEnabled = true;
            this.comboDbField.Location = new System.Drawing.Point(675, 261);
            this.comboDbField.Name = "comboDbField";
            this.comboDbField.Size = new System.Drawing.Size(232, 21);
            this.comboDbField.TabIndex = 7;
            this.comboDbField.Text = "Select field...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "DATASOURCES:";
            // 
            // lbObfuscationOps
            // 
            this.lbObfuscationOps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbObfuscationOps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbObfuscationOps.FormattingEnabled = true;
            this.lbObfuscationOps.ItemHeight = 15;
            this.lbObfuscationOps.Location = new System.Drawing.Point(12, 297);
            this.lbObfuscationOps.Name = "lbObfuscationOps";
            this.lbObfuscationOps.Size = new System.Drawing.Size(930, 259);
            this.lbObfuscationOps.TabIndex = 12;
            this.lbObfuscationOps.Click += new System.EventHandler(this.LbObfuscationOps_Click);
            this.lbObfuscationOps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LbObfuscationOps_KeyDown);
            // 
            // btnClearOps
            // 
            this.btnClearOps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearOps.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClearOps.Location = new System.Drawing.Point(88, 562);
            this.btnClearOps.Name = "btnClearOps";
            this.btnClearOps.Size = new System.Drawing.Size(47, 23);
            this.btnClearOps.TabIndex = 14;
            this.btnClearOps.Text = "Clear";
            this.btnClearOps.UseVisualStyleBackColor = true;
            this.btnClearOps.Click += new System.EventHandler(this.BtnClearOps_Click);
            // 
            // btnLoadOps
            // 
            this.btnLoadOps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadOps.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnLoadOps.Location = new System.Drawing.Point(141, 562);
            this.btnLoadOps.Name = "btnLoadOps";
            this.btnLoadOps.Size = new System.Drawing.Size(47, 23);
            this.btnLoadOps.TabIndex = 15;
            this.btnLoadOps.Text = "Load";
            this.btnLoadOps.UseVisualStyleBackColor = true;
            this.btnLoadOps.Click += new System.EventHandler(this.BtnLoadOps_Click);
            // 
            // btnSaveOps
            // 
            this.btnSaveOps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveOps.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSaveOps.Location = new System.Drawing.Point(194, 562);
            this.btnSaveOps.Name = "btnSaveOps";
            this.btnSaveOps.Size = new System.Drawing.Size(47, 23);
            this.btnSaveOps.TabIndex = 16;
            this.btnSaveOps.Text = "Save";
            this.btnSaveOps.UseVisualStyleBackColor = true;
            this.btnSaveOps.Click += new System.EventHandler(this.BtnSaveOps_Click);
            // 
            // btnRunObfuscationOps
            // 
            this.btnRunObfuscationOps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunObfuscationOps.BackColor = System.Drawing.Color.SandyBrown;
            this.btnRunObfuscationOps.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRunObfuscationOps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunObfuscationOps.Location = new System.Drawing.Point(12, 562);
            this.btnRunObfuscationOps.Name = "btnRunObfuscationOps";
            this.btnRunObfuscationOps.Size = new System.Drawing.Size(47, 23);
            this.btnRunObfuscationOps.TabIndex = 13;
            this.btnRunObfuscationOps.Text = "RUN";
            this.btnRunObfuscationOps.UseVisualStyleBackColor = false;
            this.btnRunObfuscationOps.Click += new System.EventHandler(this.BtnRunObfuscationOps_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 593);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(954, 22);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Step = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusLabel1.Text = "-";
            // 
            // btnNIF
            // 
            this.btnNIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNIF.Location = new System.Drawing.Point(149, 11);
            this.btnNIF.Name = "btnNIF";
            this.btnNIF.Size = new System.Drawing.Size(44, 20);
            this.btnNIF.TabIndex = 20;
            this.btnNIF.Text = "+NIF";
            this.toolTip1.SetToolTip(this.btnNIF, "NIF generator - mix of DNIs and NIEs");
            this.btnNIF.UseVisualStyleBackColor = true;
            this.btnNIF.Click += new System.EventHandler(this.BtnAddNew_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "DataSourceType";
            this.dataGridViewTextBoxColumn1.HeaderText = "Type";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // btnDNI
            // 
            this.btnDNI.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDNI.Location = new System.Drawing.Point(193, 11);
            this.btnDNI.Name = "btnDNI";
            this.btnDNI.Size = new System.Drawing.Size(44, 20);
            this.btnDNI.TabIndex = 21;
            this.btnDNI.Text = "+DNI";
            this.toolTip1.SetToolTip(this.btnDNI, "DNI generator");
            this.btnDNI.UseVisualStyleBackColor = true;
            this.btnDNI.Click += new System.EventHandler(this.BtnAddNew_Click);
            // 
            // btnNIE
            // 
            this.btnNIE.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNIE.Location = new System.Drawing.Point(237, 11);
            this.btnNIE.Name = "btnNIE";
            this.btnNIE.Size = new System.Drawing.Size(44, 20);
            this.btnNIE.TabIndex = 22;
            this.btnNIE.Text = "+NIE";
            this.toolTip1.SetToolTip(this.btnNIE, "NIE generator");
            this.btnNIE.UseVisualStyleBackColor = true;
            this.btnNIE.Click += new System.EventHandler(this.BtnAddNew_Click);
            // 
            // btnAddFieldToScramble
            // 
            this.btnAddFieldToScramble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFieldToScramble.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddFieldToScramble.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddFieldToScramble.Location = new System.Drawing.Point(837, 282);
            this.btnAddFieldToScramble.Name = "btnAddFieldToScramble";
            this.btnAddFieldToScramble.Size = new System.Drawing.Size(24, 21);
            this.btnAddFieldToScramble.TabIndex = 9;
            this.btnAddFieldToScramble.Text = "+";
            this.toolTip1.SetToolTip(this.btnAddFieldToScramble, "Scramble values on this column");
            this.btnAddFieldToScramble.UseVisualStyleBackColor = true;
            this.btnAddFieldToScramble.Click += new System.EventHandler(this.BtnEditScrambleOperation_Click);
            // 
            // btnRemoveFieldFromScramble
            // 
            this.btnRemoveFieldFromScramble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveFieldFromScramble.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRemoveFieldFromScramble.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveFieldFromScramble.Location = new System.Drawing.Point(861, 282);
            this.btnRemoveFieldFromScramble.Name = "btnRemoveFieldFromScramble";
            this.btnRemoveFieldFromScramble.Size = new System.Drawing.Size(24, 21);
            this.btnRemoveFieldFromScramble.TabIndex = 10;
            this.btnRemoveFieldFromScramble.Text = "-";
            this.toolTip1.SetToolTip(this.btnRemoveFieldFromScramble, "Don0t scramble values of this column");
            this.btnRemoveFieldFromScramble.UseVisualStyleBackColor = true;
            this.btnRemoveFieldFromScramble.Click += new System.EventHandler(this.BtnEditScrambleOperation_Click);
            // 
            // btnAddGroupFieldToScramble
            // 
            this.btnAddGroupFieldToScramble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddGroupFieldToScramble.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddGroupFieldToScramble.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddGroupFieldToScramble.Location = new System.Drawing.Point(885, 282);
            this.btnAddGroupFieldToScramble.Name = "btnAddGroupFieldToScramble";
            this.btnAddGroupFieldToScramble.Size = new System.Drawing.Size(24, 21);
            this.btnAddGroupFieldToScramble.TabIndex = 11;
            this.btnAddGroupFieldToScramble.Text = "( )";
            this.toolTip1.SetToolTip(this.btnAddGroupFieldToScramble, "GROUP BY this column to scramble group values");
            this.btnAddGroupFieldToScramble.UseVisualStyleBackColor = true;
            this.btnAddGroupFieldToScramble.Click += new System.EventHandler(this.BtnEditScrambleOperation_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(719, 285);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 9);
            this.label2.TabIndex = 25;
            this.label2.Text = "SCRAMBLING OPTIONS";
            // 
            // btnScramble
            // 
            this.btnScramble.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScramble.Location = new System.Drawing.Point(281, 11);
            this.btnScramble.Name = "btnScramble";
            this.btnScramble.Size = new System.Drawing.Size(44, 20);
            this.btnScramble.TabIndex = 26;
            this.btnScramble.Text = "+SCR";
            this.toolTip1.SetToolTip(this.btnScramble, "Sramble values ofuscation");
            this.btnScramble.UseVisualStyleBackColor = true;
            this.btnScramble.Click += new System.EventHandler(this.BtnAddNew_Click);
            // 
            // gridColumnDataSourceType
            // 
            this.gridColumnDataSourceType.DataPropertyName = "DataSourceType";
            this.gridColumnDataSourceType.HeaderText = "Type";
            this.gridColumnDataSourceType.Name = "gridColumnDataSourceType";
            this.gridColumnDataSourceType.ReadOnly = true;
            // 
            // columnIndexDataGridViewTextBoxColumn
            // 
            this.columnIndexDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnIndexDataGridViewTextBoxColumn.DataPropertyName = "ColumnIndex";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnIndexDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.columnIndexDataGridViewTextBoxColumn.HeaderText = "Column";
            this.columnIndexDataGridViewTextBoxColumn.Name = "columnIndexDataGridViewTextBoxColumn";
            this.columnIndexDataGridViewTextBoxColumn.ReadOnly = true;
            this.columnIndexDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.columnIndexDataGridViewTextBoxColumn.Width = 67;
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
            this.dataSourceInformationBindingSource.DataSource = typeof(Obfuscator.Entities.DataSourceInformation);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 615);
            this.Controls.Add(this.btnScramble);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAddGroupFieldToScramble);
            this.Controls.Add(this.btnRemoveFieldFromScramble);
            this.Controls.Add(this.btnAddFieldToScramble);
            this.Controls.Add(this.btnNIE);
            this.Controls.Add(this.btnDNI);
            this.Controls.Add(this.btnNIF);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnRunObfuscationOps);
            this.Controls.Add(this.btnSaveOps);
            this.Controls.Add(this.btnLoadOps);
            this.Controls.Add(this.btnClearOps);
            this.Controls.Add(this.comboDbField);
            this.Controls.Add(this.btnCreateObuscationOperation);
            this.Controls.Add(this.comboDbTableNames);
            this.Controls.Add(this.btnAddCsv);
            this.Controls.Add(this.gridCsvInformation);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkHasHeaders);
            this.Controls.Add(this.columnSelector);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtSqlConnectionString);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lbObfuscationOps);
            this.MinimumSize = new System.Drawing.Size(970, 654);
            this.Name = "MainForm";
            this.Text = "OFUSCATOR";
            ((System.ComponentModel.ISupportInitialize)(this.gridCsvInformation)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.BindingSource dataSourceInformationBindingSource;
        private System.Windows.Forms.DataGridView gridCsvInformation;
        private System.Windows.Forms.Button btnAddCsv;
        private System.Windows.Forms.ComboBox comboDbTableNames;
        private System.Windows.Forms.Button btnCreateObuscationOperation;
        private System.Windows.Forms.ComboBox comboDbField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbObfuscationOps;
        private System.Windows.Forms.Button btnClearOps;
        private System.Windows.Forms.Button btnLoadOps;
        private System.Windows.Forms.Button btnSaveOps;
        private System.Windows.Forms.Button btnRunObfuscationOps;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button btnNIF;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColumnDataSourceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn datasourceNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn btnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button btnDNI;
        private System.Windows.Forms.Button btnNIE;
        private System.Windows.Forms.Button btnAddFieldToScramble;
        private System.Windows.Forms.Button btnRemoveFieldFromScramble;
        private System.Windows.Forms.Button btnAddGroupFieldToScramble;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnScramble;
    }
}

