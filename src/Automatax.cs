using Automatax.Models;
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

        private Automaton _automaton;

        public Automatax()
        {
            InitializeComponent();
        }

        private static Automaton Parse(StreamReader reader)
        {
            List<char> alphabet = new List<char>();
            List<string> states = new List<string>();
            List<string> finalStates = new List<string>();
            List<Transition> transitions = new List<Transition>();
            TestVector testVector = new TestVector();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();

                if (line == string.Empty || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("alphabet"))
                    alphabet = line.Split(':').Last().ToCharArray().Where(c => char.IsLetterOrDigit(c)).ToList();

                if (line.StartsWith("states"))
                    states = line.Split(':').Last().Trim().Split(',').ToList();

                if (line.StartsWith("final"))
                    finalStates = line.Split(':').Last().Trim().Split(',').ToList();

                if (line.StartsWith("transitions"))
                {
                    string startState;
                    char symbol;
                    string endState;
                    Transition transition;
                    while ((line = reader.ReadLine()) != null && !line.Contains("end"))
                    {
                        line = line.Trim();

                        if (line == string.Empty || line.StartsWith("#"))
                            continue;

                        startState = line.Split(',').First();
                        symbol = line.Split(',').Last().ToCharArray().First();
                        endState = line.Split(new string[] { "-->" }, System.StringSplitOptions.None).Last().Trim();
                        transition = new Transition(startState, symbol, endState);
                        transitions.Add(transition);
                    }
                }

                if (line.StartsWith("dfa"))
                    testVector.IsDfa = StringToBool(line.Split(':').Last().Trim());

                if (line.StartsWith("words"))
                {
                    while ((line = reader.ReadLine()) != null && !line.Contains("end"))
                    {
                        line = line.Trim();

                        if (line == string.Empty || line.StartsWith("#"))
                            continue;

                        testVector.Words.Add(line.Split(',').First(), StringToBool(line.Split(',').Last()));
                    }
                }
            }

            return new Automaton(alphabet, states, finalStates, transitions) { TestVector = testVector };
        }

        private static bool? StringToBool(string input)
        {
            if (input == "y") return true;
            else if (input == "n") return false;

            return null;
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
                _automaton = Parse(reader);
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
