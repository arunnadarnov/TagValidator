using OSIsoft.AF.PI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagValidator
{
    public class ConfigChecker
    {
        private readonly string ServerName;
        private readonly string InputFolder;
        private readonly string OutputFolder;
        private readonly List<string> ErrorMessages;

        public ConfigChecker(string serverName, string inputFolder, string outputFolder)
        {
            ServerName = serverName;
            InputFolder = inputFolder;
            OutputFolder = outputFolder;
            ErrorMessages = new List<string>();
        }

        public void ValidateConfig()
        {
            CheckServerName();
            CheckInputFolder();
            CheckOutputFolder();

            if (ErrorMessages.Count > 0)
            {
                foreach (string errorMessage in ErrorMessages)
                    Console.WriteLine(errorMessage);

                Environment.Exit(0);
            }
        }

        private void CheckServerName()
        {
            if (string.IsNullOrEmpty(ServerName))
                ErrorMessages.Add("Server name is missing in app.config file.");
            else
            {
                try
                {
                    PIServer piServer = new PIServers()[ServerName];
                    piServer.Connect();
                }
                catch (Exception)
                {
                    ErrorMessages.Add("Unable to connect to server. Please check server name.");
                }
            }
        }

        private void CheckInputFolder()
        {
            if (string.IsNullOrEmpty(InputFolder))
                ErrorMessages.Add("Input folder is missing in app.config file.");
            else if (!Directory.Exists(InputFolder))
                ErrorMessages.Add("Input folder does not exist.");
            else if (Directory.GetFiles(InputFolder, "*.csv").Length == 0)
                ErrorMessages.Add("No .csv files found in input folder.");
        }

        private void CheckOutputFolder()
        {
            if (string.IsNullOrEmpty(OutputFolder))
                ErrorMessages.Add("Output folder is missing in app.config file.");
            else if (!Directory.Exists(OutputFolder))
            {
                try
                {
                    Directory.CreateDirectory(OutputFolder);
                }
                catch (Exception)
                {
                    ErrorMessages.Add("Output folder doesn't exist. Tried to create it but couldn't. Please check for necessary permissions.");
                }
            }
        }
    }
}
