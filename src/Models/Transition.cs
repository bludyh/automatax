using System;

namespace Automatax.Models
{
    public class Transition
    {

        public Transition(string startState, char symbol, char stackPop, char stackPush, string endState)
        {
            StartState = startState;
            Symbol = symbol;
            StackPop = stackPop;
            StackPush = stackPush;
            EndState = endState;
        }

        public string StartState { get; }
        public char Symbol { get; }
        public char StackPop { get; }
        public char StackPush { get; }
        public string EndState { get; }

        public override string ToString()
        {
            if (StackPop == '_' && StackPush == '_')
                return $"{Environment.NewLine}\t\"{StartState}\" -> \"{EndState}\" [label=\"{Helper.ReplaceSpecialSymbol(Symbol)}\"]";

            return $"{Environment.NewLine}\t\"{StartState}\" -> \"{EndState}\" [label=\"{Helper.ReplaceSpecialSymbol(Symbol)} [{Helper.ReplaceSpecialSymbol(StackPop)}/{Helper.ReplaceSpecialSymbol(StackPush)}]\"]";
        }
    }
}
