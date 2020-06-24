using System;
using System.Collections.Generic;

namespace Automatax.Models
{
    public class Rule
    {
        public Rule(string variable, List<string> rhs)
        {
            Variable = variable;
            RightHandSide = rhs;
        }

        public string Variable { get; }
        public List<string> RightHandSide { get; }

        public string ToText()
        {
            return $"{Environment.NewLine}{Variable}:{string.Join(" ", RightHandSide)}";
        }
    }
}
