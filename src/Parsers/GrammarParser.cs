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
            var variables = new HashSet<string>();
            var terminals = new HashSet<char>();
            var rules = new List<Rule>();
            var startVariable = string.Empty;

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
                    string variable;
                    var rhs = new List<string>();
                    Rule rule;

                    while ((line = reader.ReadLine()) != null)
                    {
                        ++lineNumber;

                        if (line == string.Empty || line.StartsWith("#"))
                            continue;

                        if (line.Contains("end."))
                            break;

                        variable = line.Split(':')[0].Trim();
                        
                        if (variables.Count == 0)
                            startVariable = variable;

                        variables.Add(variable);

                        rhs = line.Split(':')[1].Split(' ').Where(i => i != string.Empty).Select(i => i.Trim()).ToList();

                        terminals.UnionWith(rhs.Where(i => char.TryParse(i, out var c) && c != '_' && !char.IsUpper(c)).Select(i => char.Parse(i)));

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

            return new Grammar(variables, terminals, rules, startVariable);
            // FOR TESTING ONLY
            //var grammar = new Grammar(variables, terminals, rules, startVariable);
            //grammar.Simplify();
            //return grammar;
            // FOR TESTING ONLY
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
