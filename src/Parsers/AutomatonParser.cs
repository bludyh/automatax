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
            List<char> alphabet = new List<char>();
            List<char> stackAlphabet = new List<char>();
            List<string> states = new List<string>();
            List<string> finalStates = new List<string>();
            List<Transition> transitions = new List<Transition>();

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
                    alphabet = line.Split(':')[1].ToCharArray().Where(c => char.IsLetterOrDigit(c)).ToList();
                    continue;
                }

                if (line.Contains("stack:"))
                {
                    stackAlphabet = line.Split(':')[1].ToCharArray().Where(c => char.IsLetterOrDigit(c)).ToList();
                    continue;
                }

                if (line.Contains("states:"))
                {
                    states = line.Split(':')[1].Split(',').Select(s => s.Trim()).ToList();

                    if (states.Any(s => char.TryParse(s, out char c) && (alphabet.Contains(c) || stackAlphabet.Contains(c))))
                        throw new InvalidSyntaxException(
                            $"{fileName},{lineNumber}: " +
                            $"State name cannot be characters that are already in input or stack alphabet.");

                    continue;
                }

                if (line.Contains("final:"))
                {
                    finalStates = line.Split(':')[1].Split(',').Select(s => s.Trim()).ToList();

                    if (finalStates.Any(fs => !states.Contains(fs)))
                        throw new InvalidSyntaxException(
                            $"{fileName},{lineNumber}: " +
                            $"Final state must be a valid state.");

                    continue;
                }

                if (line.Contains("transitions:"))
                {
                    string startState;
                    char symbol;
                    char stackPop;
                    char stackPush;
                    string endState;
                    Transition transition;

                    while ((line = reader.ReadLine()) != null)
                    {
                        ++lineNumber;

                        if (line == string.Empty || line.StartsWith("#"))
                            continue;

                        if (line.Contains("end."))
                            break;

                        startState = line.Split(',')[0].Trim();

                        if (!states.Contains(startState))
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
                            stackPop = line.Split('[')[1].Trim().ToCharArray().First();

                            if (stackPop != '_' && !stackAlphabet.Contains(stackPop))
                                throw new InvalidSyntaxException(
                                    $"{fileName},{lineNumber}: " +
                                    $"Pop symbol must be a valid character from stack alphabet.");

                            stackPush = line.Split(']')[0].Trim().ToCharArray().Last();

                            if (stackPush != '_' && !stackAlphabet.Contains(stackPush))
                                throw new InvalidSyntaxException(
                                    $"{fileName},{lineNumber}: " +
                                    $"Push symbol must be a valid character from stack alphabet.");
                        }
                        else
                        {
                            stackPop = '_';
                            stackPush = '_';
                        }

                        endState = line.Split(new string[] { "-->" }, System.StringSplitOptions.None).Last().Trim();

                        if (!states.Contains(endState))
                            throw new InvalidSyntaxException(
                                $"{fileName},{lineNumber}: " +
                                $"Transition must have a valid end state.");

                        transition = new Transition(startState, symbol, stackPop, stackPush, endState);
                        transitions.Add(transition);
                    }

                    break;
                }

            }

            if (alphabet.Count == 0)
                throw new InvalidSyntaxException($"{fileName}: Automaton must have an alphabet.");
            if (states.Count == 0)
                throw new InvalidSyntaxException($"{fileName}: Automaton must have a list of states.");

            return new Automaton(alphabet, states, finalStates, transitions);
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
