using System;
using System.Collections.Generic;
using System.Linq;

namespace Automatax.Models
{
    public class Automaton : IAutomaton
    {

        public Automaton(List<char> alphabet, List<char> stackAlphabet, List<string> states, List<string> finalStates, List<Transition> transitions)
        {
            Alphabet = alphabet;
            StackAlphabet = stackAlphabet;
            States = states;
            StartState = States.First();
            FinalStates = finalStates;
            Transitions = transitions;
        }

        public List<char> Alphabet { get; }
        public List<char> StackAlphabet { get; }
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
            return Accepts(StartState, word, new Stack<char>());
        }

        private bool Accepts(string currentState, string input, Stack<char> currentStack)
        {
            if (input == "" && FinalStates.Contains(currentState) && currentStack.Count == 0)
                return true;

            var transitions = Transitions.FindAll(t =>
                t.StartState == currentState && (
                (input != "" && t.Symbol == input[0] && currentStack.Count > 0 && t.StackPop == currentStack.Peek())
                || (input != "" && t.Symbol == input[0] && t.StackPop == '_')
                || (t.Symbol == '_' && currentStack.Count > 0 && t.StackPop == currentStack.Peek())
                || (t.Symbol == '_' && t.StackPop == '_'))
            );

            foreach (var transition in transitions)
            {
                var stack = new Stack<char>(currentStack.Reverse());

                if (transition.StackPop != '_')
                    stack.Pop();
                if (transition.StackPush != '_')
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
    }
}
