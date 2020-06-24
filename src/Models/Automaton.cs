using System;
using System.Collections.Generic;
using System.Linq;

namespace Automatax.Models
{
    public class Automaton : IAutomaton
    {

        public Automaton(List<char> alphabet, List<string> stackAlphabet, List<string> states, List<string> finalStates, List<Transition> transitions)
        {
            Alphabet = alphabet;
            StackAlphabet = stackAlphabet;
            States = states;
            StartState = States.First();
            FinalStates = finalStates;
            Transitions = transitions;
        }

        public List<char> Alphabet { get; }
        public List<string> StackAlphabet { get; }
        public List<string> States { get; }
        public string StartState { get; }
        public List<string> FinalStates { get; }
        public List<Transition> Transitions { get; }

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

        public bool IsDfa()
        {
            if (Transitions.Exists(t => t.Symbol == '_'))
                return false;

            foreach(var state in States)
                foreach (var symbol in Alphabet)
                    if (!Transitions.Exists(t => t.StartState == state && t.Symbol == symbol)
                        || Transitions.Where(t => t.StartState == state && t.Symbol == symbol).Count() > 1)
                        return false;

            return true;
        }

        public bool Accepts(string word)
        {
            return Accepts(StartState, word, new Stack<string>());
        }

        private bool Accepts(string currentState, string input, Stack<string> currentStack)
        {
            if (input == "" && FinalStates.Contains(currentState) && currentStack.Count == 0)
                return true;

            var transitions = Transitions.FindAll(t =>
                t.StartState == currentState && (
                (input != "" && t.Symbol == input[0] && currentStack.Count > 0 && t.StackPop == currentStack.Peek())
                || (input != "" && t.Symbol == input[0] && t.StackPop == "_")
                || (t.Symbol == '_' && currentStack.Count > 0 && t.StackPop == currentStack.Peek())
                || (t.Symbol == '_' && t.StackPop == "_"))
            );

            foreach (var transition in transitions)
            {
                var stack = new Stack<string>(currentStack.Reverse());

                if (transition.StackPop != "_")
                    stack.Pop();
                if (transition.StackPush != "_")
                    stack.Push(transition.StackPush);
                if (transition.Symbol == '_')
                {
                    if (Accepts(transition.EndState, input, stack))
                        return true;
                }
                else
                {
                    if (Accepts(transition.EndState, input.Substring(1), stack))
                        return true;
                }
            }

            return false;
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
            var variables = new List<string>();
            var terminals = new List<char>(Alphabet);
            var rules = new List<Rule>();

            // Construct the temp pda satisfies the 3 conditions
            var alphabet = new List<char>(Alphabet);
            var stackAlphabet = new List<string>(StackAlphabet);
            var states = new List<string>(States);
            var finalStates = new List<string>();
            var transitions = new List<Transition>();
            var index = 0;

            // Single accepting state
            states.Add("qa");
            finalStates.Add("qa");

            // Empty stack on accepting
            states.Insert(0, "qs");
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
                    transitions.Add(new Transition(transition.StartState, transition.Symbol, transition.StackPop, "_", $"q{index++}'"));
                    transitions.Add(new Transition($"q{index - 1}'", '_', "_", transition.StackPush, transition.EndState));
                }
                else if (transition.StackPop == "_" && transition.StackPush == "_")
                {
                    transitions.Add(new Transition(transition.StartState, transition.Symbol, "_", "a", $"q{index++}'"));
                    transitions.Add(new Transition($"q{index - 1}'", '_', "a", "_", transition.EndState));
                }
                else
                    transitions.Add(transition);
            }

            var sigmaEpsilon = new List<char>(alphabet) { '_' };

            // First Rule
            foreach (var p in states)
                foreach (var q in states)
                    foreach (var r in states)
                        foreach (var s in states)
                            foreach (var alpha in sigmaEpsilon)
                                foreach (var beta in sigmaEpsilon)
                                    foreach (var symbol in stackAlphabet)
                                    {
                                        if (transitions.Any(t => t.StartState == p && t.Symbol == alpha && t.StackPop == "_" && t.StackPush == symbol && t.EndState == r)
                                            && transitions.Any(t => t.StartState == s && t.Symbol == beta && t.StackPop == symbol && t.StackPush == "_" && t.EndState == q))
                                        {
                                            var a_pq = $"A_{p}{q}";
                                            var a_rs = $"A_{r}{s}";

                                            if (!variables.Contains(a_pq))
                                                variables.Add(a_pq);
                                            if (!variables.Contains(a_rs))
                                                variables.Add(a_rs);

                                            rules.Add(new Rule(a_pq, new List<string> { alpha.ToString(), a_rs, beta.ToString() }));
                                        }
                                    }

            // Second Rule
            foreach (var p in states)
                foreach (var q in states)
                    foreach (var r in states)
                    {
                        var a_pq = $"A_{p}{q}";
                        var a_pr = $"A_{p}{r}";
                        var a_rq = $"A_{r}{q}";

                        if (!variables.Contains(a_pq))
                            variables.Add(a_pq);
                        if (!variables.Contains(a_pr))
                            variables.Add(a_pr);
                        if (!variables.Contains(a_rq))
                            variables.Add(a_rq);

                        rules.Add(new Rule(a_pq, new List<string> { a_pr, a_rq }));
                    }

            // Third Rule
            foreach (var p in states)
            {
                var a_pp = $"A_{p}{p}";

                if (!variables.Contains(a_pp))
                    variables.Add(a_pp);

                rules.Add(new Rule(a_pp, new List<string> { "_" }));
            }

            return new Grammar(variables, terminals, rules);
        }
    }
}
