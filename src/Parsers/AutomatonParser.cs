using Automatax.Exceptions;
using Automatax.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Automatax.Parsers
{
    public class AutomatonParser : IParser
    {
        public IAutomaton Parse(StreamReader reader)
        {
            var alphabet = new HashSet<char>();
            var stackAlphabet = new HashSet<string>();
            var states = new HashSet<string>();
            var startState = string.Empty;
            var finalStates = new HashSet<string>();
            var transitions = new List<Transition>();

            string line;
            string fileName = (reader.BaseStream as FileStream).Name;
            int lineNumber = 0;
            while ((line = reader.ReadLine()) != null)
            {
                ++lineNumber;

                if (line == string.Empty || line.StartsWith("#"))
                    continue;

                if (line.Contains("alphabet:"))
                {
                    alphabet.UnionWith(line.Split(':')[1].ToCharArray().Where(c => c != ' ' && c != ','));
                    continue;
                }

                if (line.Contains("stack:"))
                {
                    stackAlphabet.UnionWith(line.Split(':')[1].Split(',').Select(s => s.Trim()));
                    continue;
                }

                if (line.Contains("states:"))
                {
                    states.UnionWith(line.Split(':')[1].Split(',').Select(s => s.Trim()));
                    startState = states.First();

                    if (states.Any(s => char.TryParse(s, out char c) && (alphabet.Contains(c) || stackAlphabet.Contains(s))))
                        throw new InvalidSyntaxException(
                            $"{fileName},{lineNumber}: " +
                            $"State name cannot be characters that are already in input or stack alphabet.");

                    continue;
                }

                if (line.Contains("final:"))
                {
                    finalStates.UnionWith(line.Split(':')[1].Split(',').Select(s => s.Trim()));

                    if (finalStates.Any(fs => !states.Contains(fs)))
                        throw new InvalidSyntaxException(
                            $"{fileName},{lineNumber}: " +
                            $"Final state must be a valid state.");

                    continue;
                }

                if (line.Contains("transitions:"))
                {
                    string fromState;
                    char symbol;
                    string stackPop;
                    string stackPush;
                    string toState;
                    Transition transition;

                    while ((line = reader.ReadLine()) != null)
                    {
                        ++lineNumber;

                        if (line == string.Empty || line.StartsWith("#"))
                            continue;

                        if (line.Contains("end."))
                            break;

                        fromState = line.Split(',')[0].Trim();

                        if (!states.Contains(fromState))
                            throw new InvalidSyntaxException(
                                $"{fileName},{lineNumber}: " +
                                $"Transition must have a valid start state.");

                        symbol = line.Split(',')[1].Trim().ToCharArray().First();

                        if (symbol != '_' && !alphabet.Contains(symbol))
                            throw new InvalidSyntaxException(
                                $"{fileName},{lineNumber}: " +
                                $"Transition must have a valid input symbol.");

                        if (line.Contains('[') && line.Contains(']'))
                        {
                            stackPop = line.Split('[')[1].Split(',')[0].Trim();

                            if (stackPop != "_" && stackPop != "$" && !stackAlphabet.Contains(stackPop))
                                throw new InvalidSyntaxException(
                                    $"{fileName},{lineNumber}: " +
                                    $"Pop symbol must be a valid character from stack alphabet.");

                            stackPush = line.Split(']')[0].Split(',').Last().Trim();

                            if (stackPush != "_" && stackPush != "$" & !stackAlphabet.Contains(stackPush))
                                throw new InvalidSyntaxException(
                                    $"{fileName},{lineNumber}: " +
                                    $"Push symbol must be a valid character from stack alphabet.");
                        }
                        else
                        {
                            stackPop = "_";
                            stackPush = "_";
                        }

                        toState = line.Split(new string[] { "-->" }, System.StringSplitOptions.None).Last().Trim();

                        if (!states.Contains(toState))
                            throw new InvalidSyntaxException(
                                $"{fileName},{lineNumber}: " +
                                $"Transition must have a valid end state.");

                        transition = new Transition(fromState, symbol, stackPop, stackPush, toState);
                        transitions.Add(transition);
                    }

                    break;
                }

            }

            if (alphabet.Count == 0)
                throw new InvalidSyntaxException($"{fileName}: Automaton must have an alphabet.");
            if (states.Count == 0)
                throw new InvalidSyntaxException($"{fileName}: Automaton must have a list of states.");

            return new Automaton(alphabet, stackAlphabet, states, startState, finalStates, transitions);
        }

        public TestVector ParseTests(StreamReader reader)
        {
            TestVector testVector = new TestVector();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == string.Empty || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("dfa"))
                {
                    testVector.IsDfa = Helper.StringToBool(line.Split(':')[1].Trim());
                    continue;
                }

                if (line.StartsWith("words"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();

                        if (line == string.Empty || line.StartsWith("#"))
                            continue;

                        if (line.Contains("end."))
                            break;

                        testVector.Words.Add(line.Split(',')[0].Trim(), Helper.StringToBool(line.Split(',')[1].Trim()));
                    }
                    
                    break;
                }
            }

            return testVector;
        }

    }
}
