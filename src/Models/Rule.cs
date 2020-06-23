using System.Collections.Generic;

namespace Automatax.Models
{
    public class Rule
    {
        public Rule(char variable, List<char> rhs)
        {
            Variable = variable;
            RightHandSide = rhs;
        }

        public char Variable { get; }
        public List<char> RightHandSide { get; }
    }
}
