using Automatax.Models;
using System.IO;

namespace Automatax.Parsers
{
    public interface IParser
    {
        IAutomaton Parse(StreamReader reader);
    }
}
