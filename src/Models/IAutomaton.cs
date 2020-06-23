namespace Automatax.Models
{
    public interface IAutomaton
    {
        bool IsDfa();
        bool Accepts(string word);
        string ToGraph();
        string ToText();
    }
}
