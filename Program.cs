using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using OSIsoft.AF;
using OSIsoft.AF.PI;

namespace TagValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read values from app.config file
            string serverName = GetOptionValue(args, "serverName") ?? ConfigurationManager.AppSettings["serverName"];
            string inputFolder = GetOptionValue(args, "inputFilesFolder") ?? ConfigurationManager.AppSettings["inputFilesFolder"];
            string outputFolder = GetOptionValue(args, "outputFilesFolder") ?? ConfigurationManager.AppSettings["outputFilesFolder"];

            // Make an instance of class ConfigChecker
            ConfigChecker configChecker = new ConfigChecker(serverName, inputFolder, outputFolder);

            // Call function ValidateConfig
            configChecker.ValidateConfig();

            // Connect to PI Data Archive
            PIServer piServer = new PIServers()[serverName];
            try
            {
                piServer.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to server: " + ex.Message);
                return;
            }

            CheckTagNames checkTagNames = new CheckTagNames(piServer);

            // Process each file in the input folder
            string[] files = Directory.GetFiles(inputFolder, "*.csv");

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                Console.WriteLine($"Processing file [{i + 1}/{files.Length}] - {Path.GetFileName(file)}");

                // Read tag names from file
                HashSet<string> tagNames = checkTagNames.GetTagNamesFromFile(file);

                // Check if tag names exist on server
                List<string> invalidTagNames = checkTagNames.GetInvalidTagNames(tagNames);

                // Write invalid tag names to file
                if (invalidTagNames.Count > 0)
                {
                    //checkTagNames.PrintInvalidTags(file, invalidTagNames);

                    string outputFile = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(file) + "_invalidTags.txt");
                    if (File.Exists(outputFile))
                    {
                        File.Delete(outputFile);
                    }

                    File.WriteAllLines(outputFile, invalidTagNames);
                }

                var invalidLines = checkTagNames.InvalidLines;
                if (invalidLines.Count > 0)
                {
                    // Write invalid lines to file
                    string invalidLinesFile = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(file) + "_invalid_lines.txt");

                    //Delete file if it exists
                    if (File.Exists(invalidLinesFile))
                    {
                        File.Delete(invalidLinesFile);
                    }

                    File.WriteAllLines(invalidLinesFile, invalidLines);
                }
            }

            Console.WriteLine("Done!");
            Environment.Exit(0);
        }
        private static string GetOptionValue(string[] args, string optionName)
        {
            var option = args.FirstOrDefault(arg => arg.StartsWith($"{optionName}="));
            return option?.Split('=')[1];
        }
    }
}
