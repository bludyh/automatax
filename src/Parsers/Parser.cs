using Automatax.Models;
using Automatax.Parsers;
using System.IO;

namespace Automatax
{
    public class Parser : IParser
    {

        private IParser _innerParser;

        public IAutomaton Parse(StreamReader reader)
        {
            string content = reader.ReadToEnd();

            if (content.Contains("grammar"))
                _innerParser = new GrammarParser();
            else
                _innerParser = new AutomatonParser();

            reader.BaseStream.Position = 0;
            reader.DiscardBufferedData();

            return _innerParser.Parse(reader);
        }

        public TestVector ParseTests(StreamReader reader)
        {
            return _innerParser.ParseTests(reader);
        }

    }
}
