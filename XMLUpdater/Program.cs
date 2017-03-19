using CommandLine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace XMLUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            if (!Parser.Default.ParseArgumentsStrict(args, options))
            {
                Console.WriteLine("XMLUpdater failed to parse arguments.");
                return;
            };

            // Take in file path, Validate it.
            if(!File.Exists(options.FilePath))
            {
                Console.WriteLine($"File \"{options.FilePath}\" does not exist");
                return;
            }

            // Take in XPath, Validate XPath results.
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(options.FilePath);
            }
            catch(Exception e)
            {
                // TODO: Go through the possible exceptions more specifically
                Console.WriteLine($"Exception occured when trying to read document \"{options.FilePath}\", {Environment.NewLine}{e.Data}");
                return;
            }

            XPathNavigator nav = doc.CreateNavigator();
            XPathNodeIterator iterator = null;
            try
            {
                iterator = nav.Select(options.XPath);
            }
            catch (Exception e) when (e is ArgumentException || e is XPathException)
            {
                if (e is ArgumentException)
                {
                    Console.WriteLine("XPath argument is not valid.");
                    return;
                }
                else if (e is XPathException)
                {
                    Console.WriteLine($"Error occured while processing XPath expression, {e.Data}");
                    return;
                }
            }
            iterator.MoveNext();
            string value = iterator.Current.Value;
            if (string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine("The XPath did not yield any results");
                return;
            }

            string newValue = null;
            
            if(string.IsNullOrWhiteSpace(options.Text))
            {
                Console.WriteLine("Text is empty");
                return;
            }

            if (string.IsNullOrWhiteSpace(options.Regex))
            {
                // Replace whole value, rather than a regex selection of that value.
                newValue = options.Text;
            }
            else
            {
                // Replace value based on a regex selection
                Regex regex = null;
                Match match = null;
                try
                {
                    regex = new Regex(options.Regex);
                }
                catch(ArgumentException e)
                {
                    Console.WriteLine($"Regex argument {options.Regex} is invalid. {e.Data}");
                    return;
                }
                
                try
                {
                     match = regex.Match(value);
                }
                catch(RegexMatchTimeoutException)
                {
                    Console.WriteLine("Regex match timed out");
                    return;
                }

                // Determine selection, and replace it with a new value.
                Group selection;
                if (options.Group == 0)
                {
                    Console.WriteLine("Zero is not valid option for \"group\" argument");
                    return;
                }
                else if(options.Group == -1)
                    selection = match;
                else
                    selection = match.Groups[options.Group];

                string firstPart = value.Substring(0, selection.Index);
                string secondPart = value.Substring(selection.Index + selection.Length);
                newValue = firstPart + options.Text + secondPart;
            }

            Console.WriteLine($"Updating {value} to {newValue}");
            iterator.Current.SetValue(newValue);
            
            try
            {
                doc.Save(options.FilePath);
            }
            catch(XmlException e)
            {
                Console.WriteLine($"Unable to save changes to the xml document. Error: {e.Data}");
            }
        }
    }
}
