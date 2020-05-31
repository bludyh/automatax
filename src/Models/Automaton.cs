using System;
using System.Collections.Generic;
using System.Linq;

namespace Automatax.Models
{
    public class Automaton
    {

        public Automaton(List<char> alphabet, List<string> states, List<string> finalStates, List<Transition> transitions)
        {
            Alphabet = alphabet;
            States = states;
            StartState = States.First();
            FinalStates = finalStates;
            Transitions = transitions;
        }

        public List<char> Alphabet { get; }
        public List<string> States { get; }
        public string StartState { get; }
        public List<string> FinalStates { get; }
        public List<Transition> Transitions { get; }
        public TestVector TestVector { get; set; }

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
    }
}
