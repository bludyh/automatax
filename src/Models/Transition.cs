using System;

namespace Automatax.Models
{
    public class Transition
    {

        public Transition(string startState, char symbol, string endState)
        {
            StartState = startState;
            Symbol = symbol;
            EndState = endState;
        }

        public string StartState { get; }
        public char Symbol { get; }
        public string EndState { get; }

        public override string ToString()
        {
            return $"{Environment.NewLine}\t\"{StartState}\" -> \"{EndState}\" [label=\"{(Symbol == '_' ? 'ε' : Symbol)}\"]";
        }
    }
}
