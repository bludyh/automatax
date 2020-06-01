using System;
using System.Collections.Generic;
using System.Linq;

namespace Automatax.Models
{
    public class Automaton : IAutomaton
    {

        public Automaton(List<char> alphabet, List<string> states, List<string> finalStates, List<Transition> transitions)
        {
            Alphabet = alphabet;
            States = states;
            StartState = States.First();
            FinalStates = finalStates;
            Transitions = transitions;
            TestVector = new TestVector();
        }

        public List<char> Alphabet { get; }
        public List<string> States { get; }
        public string StartState { get; }
        public List<string> FinalStates { get; }
        public List<Transition> Transitions { get; }
        public TestVector TestVector { get; }

        public override string ToString()
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
                transitions += transition;

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

        public virtual bool Accepts(string word)
        {
            return Accepts(StartState, word);
        }

        private bool Accepts(string currentState, string input)
        {
            if (input.Length > 0)
            {
                var transitions = Transitions.FindAll(t => t.StartState == currentState && (t.Symbol == input[0] || t.Symbol == '_'));

                foreach (var transition in transitions)
                {
                    if (transition.Symbol == '_')
                    {
                        if (Accepts(transition.EndState, input))
                            return true;
                    }
                    else
                    {
                        if (Accepts(transition.EndState, input.Substring(1)))
                            return true;
                    }

                }
                return false;
            }

            if (FinalStates.Contains(currentState))
                return true;

            return false;
        }
    }
}
