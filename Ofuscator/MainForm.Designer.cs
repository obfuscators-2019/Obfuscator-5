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
            this.label1 = new System.Windows.Forms.Label();
            this.txtNamesFile = new System.Windows.Forms.TextBox();
            this.btnSelectNamesFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPhoneNumberColumn = new System.Windows.Forms.TextBox();
            this.txtAddressColumn = new System.Windows.Forms.TextBox();
            this.txtNifsColumn = new System.Windows.Forms.TextBox();
            this.txtLastNameColumn = new System.Windows.Forms.TextBox();
            this.txtPhoneNumbersFile = new System.Windows.Forms.TextBox();
            this.btnSelectPhoneNumbersFile = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNIFsFile = new System.Windows.Forms.TextBox();
            this.btnSelectNifsFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAddressFile = new System.Windows.Forms.TextBox();
            this.btnSelectAddressFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLastNamesFile = new System.Windows.Forms.TextBox();
            this.btnSelectLastNamesFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNameColumn = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.columnSelector = new System.Windows.Forms.Panel();
            this.chkHasHeaders = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSqlConnectionString = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioReplace = new System.Windows.Forms.RadioButton();
            this.radioAdd = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Names";
            // 
            // txtNamesFile
            // 
            this.txtNamesFile.Location = new System.Drawing.Point(93, 41);
            this.txtNamesFile.Name = "txtNamesFile";
            this.txtNamesFile.Size = new System.Drawing.Size(237, 20);
            this.txtNamesFile.TabIndex = 0;
            // 
            // btnSelectNamesFile
            // 
            this.btnSelectNamesFile.Location = new System.Drawing.Point(417, 39);
            this.btnSelectNamesFile.Name = "btnSelectNamesFile";
            this.btnSelectNamesFile.Size = new System.Drawing.Size(28, 23);
            this.btnSelectNamesFile.TabIndex = 1;
            this.btnSelectNamesFile.Text = "...";
            this.btnSelectNamesFile.UseVisualStyleBackColor = true;
            this.btnSelectNamesFile.Click += new System.EventHandler(this.btnSelectFileAndColumn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPhoneNumberColumn);
            this.groupBox1.Controls.Add(this.txtAddressColumn);
            this.groupBox1.Controls.Add(this.txtNifsColumn);
            this.groupBox1.Controls.Add(this.txtLastNameColumn);
            this.groupBox1.Controls.Add(this.txtPhoneNumbersFile);
            this.groupBox1.Controls.Add(this.btnSelectPhoneNumbersFile);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtNIFsFile);
            this.groupBox1.Controls.Add(this.btnSelectNifsFile);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtAddressFile);
            this.groupBox1.Controls.Add(this.btnSelectAddressFile);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtLastNamesFile);
            this.groupBox1.Controls.Add(this.btnSelectLastNamesFile);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtNameColumn);
            this.groupBox1.Controls.Add(this.txtNamesFile);
            this.groupBox1.Controls.Add(this.btnSelectNamesFile);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 182);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FILES";
            // 
            // txtPhoneNumberColumn
            // 
            this.txtPhoneNumberColumn.Location = new System.Drawing.Point(336, 145);
            this.txtPhoneNumberColumn.Name = "txtPhoneNumberColumn";
            this.txtPhoneNumberColumn.ReadOnly = true;
            this.txtPhoneNumberColumn.Size = new System.Drawing.Size(75, 20);
            this.txtPhoneNumberColumn.TabIndex = 13;
            // 
            // txtAddressColumn
            // 
            this.txtAddressColumn.Location = new System.Drawing.Point(336, 93);
            this.txtAddressColumn.Name = "txtAddressColumn";
            this.txtAddressColumn.ReadOnly = true;
            this.txtAddressColumn.Size = new System.Drawing.Size(75, 20);
            this.txtAddressColumn.TabIndex = 7;
            // 
            // txtNifsColumn
            // 
            this.txtNifsColumn.Location = new System.Drawing.Point(336, 119);
            this.txtNifsColumn.Name = "txtNifsColumn";
            this.txtNifsColumn.ReadOnly = true;
            this.txtNifsColumn.Size = new System.Drawing.Size(75, 20);
            this.txtNifsColumn.TabIndex = 10;
            // 
            // txtLastNameColumn
            // 
            this.txtLastNameColumn.Location = new System.Drawing.Point(336, 67);
            this.txtLastNameColumn.Name = "txtLastNameColumn";
            this.txtLastNameColumn.ReadOnly = true;
            this.txtLastNameColumn.Size = new System.Drawing.Size(75, 20);
            this.txtLastNameColumn.TabIndex = 4;
            // 
            // txtPhoneNumbersFile
            // 
            this.txtPhoneNumbersFile.Location = new System.Drawing.Point(93, 145);
            this.txtPhoneNumbersFile.Name = "txtPhoneNumbersFile";
            this.txtPhoneNumbersFile.Size = new System.Drawing.Size(237, 20);
            this.txtPhoneNumbersFile.TabIndex = 8;
            // 
            // btnSelectPhoneNumbersFile
            // 
            this.btnSelectPhoneNumbersFile.Location = new System.Drawing.Point(417, 143);
            this.btnSelectPhoneNumbersFile.Name = "btnSelectPhoneNumbersFile";
            this.btnSelectPhoneNumbersFile.Size = new System.Drawing.Size(28, 23);
            this.btnSelectPhoneNumbersFile.TabIndex = 9;
            this.btnSelectPhoneNumbersFile.Text = "...";
            this.btnSelectPhoneNumbersFile.UseVisualStyleBackColor = true;
            this.btnSelectPhoneNumbersFile.Click += new System.EventHandler(this.btnSelectFileAndColumn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Phone numbers";
            // 
            // txtNIFsFile
            // 
            this.txtNIFsFile.Location = new System.Drawing.Point(93, 119);
            this.txtNIFsFile.Name = "txtNIFsFile";
            this.txtNIFsFile.Size = new System.Drawing.Size(237, 20);
            this.txtNIFsFile.TabIndex = 6;
            // 
            // btnSelectNifsFile
            // 
            this.btnSelectNifsFile.Location = new System.Drawing.Point(417, 117);
            this.btnSelectNifsFile.Name = "btnSelectNifsFile";
            this.btnSelectNifsFile.Size = new System.Drawing.Size(28, 23);
            this.btnSelectNifsFile.TabIndex = 7;
            this.btnSelectNifsFile.Text = "...";
            this.btnSelectNifsFile.UseVisualStyleBackColor = true;
            this.btnSelectNifsFile.Click += new System.EventHandler(this.btnSelectFileAndColumn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "NIFs";
            // 
            // txtAddressFile
            // 
            this.txtAddressFile.Location = new System.Drawing.Point(93, 93);
            this.txtAddressFile.Name = "txtAddressFile";
            this.txtAddressFile.Size = new System.Drawing.Size(237, 20);
            this.txtAddressFile.TabIndex = 4;
            // 
            // btnSelectAddressFile
            // 
            this.btnSelectAddressFile.Location = new System.Drawing.Point(417, 91);
            this.btnSelectAddressFile.Name = "btnSelectAddressFile";
            this.btnSelectAddressFile.Size = new System.Drawing.Size(28, 23);
            this.btnSelectAddressFile.TabIndex = 5;
            this.btnSelectAddressFile.Text = "...";
            this.btnSelectAddressFile.UseVisualStyleBackColor = true;
            this.btnSelectAddressFile.Click += new System.EventHandler(this.btnSelectFileAndColumn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Address";
            // 
            // txtLastNamesFile
            // 
            this.txtLastNamesFile.Location = new System.Drawing.Point(93, 67);
            this.txtLastNamesFile.Name = "txtLastNamesFile";
            this.txtLastNamesFile.Size = new System.Drawing.Size(237, 20);
            this.txtLastNamesFile.TabIndex = 2;
            // 
            // btnSelectLastNamesFile
            // 
            this.btnSelectLastNamesFile.Location = new System.Drawing.Point(417, 65);
            this.btnSelectLastNamesFile.Name = "btnSelectLastNamesFile";
            this.btnSelectLastNamesFile.Size = new System.Drawing.Size(28, 23);
            this.btnSelectLastNamesFile.TabIndex = 3;
            this.btnSelectLastNamesFile.Text = "...";
            this.btnSelectLastNamesFile.UseVisualStyleBackColor = true;
            this.btnSelectLastNamesFile.Click += new System.EventHandler(this.btnSelectFileAndColumn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last Names";
            // 
            // txtNameColumn
            // 
            this.txtNameColumn.Location = new System.Drawing.Point(336, 42);
            this.txtNameColumn.Name = "txtNameColumn";
            this.txtNameColumn.ReadOnly = true;
            this.txtNameColumn.Size = new System.Drawing.Size(75, 20);
            this.txtNameColumn.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(333, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Column";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(90, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "File Name";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(715, 431);
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
            this.columnSelector.Location = new System.Drawing.Point(476, 38);
            this.columnSelector.Name = "columnSelector";
            this.columnSelector.Size = new System.Drawing.Size(312, 156);
            this.columnSelector.TabIndex = 1;
            // 
            // chkHasHeaders
            // 
            this.chkHasHeaders.AutoSize = true;
            this.chkHasHeaders.Location = new System.Drawing.Point(476, 18);
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
            this.label8.Location = new System.Drawing.Point(592, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "SELECT COLUMN";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 207);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "SQL Connection String";
            // 
            // txtSqlConnectionString
            // 
            this.txtSqlConnectionString.Location = new System.Drawing.Point(15, 223);
            this.txtSqlConnectionString.Name = "txtSqlConnectionString";
            this.txtSqlConnectionString.Size = new System.Drawing.Size(767, 20);
            this.txtSqlConnectionString.TabIndex = 2;
            this.txtSqlConnectionString.Text = "Server=localhost\\MSSQLLocalDB;Database=testing;Trusted_Connection=True;";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioReplace);
            this.groupBox2.Controls.Add(this.radioAdd);
            this.groupBox2.Location = new System.Drawing.Point(15, 278);
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(800, 466);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkHasHeaders);
            this.Controls.Add(this.columnSelector);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtSqlConnectionString);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label9);
            this.Name = "MainForm";
            this.Text = "OFUSCATOR";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNamesFile;
        private System.Windows.Forms.Button btnSelectNamesFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPhoneNumbersFile;
        private System.Windows.Forms.Button btnSelectPhoneNumbersFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNIFsFile;
        private System.Windows.Forms.Button btnSelectNifsFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAddressFile;
        private System.Windows.Forms.Button btnSelectAddressFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLastNamesFile;
        private System.Windows.Forms.Button btnSelectLastNamesFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPhoneNumberColumn;
        private System.Windows.Forms.TextBox txtAddressColumn;
        private System.Windows.Forms.TextBox txtNifsColumn;
        private System.Windows.Forms.TextBox txtLastNameColumn;
        private System.Windows.Forms.TextBox txtNameColumn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel columnSelector;
        private System.Windows.Forms.CheckBox chkHasHeaders;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSqlConnectionString;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioReplace;
        private System.Windows.Forms.RadioButton radioAdd;
        private System.Windows.Forms.Button button1;
    }
}

