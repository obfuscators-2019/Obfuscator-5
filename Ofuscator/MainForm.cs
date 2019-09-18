﻿using Obfuscator.Domain;
using Obfuscator.Entities;
using Obfuscator.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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
                var csvInfoBounded = (DataSourceInformation)dataSourceInformationBindingSource.Current;
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
            dataSourceInformationBindingSource.DataSource = new List<DataSourceInformation> { };
        }

        private void BtnAddNew_Click(object sender, EventArgs e)
        {
            dataSourceInformationBindingSource.AddNew();
            if (sender == btnAddCsv) SelectCsvFileForGridRow();
            else SetNIFGeneratorAsDatasource();
        }

        private void SetNIFGeneratorAsDatasource()
        {
            var dataSourceInformation = (DataSourceInformation)dataSourceInformationBindingSource.Current;
            dataSourceInformation.DataSourceName = DataSourceBase.GetDataSourcePrefix(DataSourceType.NIFGenerator);
            dataSourceInformation.ColumnName = string.Empty;
            SetupColumnSelection(dataSourceInformation);
        }

        private void GridCell_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (GridButtonToChangeDataSourceWasClicked(e, senderGrid))
                SelectCsvFileForGridRow();
            else if (ClickedOnAGridRow(e))
            {
                var dataSourceInformation = (DataSourceInformation)dataSourceInformationBindingSource.Current;
                SetupColumnSelection(dataSourceInformation);
            }
        }

        private static bool ClickedOnAGridRow(DataGridViewCellMouseEventArgs e)
        {
            return e.RowIndex >= 0;
        }

        private static bool GridButtonToChangeDataSourceWasClicked(DataGridViewCellMouseEventArgs e, DataGridView senderGrid)
        {
            return e.ColumnIndex >= 0 && senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0;
        }

        private void SelectCsvFileForGridRow()
        {
            var dataSourceInformation = (DataSourceInformation)dataSourceInformationBindingSource.Current;

            if (!File.Exists(dataSourceInformation.DataSourceName))
                dataSourceInformation.DataSourceName = string.Empty;

            DialogResult userResponseToChangeFile = ConfirmToSelectANewFile(dataSourceInformation.DataSourceName);

            if (userResponseToChangeFile == DialogResult.Yes)
            {
                var fileName = SelectFile(dataSourceInformation.DataSourceName);
                if (fileName != null)
                {
                    dataSourceInformation.DataSourceName = DataSourceBase.GetDataSourcePrefix(DataSourceType.CSV) + fileName;
                    dataSourceInformation.ColumnName = string.Empty;
                }
                else
                    fileName = dataSourceInformation.DataSourceName;

                if (string.IsNullOrEmpty(fileName))
                {
                    dataSourceInformationBindingSource.Remove(dataSourceInformation);
                    gridCsvInformation.Refresh();
                    return;
                }
            }

            SetupColumnSelection(dataSourceInformation);

            gridCsvInformation.Refresh();
        }

        private void SetupColumnSelection(DataSourceInformation dataSourceInformation)
        {
            columnSelector.Controls.Clear();
            BackupCsvInstanceForCurrentColumns(dataSourceInformation);

            if (DataSourceBase.IsNifGenerator(dataSourceInformation.DataSourceName)) return;

            CsvFile csvFile = ReadFiveLinesFromCsv(dataSourceInformation);
            IEnumerable<string> headers = csvFile.GetHeaders();

            var previousCoordinate = 0;
            for (int columnIndex = 0; columnIndex < csvFile.GetColumns(); columnIndex++)
            {
                Label label = CreateLabel();
                AddColumnIndexAndHeadersToLabel(dataSourceInformation, headers, columnIndex, label);
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
                var csvDataSource = (List<DataSourceInformation>)dataSourceInformationBindingSource.DataSource;
                DataSourceInformation currentCsvInfo = GetCsvInstanceForCurrentColumns();
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

        private void AddColumnIndexAndHeadersToLabel(DataSourceInformation csvInformation, IEnumerable<string> headers, int columnIndex, Label label)
        {
            if (csvInformation.HasHeaders) label.Text = $"{columnIndex} [{headers.ElementAt(columnIndex)}]";
            else label.Text = $"{columnIndex}";
        }

        private CsvFile ReadFiveLinesFromCsv(DataSourceInformation dataSourceInformation)
        {
            var csvFile = new CsvFile();
            csvFile.ReadFile(dataSourceInformation.DataSourceName.Substring(DataSourceBase.GetDataSourcePrefix(DataSourceType.CSV).Length), 5);
            csvFile.HasHeaders = chkHasHeaders.Checked = dataSourceInformation.HasHeaders;
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

        private DataSourceInformation GetCsvInstanceForCurrentColumns()
        {
            return (DataSourceInformation)columnSelector.Tag;
        }

        private void BackupCsvInstanceForCurrentColumns(DataSourceInformation csvInformation)
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
                SetObfuscationOps(new BindingList<ObfuscationParser>());

            return (BindingList<ObfuscationParser>)lbObfuscationOps.DataSource;
        }

        private void SetObfuscationOps(IEnumerable<ObfuscationParser> obfuscationOps)
        {
            lbObfuscationOps.DataSource = new BindingList<ObfuscationParser>(obfuscationOps.ToList()); 
            lbObfuscationOps.DisplayMember = "ReadableContent";
        }

        private ObfuscationParser CreateObfuscationInfo()
        {
            return new ObfuscationParser()
            {
                Origin = (DataSourceInformation)gridCsvInformation.SelectedRows[0].DataBoundItem,
                Destination = new DbInfo
                {
                    ConnectionString = txtSqlConnectionString.Text,
                    TableName = comboDbTableNames.Text,
                    ColumnInfo = (DbColumnInfo)comboDbField.SelectedItem,
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
            if (DialogResult.Yes == MessageBox.Show("Are you sure?", "PLEASE CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                lbObfuscationOps.DataSource = new List<ObfuscationParser>();
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
            var sqlDb = new SqlDatabase();
            sqlDb.StatusChanged += (callbackInfo, callbackArgs) =>
            {
                var statusInformation = (SqlDatabase.StatusInformation)callbackInfo;
                SetStatus(statusInformation.Message, statusInformation.Progress, statusInformation.Total);
            };

            var obfuscationOps = GetObfuscationOps();
            sqlDb.RunOperations(obfuscationOps);
        }

        private void SetStatus(string text, int progress, int max)
        {
            if (this.InvokeRequired)
            {
               this.Invoke((MethodInvoker)delegate
               {
                   SetStatus(text, progress, max);
               });
            }
            else
            {
                toolStripStatusLabel1.Text = text;
                toolStripProgressBar1.Maximum = max;
                toolStripProgressBar1.Value = progress;
            }
        }

        private void BtnSaveOps_Click(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            var dialogResult = saveDialog.ShowDialog();
            if (dialogResult == DialogResult.Cancel) return;

            var obfuscationOps = GetObfuscationOps();
            var serializer = new FileSerializer();
            serializer.SaveObfuscationOps(obfuscationOps, saveDialog.FileName);

            SetStatus($"FILE {Path.GetFileName(saveDialog.FileName)} SAVED at {DateTime.Now.ToShortTimeString()}", 0, 0);
        }

        private void BtnOpenOps_Click(object sender, EventArgs e)
        {
            var loadDialog = new OpenFileDialog();
            var dialogResult = loadDialog.ShowDialog();
            if (dialogResult == DialogResult.Cancel) return;

            var serializer = new FileSerializer();
            var obfuscationOps = serializer.LoadObfuscationOps(loadDialog.FileName);
            SetObfuscationOps(obfuscationOps.Select(x => new ObfuscationParser(x)));

            SetStatus($"FILE: {Path.GetFileName(loadDialog.FileName)}", 0, 0);
        }
    }
}
