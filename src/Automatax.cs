using Automatax.Models;
using Automatax.Parsers;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Automatax
{
    public partial class Automatax : Form
    {

        private IParser _parser;
        private IAutomaton _automaton;

        public Automatax()
        {
            _parser = new Parser();

            InitializeComponent();
        }

        private void loadButton_Click(object sender, System.EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    inputTextBox.Text = openFileDialog.FileName;
                    inputTextBox.ReadOnly = true;
                }
            }
        }

        private void readButton_Click(object sender, System.EventArgs e)
        {
            loadButton.Enabled = false;
            readButton.Enabled = false;

            if (string.IsNullOrEmpty(inputTextBox.Text))
            {
                message1.Text = "Invalid input.";
                return;
            }

            // Read from file
            using (var reader = new StreamReader(inputTextBox.Text))
            {
                _automaton = _parser.Parse(reader);
            }

            // Show PictureBox
            File.WriteAllText("automaton.dot", _automaton.ToString());
            Process dot = new Process();
            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = "-Tpng -oautomaton.png automaton.dot";
            dot.Start();
            dot.WaitForExit();
            pictureBox1.ImageLocation = "automaton.png";

            // Is dfa
            bool? expected = _automaton.TestVector.IsDfa;
            bool actual = _automaton.IsDfa();

            if (expected == null)
            {
                isDfaTextBox.Text = actual.ToString();
                message2.Text = "No test vector.";
            }
            else
            {
                isDfaTextBox.Text = expected.ToString();

                if (actual == expected)
                    isDfaTextBox.BackColor = Color.LightGreen;
                else
                {
                    isDfaTextBox.BackColor = Color.Red;
                    message2.Text = "Test vector is incorrect.";
                }
            }

            // Words
            foreach (var item in _automaton.TestVector.Words)
            {
                expected = _automaton.TestVector.Words[item.Key];
                actual = _automaton.Accepts(item.Key);
                var row = (DataGridViewRow)resultsGridView.Rows[0].Clone();

                row.Cells[0].Value = item.Key;

                if (expected == null)
                {
                    row.Cells[1].Value = actual;
                    row.Cells[2].Value = "No test vector.";
                }
                else
                {
                    row.Cells[1].Value = expected;

                    if (actual == expected)
                        row.Cells[1].Style.BackColor = Color.LightGreen;
                    else
                    {
                        row.Cells[1].Style.BackColor = Color.Red;
                        row.Cells[2].Value = "Test vector is incorrect.";
                    }
                }

                resultsGridView.Rows.Add(row);
            }
        }

        private void clearButton_Click(object sender, System.EventArgs e)
        {
            loadButton.Enabled = true;
            readButton.Enabled = true;
            inputTextBox.Text = string.Empty;
            inputTextBox.ReadOnly = false;
            pictureBox1.Image = null;
            message1.Text = string.Empty;
            isDfaTextBox.Text = string.Empty;
            isDfaTextBox.BackColor = Color.White;
            message2.Text = string.Empty;
            resultsGridView.Rows.Clear();
        }

        private void resultsGridView_SelectionChanged(object sender, System.EventArgs e)
        {
            resultsGridView.ClearSelection();
        }
    }
}
