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
            Stack = new Stack<char>();
        }

        public List<char> Alphabet { get; }
        public List<string> States { get; }
        public string StartState { get; }
        public List<string> FinalStates { get; }
        public List<Transition> Transitions { get; }
        public Stack<char> Stack { get; }

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

        public bool Accepts(string word)
        {
            var result = Accepts(StartState, word);

            Stack.Clear();

            return result;
        }

        private bool Accepts(string currentState, string input)
        {
            var transitions = Transitions.FindAll(t =>
                t.StartState == currentState && (
                (input != "" && t.Symbol == input[0] && Stack.Count > 0 && t.StackPop == Stack.Peek())
                || (input != "" && t.Symbol == input[0] && t.StackPop == '_')
                || (t.Symbol == '_' && Stack.Count > 0 && t.StackPop == Stack.Peek())
                || (t.Symbol == '_' && t.StackPop == '_'))
            );

            foreach (var transition in transitions)
            {
                if (transition.StackPop != '_')
                    Stack.Pop();
                if (transition.StackPush != '_')
                    Stack.Push(transition.StackPush);
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

            if (input == "" && FinalStates.Contains(currentState) && Stack.Count == 0)
                return true;

            return false;
        }
    }
}
