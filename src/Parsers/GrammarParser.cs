using Automatax.Exceptions;
using Automatax.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Automatax.Parsers
{
    public class GrammarParser : IParser
    {
        public IAutomaton Parse(StreamReader reader)
        {
            var variables = new List<char>();
            var terminals = new List<char>();
            var rules = new List<Rule>();

            string line;
            string fileName = (reader.BaseStream as FileStream).Name;
            int lineNumber = 0;
            while ((line = reader.ReadLine()) != null)
            {
                ++lineNumber;

                if (line == string.Empty || line.StartsWith("#"))
                    continue;

                if (line.Contains("grammar:"))
                {
                    char variable;
                    var rhs = new List<char>();
                    Rule rule;

                    while ((line = reader.ReadLine()) != null)
                    {
                        ++lineNumber;

                        if (line == string.Empty || line.StartsWith("#"))
                            continue;

                        if (line.Contains("end."))
                            break;

                        try
                        {
                            variable = line.Split(':')[0].ToCharArray().Where(c => char.IsUpper(c)).First();

                            if (!variables.Contains(variable))
                                variables.Add(variable);
                        }
                        catch (InvalidOperationException)
                        {
                            throw new InvalidSyntaxException(
                                $"{fileName},{lineNumber}: " +
                                $"Variable must be an uppercase letter.");
                        }

                        rhs = line.Split(':')[1].ToCharArray().Where(c => c != ' ').ToList();

                        terminals.AddRange(rhs.Where(c => c != '_' && !char.IsUpper(c) && !terminals.Contains(c)));

                        rule = new Rule(variable, rhs);
                        rules.Add(rule);
                    }

                    break;
                }
            }

            if (variables.Count == 0)
                throw new InvalidSyntaxException($"{fileName}: Grammar must have variables.");
            if (terminals.Count == 0)
                throw new InvalidSyntaxException($"{fileName}: Grammar must have terminal symbols.");
            if (rules.Count == 0)
                throw new InvalidSyntaxException($"{fileName}: Grammar must have rules.");

            return new Grammar(variables, terminals, rules, variables.First());
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
