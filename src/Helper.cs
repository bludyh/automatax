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

        public static char ReplaceSpecialSymbol(char symbol)
        {
            if (symbol == '_')
                symbol = 'ε';

            return symbol;
        }
    }
}
