using Ofuscator.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }

        private void btnSelectFileAndColumn_Click(object sender, EventArgs e)
        {
            columnSelector.Controls.Clear();

            TextBox[] txtCsvFileNameAndColumn = GetTextBoxesFor((Button)sender);
            var fileName = txtCsvFileNameAndColumn[0].Text;

            if (!File.Exists(fileName)) fileName = "";

            DialogResult userResponseToChangeFile = ConfirmToSelectANewFile(fileName);

            if (userResponseToChangeFile == DialogResult.Yes)
            {
                fileName = SelectFile(txtCsvFileNameAndColumn[0].Text);
                if (fileName == null) return;
                txtCsvFileNameAndColumn[0].Text = fileName;
                txtCsvFileNameAndColumn[1].Text = string.Empty;
            }

            SetupColumnSelection(fileName, txtCsvFileNameAndColumn[1]);
        }

        private static DialogResult ConfirmToSelectANewFile(string fileName)
        {
            var userResponseToChangeFile = DialogResult.Yes;

            if (!string.IsNullOrEmpty(fileName))
                userResponseToChangeFile = MessageBox.Show("Change file?", "CONFIRM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return userResponseToChangeFile;
        }

        private void SetupColumnSelection(string fileName, TextBox txtColumnName)
        {
            var csvFile = new CsvFile();
            csvFile.ReadFile(fileName, 5);
            csvFile.HasHeaders = chkHasHeaders.Checked = true;
            var headers = csvFile.GetHeaders();

            int columnIndex = 0;
            var previousCoordinate = 0;
            foreach (var header in headers)
            {
                var label = new Label {
                    Text = $"{columnIndex} [{header}]",
                    BorderStyle = BorderStyle.FixedSingle,
                    AutoSize = false,
                    Height = columnSelector.Height
                };
                foreach (var columnContent in csvFile.GetContent(columnIndex))
                {
                    label.Text += $"\n{columnContent}";
                }
                label.Click += (sender, e) =>
                {
                    var clicked = (Label)sender;
                    var firstEnterCharIndex = clicked.Text.IndexOf('\n');
                    if (firstEnterCharIndex < 0)
                        MessageBox.Show("Can't get this header.\nPlease check the content of this file.", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        txtColumnName.Text = $"{clicked.Text.Substring(0, firstEnterCharIndex)}";
                };
                label.Left = previousCoordinate + 2;
                columnSelector.Controls.Add(label);
                previousCoordinate += label.Width;
                columnIndex++;
            }
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

        private TextBox[] GetTextBoxesFor(Button sender)
        {
            TextBox[] result = null;

            if (sender == btnSelectNamesFile)
                    result = new TextBox[] { txtNamesFile, txtNameColumn };

            else if(sender == btnSelectLastNamesFile)
                    result = new TextBox[] { txtLastNamesFile, txtLastNameColumn };

            else if (sender == btnSelectAddressFile)
                    result = new TextBox[] { txtAddressFile, txtAddressColumn};

            else if (sender == btnSelectNifsFile)
                result = new TextBox[] { txtNIFsFile, txtNifsColumn};

            else if (sender == btnSelectPhoneNumbersFile)
                result = new TextBox[] { txtPhoneNumbersFile, txtPhoneNumberColumn};

            return result;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
