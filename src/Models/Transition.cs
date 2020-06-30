using System;

namespace Automatax.Models
{
    public class Transition
    {

        public Transition(string fromState, char symbol, string stackPop, string stackPush, string toState)
        {
            FromState = fromState;
            Symbol = symbol;
            StackPop = stackPop;
            StackPush = stackPush;
            ToState = toState;
        }

        public string FromState { get; }
        public char Symbol { get; }
        public string StackPop { get; }
        public string StackPush { get; }
        public string ToState { get; }

        public string ToGraph()
        {
            if (StackPop == "_" && StackPush == "_")
                return $"{Environment.NewLine}\t\"{FromState}\" -> \"{ToState}\" [label=\"{Helper.ReplaceSpecialSymbol(Symbol.ToString())}\"]";

            return $"{Environment.NewLine}\t\"{FromState}\" -> \"{ToState}\" [label=\"{Helper.ReplaceSpecialSymbol(Symbol.ToString())} [{Helper.ReplaceSpecialSymbol(StackPop)}/{Helper.ReplaceSpecialSymbol(StackPush)}]\"]";
        }

        public string ToText()
        {
            if (StackPop == "_" && StackPush == "_")
                return $"{Environment.NewLine}{FromState},{Symbol} --> {ToState}";

            return $"{Environment.NewLine}{FromState},{Symbol} [{StackPop},{StackPush}] --> {ToState}";
        }
    }
}
