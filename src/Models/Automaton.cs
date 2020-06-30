using System;
using System.Collections.Generic;
using System.Linq;

namespace Automatax.Models
{
    public class Automaton : IAutomaton
    {

        public Automaton(HashSet<char> alphabet, HashSet<string> stackAlphabet, HashSet<string> states, string startState, HashSet<string> finalStates, List<Transition> transitions)
        {
            Alphabet = alphabet;
            StackAlphabet = stackAlphabet;
            States = states;
            StartState = startState;
            FinalStates = finalStates;
            Transitions = transitions;
        }

        public HashSet<char> Alphabet { get; }
        public HashSet<string> StackAlphabet { get; }
        public HashSet<string> States { get; }
        public string StartState { get; }
        public HashSet<string> FinalStates { get; }
        public List<Transition> Transitions { get; }

        public bool IsDfa()
        {
            if (Transitions.Exists(t => t.Symbol == '_'))
                return false;

            foreach (var state in States)
                foreach (var symbol in Alphabet)
                    if (!Transitions.Exists(t => t.FromState == state && t.Symbol == symbol)
                        || Transitions.Where(t => t.FromState == state && t.Symbol == symbol).Count() > 1)
                        return false;

            return true;
        }

        public bool Accepts(string word)
        {
            var stateConfigurations = new List<StateConfiguration> { new StateConfiguration(word, StartState, new Stack<string>()) };
            var count = 0;

            return Bfs(stateConfigurations, ref count);
        }

        private bool Bfs(List<StateConfiguration> stateConfigurations, ref int count)
        {
            if (++count > 50)
                throw new Exception("Cannot determine acceptance. Reached recursion limit of 50 steps.");

            if (stateConfigurations.Any(sc => sc.CurrentInput == string.Empty && FinalStates.Contains(sc.State) && sc.Stack.Count == 0))
                return true;

            var nextStateConfigurations = new List<StateConfiguration>();

            foreach (var config in stateConfigurations)
            {
                var transitions = Transitions.Where(t =>
                    t.FromState == config.State && (
                    (config.CurrentInput != string.Empty && t.Symbol == config.CurrentInput[0] && config.Stack.Count > 0 && t.StackPop == config.Stack.Peek())
                    || (config.CurrentInput != string.Empty && t.Symbol == config.CurrentInput[0] && t.StackPop == "_")
                    || (t.Symbol == '_' && config.Stack.Count > 0 && t.StackPop == config.Stack.Peek())
                    || (t.Symbol == '_' && t.StackPop == "_"))
                );

                foreach (var transition in transitions)
                {
                    string input = null;
                    var stack = new Stack<string>(config.Stack.Reverse());

                    if (transition.StackPop != "_")
                        stack.Pop();

                    if (transition.StackPush != "_")
                        stack.Push(transition.StackPush);

                    if (transition.Symbol == '_')
                        input = config.CurrentInput;
                    else
                        input = config.CurrentInput.Substring(1);

                    nextStateConfigurations.Add(new StateConfiguration(input, transition.ToState, stack));
                }
            }

            if (nextStateConfigurations.Count == 0)
                return false;

            return Bfs(nextStateConfigurations, ref count);
        }

        public string ToGraph()
        {
            string states = $"{Environment.NewLine}\t\"\" [shape=none]";

            foreach (var state in States)
            {
                states += $"{Environment.NewLine}\t\"{state}\"";
                if (FinalStates.Contains(state))
                    states += $" [shape=doublecircle]";
                else
                    states += $" [shape=circle]";
            }

            string transitions = $"{Environment.NewLine}\t\"\" -> \"{StartState}\"";

            foreach (var transition in Transitions)
                transitions += transition.ToGraph();

            string result = $"digraph automaton {{{Environment.NewLine}\trankdir=LR;{states}{Environment.NewLine}{transitions}{Environment.NewLine}}}";
            return result;
        }

        public string ToText()
        {
            string transitions = null;

            foreach (var transition in Transitions)
                transitions += transition.ToText();

            return $"alphabet: {string.Join(",", Alphabet)}"
                + ((StackAlphabet.Count > 0) ? $"{Environment.NewLine}stack: {string.Join(",", StackAlphabet)}" : string.Empty)
                + $"{Environment.NewLine}states: {string.Join(",", States)}"
                + $"{Environment.NewLine}final: {string.Join(",", FinalStates)}"
                + $"{Environment.NewLine}transitions:"
                + transitions
                + $"{Environment.NewLine}end.";
        }

