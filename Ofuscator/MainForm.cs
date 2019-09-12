using Ofuscator.Domain;
using Ofuscator.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ofuscator
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
                var columnContent = (Label)control;
                if (chkHasHeaders.Checked)
                    FormatColumnWithHeader(columnContent);
                else
                    FormatColumnWithoutHeader(columnContent);
            }
        }

        private static void FormatColumnWithoutHeader(Label columnContent)
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

        private static void FormatColumnWithHeader(Label columnContent)
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
            dataGridCsvInformation.AllowUserToAddRows = false;
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
                    dataGridCsvInformation.Refresh();
                    return;
                }
            }

            SetupColumnSelection(csvInformation);

            dataGridCsvInformation.Refresh();
        }

        private void SetupColumnSelection(CsvInformation csvInformation)
        {
            var csvFile = new CsvFile();
            csvFile.ReadFile(csvInformation.FileName, 5);
            csvFile.HasHeaders = chkHasHeaders.Checked = true;
            var headers = csvFile.GetHeaders();

            columnSelector.Controls.Clear();
            columnSelector.Tag = csvInformation;

            int columnIndex = 0;
            var previousCoordinate = 0;
            foreach (var header in headers)
            {
                var label = new Label
                {
                    Text = $"{columnIndex} [{header}]",
                    BorderStyle = BorderStyle.FixedSingle,
                    AutoSize = false,
                    Height = columnSelector.Height
                };

                foreach (var columnContent in csvFile.GetContent(columnIndex))
                    label.Text += $"\n{columnContent}";

                label.Click += (sender, e) =>
                {
                    var clicked = (Label)sender;
                    var firstEnterCharIndex = clicked.Text.IndexOf('\n');
                    if (firstEnterCharIndex < 0)
                        MessageBox.Show("Can't get this header.\nPlease check the content of this file.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        var csvDataSource = (List<CsvInformation>)dataSourceInformationBindingSource.DataSource;
                        var csvInfoBounded = csvDataSource.FirstOrDefault(x => x == (CsvInformation)columnSelector.Tag);
                        if (chkHasHeaders.Checked)
                        {
                            var firstBracketIndex = clicked.Text.IndexOf('[');
                            csvInformation.ColumnIndex = int.Parse(clicked.Text.Substring(0, firstBracketIndex));
                            csvInformation.ColumnName = $"{clicked.Text.Substring(firstBracketIndex+1, firstEnterCharIndex-firstBracketIndex-2)}";
                        }
                        else
                        {
                            csvInformation.ColumnIndex = int.Parse(clicked.Text.Substring(0, firstEnterCharIndex));
                            csvInformation.ColumnName = "";
                        }
                        dataGridCsvInformation.Refresh();
                    }
                };

                label.Left = previousCoordinate + 2;
                columnSelector.Controls.Add(label);
                previousCoordinate += label.Width;
                columnIndex++;
            }
        }
    }
}
