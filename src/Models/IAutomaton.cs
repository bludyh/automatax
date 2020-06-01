namespace Automatax.Models
{
    public interface IAutomaton
    {
        TestVector TestVector { get; }

        bool IsDfa();
        bool Accepts(string word);
    }
}
