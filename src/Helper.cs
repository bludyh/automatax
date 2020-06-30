using System;
using System.Collections.Generic;
using System.Linq;

namespace Automatax
{
    public static class Helper
    {
        public static bool? StringToBool(string input)
        {
            if (input == "y") return true;
            else if (input == "n") return false;

            return null;
        }

        public static string ReplaceSpecialSymbol(string input)
        {
            if (input == "_")
                input = "ε";

            return input;
        }

        public static List<List<T>> GetCombinations<T>(List<T> list)
        {
            int comboCount = (int)Math.Pow(2, list.Count) - 1;

            List<List<T>> result = new List<List<T>>();

            for (int i = 1; i < comboCount + 1; i++)
            {
                result.Add(new List<T>());

                for (int j = 0; j < list.Count; j++)
                {
                    if ((i >> j) % 2 != 0)
                        result.Last().Add(list[j]);
                }
            }
            return result;
        }
    }
}
