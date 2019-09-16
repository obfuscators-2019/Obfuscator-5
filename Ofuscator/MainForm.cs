﻿using Obfuscator.Domain;
using Obfuscator.Entities;
using Obfuscator.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Obfuscator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            ConfigureDataGridBindingSource();
        }

        private static DialogResult ConfirmToSelectANewFile(string fileName)
        {
            var userResponseToChangeFile = DialogResult.Yes;

            if (!string.IsNullOrEmpty(fileName))
                userResponseToChangeFile = MessageBox.Show("Change file?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return userResponseToChangeFile;
        }

        private void ChkHasHeaders_CheckedChanged(object sender, EventArgs e)
        {
            if (columnSelector.Controls.Count == 0) return;

            foreach (var control in columnSelector.Controls)
            {
                var csvInfoBounded = (CsvInformation)dataSourceInformationBindingSource.Current;
                csvInfoBounded.HasHeaders = chkHasHeaders.Checked;

                var columnContent = (Label)control;
                if (chkHasHeaders.Checked)
                    FormatColumnWithHeader(columnContent);
                else
                    FormatColumnWithoutHeader(columnContent);
            }
        }

        private void FormatColumnWithoutHeader(Label columnContent)
        {
            var closingBracketsCharIndex = columnContent.Text.IndexOf(']', 3);
            if (closingBracketsCharIndex < 0)
                columnContent.Text = string.Empty;
            else
                columnContent.Text = columnContent.Text.Substring(0, 1)
                    + "\n"
                    + columnContent.Text.Substring(3, closingBracketsCharIndex - 3)
                    + "\n"
                    + columnContent.Text.Substring(closingBracketsCharIndex + 2);
        }

        private void FormatColumnWithHeader(Label columnContent)
        {
            var secondColumnCharIndex = columnContent.Text.IndexOf('\n', 2);
            columnContent.Text = columnContent.Text.Substring(0, 1)
                + " ["
                + columnContent.Text.Substring(2, secondColumnCharIndex - 2)
                + "]\n"
                + columnContent.Text.Substring(secondColumnCharIndex + 1);
        }

        private string SelectFile(string previousFileName)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = previousFileName;
            var dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
                return openFileDialog.FileName;
            else
                return null;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfigureDataGridBindingSource()
        {
            gridCsvInformation.AllowUserToAddRows = false;
            dataSourceInformationBindingSource.DataSource = new List<CsvInformation> { };
        }

        private void BtnAddNew_Click(object sender, EventArgs e)
        {
            dataSourceInformationBindingSource.AddNew();
            SelectCsvFileForGridRow();
        }

        private void GridCell_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex >= 0 && senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                SelectCsvFileForGridRow();
            else if (e.RowIndex >= 0)
            {
                var csvInformation = (CsvInformation)dataSourceInformationBindingSource.Current;
                SetupColumnSelection(csvInformation);
            }
        }

        private void SelectCsvFileForGridRow()
        {
            var csvInformation = (CsvInformation)dataSourceInformationBindingSource.Current;

            if (!File.Exists(csvInformation.FileName))
                csvInformation.FileName = string.Empty;

            DialogResult userResponseToChangeFile = ConfirmToSelectANewFile(csvInformation.FileName);

            if (userResponseToChangeFile == DialogResult.Yes)
            {
                var fileName = SelectFile(csvInformation.FileName);
                if (fileName != null)
                {
                    csvInformation.FileName = fileName;
                    csvInformation.ColumnName = string.Empty;
                }
                else
                    fileName = csvInformation.FileName;

                if (string.IsNullOrEmpty(fileName))
                {
                    dataSourceInformationBindingSource.Remove(csvInformation);
                    gridCsvInformation.Refresh();
                    return;
                }
            }

            SetupColumnSelection(csvInformation);

            gridCsvInformation.Refresh();
        }

        private void SetupColumnSelection(CsvInformation csvInformation)
        {
            columnSelector.Controls.Clear();
            BackupCsvInstanceForCurrentColumns(csvInformation);

            CsvFile csvFile = ReadFiveLinesFromCsv(csvInformation);
            IEnumerable<string> headers = csvFile.GetHeaders();

            var previousCoordinate = 0;
            for (int columnIndex = 0; columnIndex < csvFile.GetColumns(); columnIndex++)
            {
                Label label = CreateLabel();
                AddColumnIndexAndHeadersToLabel(csvInformation, headers, columnIndex, label);
                AddContentToLabel(csvFile, columnIndex, label);

                label.Click += LabelColumn_Click;
                label.Left = previousCoordinate + 2;
                previousCoordinate += label.Width;

                columnSelector.Controls.Add(label);
            }
        }

        private void LabelColumn_Click(object sender, EventArgs e)
        {
            var clicked = (Label)sender;
            var firstEnterCharIndex = clicked.Text.IndexOf('\n');
            if (firstEnterCharIndex < 0)
                MessageBox.Show("Can't get this header.\nPlease check the content of this file.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                var csvDataSource = (List<CsvInformation>)dataSourceInformationBindingSource.DataSource;
                CsvInformation currentCsvInfo = GetCsvInstanceForCurrentColumns();
                var csvInfoBounded = csvDataSource.FirstOrDefault(x => x == currentCsvInfo);
                if (chkHasHeaders.Checked)
                {
                    var firstBracketIndex = clicked.Text.IndexOf('[');
                    csvInfoBounded.ColumnIndex = int.Parse(clicked.Text.Substring(0, firstBracketIndex));
                    csvInfoBounded.ColumnName = $"{clicked.Text.Substring(firstBracketIndex + 1, firstEnterCharIndex - firstBracketIndex - 2)}";
                }
                else
                {
                    csvInfoBounded.ColumnIndex = int.Parse(clicked.Text.Substring(0, firstEnterCharIndex));
                    csvInfoBounded.ColumnName = "";
                }
                gridCsvInformation.Refresh();
            }
        }

        private void AddContentToLabel(CsvFile csvFile, int columnIndex, Label label)
        {
            foreach (var columnContent in csvFile.GetContent(columnIndex))
                label.Text += $"\n{columnContent}";
        }

        private void AddColumnIndexAndHeadersToLabel(CsvInformation csvInformation, IEnumerable<string> headers, int columnIndex, Label label)
        {
            if (csvInformation.HasHeaders) label.Text = $"{columnIndex} [{headers.ElementAt(columnIndex)}]";
            else label.Text = $"{columnIndex}";
        }

        private CsvFile ReadFiveLinesFromCsv(CsvInformation csvInformation)
        {
            var csvFile = new CsvFile();
            csvFile.ReadFile(csvInformation.FileName, 5);
            csvFile.HasHeaders = chkHasHeaders.Checked = csvInformation.HasHeaders;
            return csvFile;
        }

        private Label CreateLabel()
        {
            return new Label
            {
                BorderStyle = BorderStyle.FixedSingle,
                AutoSize = false,
                Height = columnSelector.Height
            };
        }

        private CsvInformation GetCsvInstanceForCurrentColumns()
        {
            return (CsvInformation)columnSelector.Tag;
        }

        private void BackupCsvInstanceForCurrentColumns(CsvInformation csvInformation)
        {
            columnSelector.Tag = csvInformation;
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                var sqlDb = new SqlDatabase { ConnectionString = txtSqlConnectionString.Text };
                var tables = sqlDb.RetrieveDatabaseInfo();

                InitializeComboDatabaseTableNames();
                comboDbTableNames.DataSource = tables;
                comboDbTableNames.DisplayMember = "Name";
                comboDbTableNames.SelectedIndexChanged += ComboTableNames_SelectedIndexChanged;
                ComboTableNames_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                var exceptionInfo = $"EXCEPTION: ({ex.GetType().ToString()}) {ex.Message}";
                Trace.WriteLine(exceptionInfo);
                MessageBox.Show(exceptionInfo, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                InitializeComboDatabaseTableNames();
                InitializeComboDatabaseFields();
            }
        }

        private void ComboTableNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeComboDatabaseFields();
            var table = (DbTableInfo)comboDbTableNames.SelectedItem;
            if (table != null && table.Columns != null)
            {
                comboDbField.DataSource = table.Columns;
                comboDbField.DisplayMember = "Name";
            }
        }

        private void InitializeComboDatabaseFields()
        {
            comboDbField.DisplayMember = string.Empty;
            comboDbField.DataSource = null;
            comboDbField.Text = "Select field...";
        }

        private void InitializeComboDatabaseTableNames()
        {
            comboDbTableNames.SelectedIndexChanged -= ComboTableNames_SelectedIndexChanged;
            comboDbTableNames.DisplayMember = string.Empty;
            comboDbTableNames.DataSource = null;
            comboDbTableNames.Text = "Select table...";
        }

        private void BtnCreateObuscationOperation_Click(object sender, EventArgs e)
        {
            if (!EnoughInformationToCreateObfuscationOp()) return;

            var obfuscationOps = GetObfuscationOps();
            var obfuscationOperation = CreateObfuscationInfo();
            obfuscationOps.Add(obfuscationOperation);
            lbObfuscationOps.Refresh();
        }

        private BindingList<ObfuscationParser> GetObfuscationOps()
        {
            if (lbObfuscationOps.DataSource == null)
            {
                lbObfuscationOps.DataSource = new BindingList<ObfuscationParser>();
                lbObfuscationOps.DisplayMember = "ReadableContent";
            }
            return (BindingList<ObfuscationParser>)lbObfuscationOps.DataSource;
        }

        private ObfuscationParser CreateObfuscationInfo()
        {
            return new ObfuscationParser()
            {
                Origin = (CsvInformation)gridCsvInformation.SelectedRows[0].DataBoundItem,
                Destination = new DbInfo
                {
                    ConnectionString = txtSqlConnectionString.Text,
                    TableName = comboDbTableNames.Text,
                    ColumnInfo = (DbColumnInfo)comboDbField.SelectedItem
                }
            };
        }

        private bool EnoughInformationToCreateObfuscationOp()
        {
            if (gridCsvInformation.Rows.Count == 0)
            {
                MessageBox.Show("There is no obfuscation data on the grid above\n(PICK FILE AND COLUMN)", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (gridCsvInformation.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select the obfuscation datasource on the grid above\n(SELECT A ROW)", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (comboDbField.Items?.Count == 0 || comboDbField.SelectedIndex < 0)
            {
                MessageBox.Show("Select the database info - what will be obfuscated\n(CONNECT TO A DATABASE, SELECT TABLE AND COLUMN)", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void BtnClearOps_Click(object sender, EventArgs e)
        {
            lbObfuscationOps.DataSource = new BindingList<ObfuscationParser>();
        }

        private void LbObfuscationOps_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && lbObfuscationOps.SelectedIndex >= 0)
            {
                var ops = GetObfuscationOps();
                ops.Remove((ObfuscationParser)lbObfuscationOps.SelectedItem);
            }
        }

        private void BtnRunObfuscationOps_Click(object sender, EventArgs e)
        {
            var obfuscationOps = GetObfuscationOps();
            var sqlDb = new SqlDatabase { ConnectionString = txtSqlConnectionString.Text };

            foreach (var obfuscation in obfuscationOps)
            {
                sqlDb.RunOperation(obfuscation);
            }
        }
    }
}
