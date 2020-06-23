using System.Collections.Generic;
using System.Linq;

namespace Automatax.Models
{
    public class Grammar : IAutomaton
    {

        private Automaton _pda;

        public Grammar(List<char> variables, List<char> terminals, List<Rule> rules, char startVariable)
        {
            Variables = variables;
            Terminals = terminals;
            Rules = rules;
            StartVariable = startVariable;
        }

        public List<char> Variables { get; }
        public List<char> Terminals { get; }
        public List<Rule> Rules { get; }
        public char StartVariable { get; }

        public Automaton ToPda()
        {
            var alphabet = new List<char>(Terminals);
            var stackAlphabet = new List<char>(Variables.Concat(Terminals));
            var states = new List<string> { "q0", "q1", "q2", "q3" };
            var finalStates = new List<string> { "q3" };
            var transitions = new List<Transition> { 
                new Transition("q0", '_', '_', '$', "q1"),
                new Transition("q1", '_', '_', StartVariable, "q2"),
                new Transition("q2", '_', '$', '_', "q3")
            };

            foreach (var terminal in Terminals)
                transitions.Add(new Transition("q2", terminal, terminal, '_', "q2"));

            var index = 4;

            foreach (var rule in Rules)
            {
                if (rule.RightHandSide.Count == 1)
                {
                    transitions.Add(new Transition("q2", '_', rule.Variable, rule.RightHandSide.First(), "q2"));
                }
                else
                {
                    var startState = "q2";
                    var endState = $"q{index++}";

                    states.Add(endState);
                    transitions.Add(new Transition(startState, '_', rule.Variable, rule.RightHandSide.Last(), endState));

                    for (int i = rule.RightHandSide.Count - 2; i > 0; i--)
                    {
                        startState = endState;
                        endState = $"q{index++}";

                        states.Add(endState);
                        transitions.Add(new Transition(startState, '_', '_', rule.RightHandSide[i], endState));
                    }

                    startState = endState;
                    endState = "q2";

                    transitions.Add(new Transition(startState, '_', '_', rule.RightHandSide.First(), endState));
                }
            }

            return new Automaton(alphabet, stackAlphabet, states, finalStates, transitions);
        }

        public bool Accepts(string word)
        {
            if (_pda == null)
                _pda = ToPda();

            return _pda.Accepts(word);
        }

        public bool IsDfa()
        {
            return false;
        }

        public string ToText()
        {
            throw new System.NotImplementedException();
        }

        public string ToGraph()
        {
            if (_pda == null)
                _pda = ToPda();

            return _pda.ToGraph();
        }
    }
}
