using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLUpdater
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "The XML file to modify.")]
        public string FilePath { get; set;}

        [Option('x', "xpath", Required = true, HelpText = "The XPath to select.")]
        public string XPath { get; set; }

        [Option('r', "regex", Required = false, HelpText = "The regex to select from the xpath value.")]
        public string Regex { get; set; }

        [Option('g', "group", DefaultValue = 1, Required = false, HelpText = "The group number of the of the regex to replace, or -1 to replace the entire match. Not zero based")]
        public int Group { get; set; }

        [Option('t', "text", Required = true, HelpText = "The text to replace the selected xpath value with.")]
        public string Text { get; set; }

        [Option('v', "verbose", DefaultValue = true, Required = false, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"FilePath: { FilePath }");
            sb.AppendLine($"XPath: { XPath }");
            sb.AppendLine($"Regex: { Regex }");
            sb.AppendLine($"Group: { Group }");
            sb.AppendLine($"Text: { Text }");
            sb.AppendLine($"Verbose: { Verbose }");

            return sb.ToString();
        }
    }
}
