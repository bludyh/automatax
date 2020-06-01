using Automatax.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Automatax.Parsers
{
    public class FiniteAutomatonParser : IParser
    {
        public IAutomaton Parse(StreamReader reader)
        {
            IAutomaton automaton = null;
            List<char> alphabet = new List<char>();
            List<string> states = new List<string>();
            List<string> finalStates = new List<string>();
            List<Transition> transitions = new List<Transition>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();

                if (line == string.Empty || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("alphabet"))
                {
                    alphabet = line.Split(':').Last().ToCharArray().Where(c => char.IsLetterOrDigit(c)).ToList();
                    continue;
                }

                if (line.StartsWith("states"))
                {
                    states = line.Split(':').Last().Trim().Split(',').ToList();
                    continue;
                }

                if (line.StartsWith("final"))
                {
                    finalStates = line.Split(':').Last().Trim().Split(',').ToList();
                    continue;
                }

                if (line.StartsWith("transitions"))
                {
                    string startState;
                    char symbol;
                    string endState;
                    Transition transition;
                    while ((line = reader.ReadLine()) != null && !line.Contains("end"))
                    {
                        line = line.Trim();

                        if (line == string.Empty || line.StartsWith("#"))
                            continue;

                        startState = line.Split(',').First();
                        symbol = line.Split(',').Last().ToCharArray().First();
                        endState = line.Split(new string[] { "-->" }, System.StringSplitOptions.None).Last().Trim();
                        transition = new Transition(startState, symbol, endState);
                        transitions.Add(transition);
                    }
                    continue;
                }

                if (automaton == null)
                    automaton = new Automaton(alphabet, states, finalStates, transitions);

                if (line.StartsWith("dfa"))
                {
                    automaton.TestVector.IsDfa = Helper.StringToBool(line.Split(':').Last().Trim());
                    continue;
                }

                if (line.StartsWith("words"))
                {
                    while ((line = reader.ReadLine()) != null && !line.Contains("end"))
                    {
                        line = line.Trim();

                        if (line == string.Empty || line.StartsWith("#"))
                            continue;

                        automaton.TestVector.Words.Add(line.Split(',').First(), Helper.StringToBool(line.Split(',').Last()));
                    }
                    continue;
                }
            }

            return automaton;
        }

    }
}
