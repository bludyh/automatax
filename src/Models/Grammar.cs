using System;
using System.Collections.Generic;
using System.Linq;

namespace Automatax.Models
{
    public class Grammar : IAutomaton
    {

        private Automaton _pda;

        public Grammar(HashSet<string> variables, HashSet<char> terminals, List<Rule> rules, string startVariable)
        {
            Variables = variables;
            Terminals = terminals;
            Rules = rules;
            StartVariable = startVariable;
        }

        public HashSet<string> Variables { get; }
        public HashSet<char> Terminals { get; }
        public List<Rule> Rules { get; }
        public string StartVariable { get; }

        public bool IsDfa()
        {
            return false;
        }

        public bool Accepts(string word)
        {
            if (_pda == null)
                _pda = ToPda();

            return _pda.Accepts(word);
        }

        public string ToGraph()
        {
            if (_pda == null)
                _pda = ToPda();

            return _pda.ToGraph();
        }

        public string ToText()
        {
            string rules = null;

            foreach (var rule in Rules)
                rules += rule.ToText();

            return $"grammar:"
                + rules
                + $"{Environment.NewLine}end.";
        }

        public Automaton ToPda()
        {
            var alphabet = new HashSet<char>(Terminals);
            var stackAlphabet = new HashSet<string>(Variables.Union(Terminals.Select(c => c.ToString())));
            var states = new HashSet<string> { "q0", "q1", "q2", "q3" };
            var startState = "q0";
            var finalStates = new HashSet<string> { "q3" };
            var transitions = new List<Transition> {
                new Transition("q0", '_', "_", "$", "q1"),
                new Transition("q1", '_', "_", StartVariable, "q2"),
                new Transition("q2", '_', "$", "_", "q3")
            };

            foreach (var terminal in Terminals)
                transitions.Add(new Transition("q2", terminal, terminal.ToString(), "_", "q2"));

            var index = 4;

            foreach (var rule in Rules)
            {
                if (rule.RightHandSide.Count == 1)
                {
                    transitions.Add(new Transition("q2", '_', rule.Variable, rule.RightHandSide.First(), "q2"));
                }
                else
                {
                    var fromState = "q2";
                    var toState = $"q{index++}";

                    states.Add(toState);
                    transitions.Add(new Transition(fromState, '_', rule.Variable, rule.RightHandSide.Last(), toState));

                    for (int i = rule.RightHandSide.Count - 2; i > 0; i--)
                    {
                        fromState = toState;
                        toState = $"q{index++}";

                        states.Add(toState);
                        transitions.Add(new Transition(fromState, '_', "_", rule.RightHandSide[i], toState));
                    }

                    fromState = toState;
                    toState = "q2";

                    transitions.Add(new Transition(fromState, '_', "_", rule.RightHandSide.First(), toState));
                }
            }

            return new Automaton(alphabet, stackAlphabet, states, startState, finalStates, transitions);
        }

        public void Simplify()
        {
            RemoveNonGeneratingProductions();
            RemoveUnreachableProductions();
        }

        private void RemoveNonGeneratingProductions()
        {
            var generatingVariables = new HashSet<string>();

            bool newGeneratingVariablesAdded;

            do
            {
                newGeneratingVariablesAdded = false;

                var newGeneratingVariables = Rules
                    .Where(r => r.RightHandSide.All(i => i == "_" || (char.TryParse(i, out var c) && Terminals.Contains(c)) || generatingVariables.Contains(i)))
                    .Select(r => r.Variable)
                    .Where(v => !generatingVariables.Contains(v));

                if (newGeneratingVariables.Count() > 0)
                {
                    generatingVariables.UnionWith(newGeneratingVariables);
                    newGeneratingVariablesAdded = true;
                }
            }
            while (newGeneratingVariablesAdded);

            var nonGeneratingVariables = Variables.Except(generatingVariables);

            Rules.RemoveAll(r => nonGeneratingVariables.Contains(r.Variable) || r.RightHandSide.Any(i => nonGeneratingVariables.Contains(i)));
        }

        private void RemoveUnreachableProductions()
        {
            var reachableVariables = new HashSet<string> { StartVariable };

            bool newReachableVariablesAdded;

            do
            {
                newReachableVariablesAdded = false;

                var newReachableVariables = Rules
                    .Where(r => reachableVariables.Contains(r.Variable))
                    .SelectMany(r => r.RightHandSide.Where(i => Variables.Contains(i)))
                    .Where(v => !reachableVariables.Contains(v));

                if (newReachableVariables.Count() > 0)
                {
                    reachableVariables.UnionWith(newReachableVariables);
                    newReachableVariablesAdded = true;
                }
            }
            while (newReachableVariablesAdded);

            var unreachableVariables = Variables.Except(reachableVariables);

            Rules.RemoveAll(r => unreachableVariables.Contains(r.Variable) || r.RightHandSide.Any(i => unreachableVariables.Contains(i)));

            Variables.ExceptWith(unreachableVariables.ToList());
        }
    }
}
