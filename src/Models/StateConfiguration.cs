using System.Collections.Generic;

namespace Automatax.Models
{
    public class StateConfiguration
    {

        public StateConfiguration(string currentInput, string state, Stack<string> stack)
        {
            CurrentInput = currentInput;
            State = state;
            Stack = stack;
        }

        public string CurrentInput { get; }
        public string State { get; }
        public Stack<string> Stack { get; }

    }
}
