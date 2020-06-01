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
    }
}
