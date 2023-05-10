using System;
using System.Collections.Generic;
using System.IO;
using OSIsoft.AF.PI;

namespace TagValidator
{
    public class CheckTagNames
    {
        private readonly PIServer _piServer;
        public List<string> InvalidLines { get; private set; }

        public CheckTagNames(PIServer piServer)
        {
            _piServer = piServer;
            InvalidLines = new List<string>();
        }

        public HashSet<string> GetTagNamesFromFile(string file)
        {
            HashSet<string> tagNames = new HashSet<string>();
            InvalidLines.Clear();

            using (StreamReader reader = new StreamReader(file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                        tagNames.Add(parts[0]);
                    else
                        InvalidLines.Add(line);
                }
            }

            return tagNames;
        }

        public List<string> GetInvalidTagNames(IEnumerable<string> tagNames)
        {
            List<string> invalidTagNames = new List<string>();

            foreach (string tagName in tagNames)
            {
                if (!PIPoint.TryFindPIPoint(_piServer, tagName, out _))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine($"Invalid tag - {tagName}");
                    Console.ResetColor();
                    invalidTagNames.Add(tagName);
                }
            }

            return invalidTagNames;
        }

        public void PrintInvalidTags(string file, IEnumerable<string> invalidTagNames)
        {
            Console.WriteLine("Found invalid tags in file: " + file);
            foreach (string tagName in invalidTagNames)
                Console.WriteLine("  " + tagName);
        }
    }
}
