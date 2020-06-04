using Automatax.Exceptions;
using Automatax.Models;
using Automatax.Parsers;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Automatax
{
    public partial class Automatax : Form
    {

        public Automatax()
        {
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

            IAutomaton automaton;
            TestVector testVector;
            IParser parser = new Parser();

            // Read from file
            using (var reader = new StreamReader(inputTextBox.Text))
            {
                try
                {
                    automaton = parser.Parse(reader);
                    testVector = parser.ParseTests(reader);
                }
                catch (InvalidSyntaxException ex)
                {
                    message1.Text = ex.Message;
                    return;
                }
            }

            // Show Graph
            File.WriteAllText("automaton.dot", automaton.ToString());
            Process dot = new Process();
            dot.StartInfo.FileName = "dot.exe";
            dot.StartInfo.Arguments = "-Tpng -oautomaton.png automaton.dot";
            dot.Start();
            dot.WaitForExit();

            Process.Start("automaton.png");

            // Is dfa
            bool? expected = testVector.IsDfa;
            bool actual = automaton.IsDfa();

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
            foreach (var item in testVector.Words)
            {
                expected = testVector.Words[item.Key];
                actual = automaton.Accepts(item.Key);
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
