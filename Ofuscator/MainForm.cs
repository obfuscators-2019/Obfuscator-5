﻿using Obfuscator.Domain;
using Obfuscator.Entities;
using Obfuscator.Services;
using Obfuscator.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Obfuscator
{
    public partial class MainForm : Form
    {
        private const int ROWS_TO_SHOW_IN_COLUMN_SELECTOR = 10;

        private CancellationTokenSource _cancellationTokenSource;

        public MainForm()
        {
            InitializeComponent();

            ConfigureDataGridBindingSource();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChkHasHeaders_CheckedChanged(object sender, EventArgs e)
        {
            if (columnSelector.Controls.Count == 0) return;

            var formatter = chkHasHeaders.Checked ? new Action<Label>(ColumnSelector_FormatColumnWithHeader) : new Action<Label>(ColumnSelector_FormatColumnWithoutHeader);
            ColumnSelector_FormatHeaders(formatter);
        }

        private void BtnAddNew_Click(object sender, EventArgs e)
        {
            dataSourceInformationBindingSource.AddNew();
            if (sender == btnAddCsv) SelectCsvFileForGridRow();
            else if (sender == btnNIF) SetGeneratorAsDatasource(DataSourceType.NIFGenerator);
            else if (sender == btnDNI) SetGeneratorAsDatasource(DataSourceType.DNIGenerator);
            else if (sender == btnNIE) SetGeneratorAsDatasource(DataSourceType.NIEGenerator);
            else if (sender == btnScramble) SetGeneratorAsDatasource(DataSourceType.Scramble);
        }

        private void GridCell_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (GridButtonToChangeDataSourceWasClicked(senderGrid, e))
                SelectCsvFileForGridRow();

            else if (ClickedOnAGridRow(e))
                SelectCurrentGridRow();
        }

        private void SelectCurrentGridRow()
        {
            if (gridCsvInformation.CurrentRow == null) return;

            gridCsvInformation.CurrentRow.Selected = true;
            var dataSourceInformation = (DataSourceInformation)dataSourceInformationBindingSource.Current;
            SetupColumnSelection(dataSourceInformation);
        }

        private void ColumnSelector_ColumnClick(object sender, EventArgs e)
        {
            var clicked = (Label)sender;
            var firstEnterCharIndex = clicked.Text.IndexOf('\n');

            if (firstEnterCharIndex < 0)
            {
                MessageBox.Show("Can't get this header.\nPlease check the content of this file.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                List<DbTableInfo> tables = RetrieveInformationOfTablesFromDatabase();

                InitializeComboDatabaseTableNames();
                comboDbTableNames.DataSource = tables;
                comboDbTableNames.DisplayMember = "Name";
                comboDbTableNames.SelectedIndexChanged += ComboTableNames_SelectedIndexChanged;
                ComboTableNames_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                var exceptionInfo = $"EXCEPTION: ({ex.GetType().ToString()}) {ex.Message}";
                MessageBox.Show(exceptionInfo, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                InitializeComboDatabaseTableNames();
                InitializeComboDatabaseFields();
            }
        }

        private List<DbTableInfo> RetrieveInformationOfTablesFromDatabase()
        {
            var dataPersistence = new SqlDataPersistence
            {
                ConnectionString = txtSqlConnectionString.Text,
                StatusChanged = StatusInformationChanged,
            };

            var obfuscation = new Obfuscation
            {
                DataPersistence = dataPersistence,
                StatusChanged = StatusInformationChanged,
            };

            if (_cancellationTokenSource?.Token.CanBeCanceled ?? false) _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            var tables = obfuscation.RetrieveDatabaseInfo(_cancellationTokenSource);
            return tables;
        }

        private void StatusInformationChanged(object callbackInfo, EventArgs e)
        {
            var statusInformation = (StatusInformation)callbackInfo;
            SetStatus(statusInformation.Message, statusInformation.Progress, statusInformation.Total);

        }

        private void BtnClearOps_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Are you sure?", "PLEASE CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                lbObfuscationOps.DataSource = new List<ObfuscationParser>();
        }

        private void LbObfuscationOps_KeyDown(object sender, KeyEventArgs e)
        {
            var currentIndex = lbObfuscationOps.SelectedIndex;
            if (currentIndex == -1) return;

            var ops = GetObfuscationOps();
            switch (e.KeyData)
            {
                case Keys.Delete:
                    ops.RemoveAt(currentIndex);
                    break;
                case Keys.Control | Keys.Up:
                    MoveUpListBoxElement(currentIndex, ops);
                    break;
                case Keys.Control | Keys.Down:
                    MoveDownListBoxElement(currentIndex, ops);
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        private void MoveDownListBoxElement(int currentIndex, BindingList<ObfuscationParser> ops)
        {
            if (currentIndex < ops.Count - 1)
            {
                var currentOp = ops[currentIndex];
                ops.Insert(currentIndex + 2, currentOp);
                ops.RemoveAt(currentIndex);
                lbObfuscationOps.SelectedIndex = currentIndex;
            }
        }

        private void MoveUpListBoxElement(int currentIndex, BindingList<ObfuscationParser> ops)
        {
            if (currentIndex > 0)
            {
                var currentOp = ops[currentIndex];
                ops.Insert(currentIndex - 1, currentOp);
                ops.RemoveAt(currentIndex + 1);
                lbObfuscationOps.SelectedIndex = currentIndex;
            }
        }

        private void LbObfuscationOps_Click(object sender, EventArgs e)
        {
            if (lbObfuscationOps.SelectedItem != null)
            {
                var operation = (ObfuscationParser)lbObfuscationOps.SelectedItem;
                txtSqlConnectionString.Text = operation.Destination.ConnectionString;
            }
        }

        private void BtnRunObfuscationOps_Click(object sender, EventArgs e)
        {
            var ofuscation = new Obfuscation
            {
                StatusChanged = StatusInformationChanged,
                DataPersistence = new SqlDataPersistence(),
            };

            var obfuscationOps = GetObfuscationOps();
            ofuscation.RunOperations(obfuscationOps);
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

        private void BtnLoadOps_Click(object sender, EventArgs e)
        {
            var loadDialog = new OpenFileDialog();
            var dialogResult = loadDialog.ShowDialog();
            if (dialogResult == DialogResult.Cancel) return;

            if (_cancellationTokenSource?.Token.CanBeCanceled ?? false) _cancellationTokenSource.Cancel();

            var serializer = new FileSerializer();
            var obfuscationOps = serializer.LoadObfuscationOps(loadDialog.FileName);
            SetObfuscationOps(obfuscationOps.Select(x => new ObfuscationParser(x)));

            lbObfuscationOps.Focus();

            SetStatus($"FILE: {Path.GetFileName(loadDialog.FileName)}", 0, 0);
        }

        private void ComboTableNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeComboDatabaseFields();
            var table = (DbTableInfo)comboDbTableNames.SelectedItem;
            if (table != null)
            {
                if (table.Columns == null || table.Columns.Count == 0)
                    (new Obfuscation { DataPersistence = new SqlDataPersistence { ConnectionString = table.ConnectionString } }).RetrieveTableColumns(table);
                
                if (table.Columns != null && table.Columns.Count > 0)
                {
                    comboDbField.DataSource = table.Columns;
                    comboDbField.DisplayMember = "Name";
                }
            }
        }

        private void BtnCreateObuscationOperation_Click(object sender, EventArgs e)
        {
            if (!EnoughInformationToCreateObfuscationOp()) return;

            var obfuscationOps = GetObfuscationOps();
            var obfuscationOperation = CreateObfuscationInfo();
            obfuscationOps.Add(obfuscationOperation);
            lbObfuscationOps.Refresh();
        }

        private void BtnEditScrambleOperation_Click(object sender, EventArgs e)
        {
            if (!EnoughInformationToEditScrambleOp((Button)sender)) return;

            var obfuscationOps = GetObfuscationOps();
            var currentObfuscationOp = (ObfuscationParser)lbObfuscationOps.SelectedItem;

            var dbColumnInfo = (DbColumnInfo)comboDbField.SelectedItem;
            if (sender == btnAddFieldToScramble)
            {
                dbColumnInfo.IsGroupColumn = false;
                currentObfuscationOp.Destination.Columns.Add(dbColumnInfo);
            }
            else if (sender == btnRemoveFieldFromScramble)
            {
                currentObfuscationOp.Destination.Columns.RemoveAll(c => c.Name == dbColumnInfo.Name);
            }
            else if (sender == btnAddGroupFieldToScramble)
            {
                dbColumnInfo.IsGroupColumn = true;
                currentObfuscationOp.Destination.Columns.Add(dbColumnInfo);
            }

            obfuscationOps.ResetItem(lbObfuscationOps.SelectedIndex);
        }

        private static DialogResult ConfirmToSelectANewFile(DataSourceInformation dataSourceInformation)
        {
            var userResponseToChangeFile = DialogResult.Yes;
            var fileName = dataSourceInformation.DataSourceName;

            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                userResponseToChangeFile = MessageBox.Show("Change file?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return userResponseToChangeFile;
        }

        private void ColumnSelector_FormatHeaders(Action<Label> formatHeader)
        {
            foreach (var control in columnSelector.Controls)
            {
                var csvInfoBounded = (DataSourceInformation)dataSourceInformationBindingSource.Current;
                csvInfoBounded.HasHeaders = chkHasHeaders.Checked;

                var columnContent = (Label)control;
                formatHeader(columnContent);
            }
        }

        private void ColumnSelector_FormatColumnWithoutHeader(Label columnContent)
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

        private void ColumnSelector_FormatColumnWithHeader(Label columnContent)
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

        private void ConfigureDataGridBindingSource()
        {
            gridCsvInformation.AllowUserToAddRows = false;
            dataSourceInformationBindingSource.DataSource = new List<DataSourceInformation> { };
        }

        private void SetGeneratorAsDatasource(DataSourceType dataSourceType)
        {
            var dataSourceInformation = (DataSourceInformation)dataSourceInformationBindingSource.Current;
            dataSourceInformation.DataSourceName = string.Empty;
            dataSourceInformation.ColumnName = string.Empty;
            dataSourceInformation.DataSourceType = dataSourceType;
            SelectCurrentGridRow();
        }

        private static bool ClickedOnAGridRow(DataGridViewCellMouseEventArgs e)
        {
            return e.RowIndex >= 0;
        }

        private bool GridButtonToChangeDataSourceWasClicked(DataGridView senderGrid, DataGridViewCellMouseEventArgs e)
        {
            return e.ColumnIndex >= 0 && senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0;
        }

        private void SelectCsvFileForGridRow()
        {
            var dataSourceInformation = (DataSourceInformation)dataSourceInformationBindingSource.Current;

            DialogResult userResponseToChangeFile = ConfirmToSelectANewFile(dataSourceInformation);

            if (userResponseToChangeFile == DialogResult.Yes)
            {
                var fileName = SelectFile(dataSourceInformation.DataSourceName);
                if (!string.IsNullOrEmpty(fileName))
                {
                    dataSourceInformation.DataSourceType = DataSourceType.CSV;
                    dataSourceInformation.DataSourceName = fileName;
                    dataSourceInformation.ColumnName = string.Empty;
                }
                else if (dataSourceInformation.DataSourceType == DataSourceType.CSV && string.IsNullOrEmpty(dataSourceInformation.DataSourceName))
                    dataSourceInformationBindingSource.Remove(dataSourceInformation);
            }

            SelectCurrentGridRow();
            gridCsvInformation.Refresh();
        }

        private void SetupColumnSelection(DataSourceInformation dataSourceInformation)
        {
            columnSelector.Controls.Clear();
            BackupCsvInstanceForCurrentColumns(dataSourceInformation);

            if (dataSourceInformation.DataSourceType != DataSourceType.CSV) return;

            CsvFile csvFile = ReadFiveLinesFromCsv(dataSourceInformation);
            IEnumerable<string> headers = csvFile.GetHeaders();

            var previousCoordinate = 0;
            for (int columnIndex = 0; columnIndex < csvFile.GetColumns(); columnIndex++)
            {
                Label label = CreateLabelForColumn();
                AddColumnIndexAndHeadersToLabel(dataSourceInformation, headers, columnIndex, label);
                AddContentToLabel(csvFile, columnIndex, label);

                label.Click += ColumnSelector_ColumnClick;
                label.Left = previousCoordinate + 2;
                previousCoordinate += label.Width;

                columnSelector.Controls.Add(label);
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
            csvFile.ReadFile(dataSourceInformation.DataSourceName, ROWS_TO_SHOW_IN_COLUMN_SELECTOR);
            csvFile.HasHeaders = chkHasHeaders.Checked = dataSourceInformation.HasHeaders;
            return csvFile;
        }

        private Label CreateLabelForColumn()
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

        private BindingList<ObfuscationParser> GetObfuscationOps()
        {
            if (lbObfuscationOps.DataSource == null || !(lbObfuscationOps.DataSource is BindingList<ObfuscationParser>))
                SetObfuscationOps(new BindingList<ObfuscationParser>());

            return (BindingList<ObfuscationParser>)lbObfuscationOps.DataSource;
        }

        private void SetObfuscationOps(IEnumerable<ObfuscationParser> obfuscationOps)
        {
            lbObfuscationOps.DataSource = new BindingList<ObfuscationParser>(obfuscationOps.ToList()); 
            lbObfuscationOps.DisplayMember = "ReadableContent";
            if (lbObfuscationOps.Items.Count > 0)
            {
                lbObfuscationOps.SelectedIndex = 0;
                LbObfuscationOps_Click(this, null);
            }
        }

        private ObfuscationParser CreateObfuscationInfo()
        {
            return new ObfuscationParser()
            {
                Origin = (DataSourceInformation)gridCsvInformation.SelectedRows[0].DataBoundItem,
                Destination = new DbTableInfo
                {
                    ConnectionString = txtSqlConnectionString.Text,
                    Name = comboDbTableNames.Text,
                    Columns = new List<DbColumnInfo> { (DbColumnInfo)comboDbField.SelectedItem }
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

        private bool EnoughInformationToEditScrambleOp(Button sender)
        {
            var currentObfuscationOp = (ObfuscationParser)lbObfuscationOps?.SelectedItem;

            if (currentObfuscationOp?.Origin.DataSourceType != DataSourceType.Scramble)
            {
                MessageBox.Show("This operation can only be done over a Scrambling Ofuscation", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (txtSqlConnectionString.Text != currentObfuscationOp.Destination.ConnectionString)
            {
                MessageBox.Show("Connection string does not match on the selected obfuscation operation", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var table = (DbTableInfo)comboDbTableNames.SelectedItem;
            if (table?.Name != currentObfuscationOp.Destination.Name)
            {
                MessageBox.Show("Selected destination table does not match on the selected obfuscation operation", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (comboDbField.Items?.Count == 0 || comboDbField.SelectedIndex < 0)
            {
                MessageBox.Show("Select the database info - what will be obfuscated\n(CONNECT TO A DATABASE, SELECT TABLE AND COLUMN)", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                var selectedColumn = (DbColumnInfo)comboDbField.SelectedItem;
                if ((sender == btnAddFieldToScramble || sender == btnAddGroupFieldToScramble) && currentObfuscationOp.Destination.Columns.Any(x => x.Name == selectedColumn.Name))
                {
                    MessageBox.Show("Column is already selected either for scramble or for grouping.\nPlease remove it before using it again in the same obfuscation operation", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (sender == btnRemoveFieldFromScramble)
                {
                    if (!currentObfuscationOp.Destination.Columns.Any(x => x.Name == selectedColumn.Name))
                    {
                        MessageBox.Show("Can't remove a column that was not previously selected for scramble", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    else if (!selectedColumn.IsGroupColumn && currentObfuscationOp.Destination.Columns.Count(c => !c.IsGroupColumn) == 1)
                    {
                        MessageBox.Show("Can't leave an obfuscation operation without columns to operate in", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            return true;
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
                Application.DoEvents();
            }
        }
    }
}
