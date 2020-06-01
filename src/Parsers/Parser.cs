using Automatax.Models;
using Automatax.Parsers;
using System.IO;

namespace Automatax
{
    public class Parser : IParser
    {

        public IAutomaton Parse(StreamReader reader)
        {
            IParser innerParser;
            string content = reader.ReadToEnd();

            if (content.Contains("stack"))
                innerParser = new PushDownAutomatonParser();
            else if (content.Contains("grammar"))
                innerParser = new GrammarParser();
            else
                innerParser = new FiniteAutomatonParser();

            reader.BaseStream.Position = 0;
            reader.DiscardBufferedData();

            return innerParser.Parse(reader);
        }

    }
}
