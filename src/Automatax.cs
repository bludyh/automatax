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
                    if (line.Split(':').Last().Trim() == "y")
                        testVector.IsDfa = true;
                    else if (line.Split(':').Last().Trim() == "n")
                        testVector.IsDfa = false;
            }

            return new Automaton(alphabet, states, finalStates, transitions) { TestVector = testVector };
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

            // Test vectors
            isDfaTextBox.Text = _automaton.IsDfa().ToString();
            if (_automaton.IsDfa() == _automaton.TestVector.IsDfa)
                isDfaTextBox.BackColor = Color.Green;
            else
            {
                isDfaTextBox.BackColor = Color.Red;
                message2.Text = "Test vector is incorrect.";
            }
        }

        private void clearButton_Click(object sender, System.EventArgs e)
        {
            inputTextBox.Text = string.Empty;
            inputTextBox.ReadOnly = false;
            pictureBox1.Image = null;
            message1.Text = string.Empty;
            isDfaTextBox.Text = string.Empty;
            isDfaTextBox.BackColor = Color.White;
            message2.Text = string.Empty;
        }
    }
}