        public Grammar ToGrammar()
        {
            var startVariable = "S";
            var variables = new HashSet<string> { startVariable };
            var terminals = new HashSet<char>(Alphabet);
            var rules = new List<Rule> { new Rule(startVariable, new List<string> { $"A_{States.First()}{States.Last()}" }) };

            var pda = NormalizePda();

            var sigmaEpsilon = new HashSet<char>(pda.Alphabet) { '_' };

            // First Rule
            foreach (var p in pda.States)
                foreach (var q in pda.States)
                    foreach (var r in pda.States)
                        foreach (var s in pda.States)
                            foreach (var alpha in sigmaEpsilon)
                                foreach (var beta in sigmaEpsilon)
                                    foreach (var symbol in pda.StackAlphabet)
                                    {
                                        if (pda.Transitions.Any(t => t.FromState == p && t.Symbol == alpha && t.StackPop == "_" && t.StackPush == symbol && t.ToState == r)
                                            && pda.Transitions.Any(t => t.FromState == s && t.Symbol == beta && t.StackPop == symbol && t.StackPush == "_" && t.ToState == q))
                                        {
                                            var a_pq = $"A_{p}{q}";
                                            variables.Add(a_pq);

                                            var a_rs = $"A_{r}{s}";
                                            variables.Add(a_rs);

                                            rules.Add(new Rule(a_pq, new List<string> { alpha.ToString(), a_rs, beta.ToString() }.Where(i => i != "_").ToList()));
                                        }
                                    }

            // Second Rule
            foreach (var p in pda.States)
                foreach (var q in pda.States)
                    foreach (var r in pda.States)
                    {
                        if (p != q && q != r && r != p)
                        {

                            var a_pq = $"A_{p}{q}";
                            variables.Add(a_pq);

                            var a_pr = $"A_{p}{r}";
                            variables.Add(a_pr);

                            var a_rq = $"A_{r}{q}";
                            variables.Add(a_rq);

                            rules.Add(new Rule(a_pq, new List<string> { a_pr, a_rq }));
                        }
                    }

            // Third Rule
            foreach (var p in pda.States)
            {
                var a_pp = $"A_{p}{p}";

                variables.Add(a_pp);

                rules.Add(new Rule(a_pp, new List<string> { "_" }));
            }

            var grammar = new Grammar(variables, terminals, rules, startVariable);

            // Simplify
            grammar.Simplify();

            return grammar;
        }

        private Automaton NormalizePda()
        {
            var alphabet = new HashSet<char>(Alphabet);
            var stackAlphabet = new HashSet<string>(StackAlphabet);
            var states = new HashSet<string>(States);
            var finalStates = new HashSet<string>();
            var transitions = new List<Transition>();
            var index = 0;

            // Single accepting state
            states.Add("qa");
            finalStates.Add("qa");

            // Empty stack on accepting
            string startState = "qs";
            states.Add(startState);
            transitions.Add(new Transition("qs", '_', "_", "$", StartState));

            states.Add("qt");
            foreach (var state in FinalStates)
            {
                transitions.Add(new Transition(state, '_', "_", "a", $"q{index++}'"));
                transitions.Add(new Transition($"q{index - 1}'", '_', "a", "_", "qt"));
            }

            foreach (var symbol in stackAlphabet)
                transitions.Add(new Transition("qt", '_', symbol, "_", "qt"));

            transitions.Add(new Transition("qt", '_', "$", "_", "qa"));

            // Each transition is a push or pop but not both
            foreach (var transition in Transitions)
            {
                if (transition.StackPop != "_" && transition.StackPush != "_")
                {
                    transitions.Add(new Transition(transition.FromState, transition.Symbol, transition.StackPop, "_", $"q{index++}'"));
                    transitions.Add(new Transition($"q{index - 1}'", '_', "_", transition.StackPush, transition.ToState));
                }
                else if (transition.StackPop == "_" && transition.StackPush == "_")
                {
                    transitions.Add(new Transition(transition.FromState, transition.Symbol, "_", "a", $"q{index++}'"));
                    transitions.Add(new Transition($"q{index - 1}'", '_', "a", "_", transition.ToState));
                }
                else
                    transitions.Add(transition);
            }

            return new Automaton(alphabet, stackAlphabet, states, startState, finalStates, transitions);
        }

    }
}
