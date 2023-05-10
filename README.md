# Invalid Tag Checker Script

## Overview
This script reads a list of CSV files from a specified folder and checks if the tag names in the files exist in the OSI PI Historian. If a tag name does not exist, it is written to an output file in a specified output folder.

# Prerequisites
* .NET Framework 4.7.2 or higher
* OSIsoft PI AF Client

# Installation
1. Clone this repository or download the source code as a ZIP file and extract it.
2. Open the solution file (InvalidTagCheckerScript.sln) in Visual Studio.
3. Build the solution by clicking on the Build menu and selecting Build Solution.

# Configuration
The script can read the following parameters from a configuration file or as command line arguments:

* `serverName`: The name of the OSI PI Historian server.
* `inputFilesFolder` : The path to the folder where the input CSV files are located.
* `outputFilesFolder` : The path to the folder where the output files will be saved.

Here’s an example of what the configuration file should look like:

``` <?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>
	<appSettings>
		<add key="serverName" value="YOUR_SERVER_NAME"/>
		<add key="inputFilesFolder" value="C:\Path\To\Input\Folder"/>
		<add key="outputFilesFolder" value="C:\Path\To\Output\Folder"/>
	</appSettings>
</configuration>
```
To pass configuration options as command line arguments, use the following format:

```TagValidator.exe serverName="YOUR_SERVER_NAME" inputFilesFolder="C:\Path\To\Input\Folder" outputFilesFolder="C:\Path\To\Output\Folder"
Note that command line options take precedence over options specified in the configuration file.
```
# Usage
Place the CSV files in the specified input folder.
Run `TagValidator.exe` with the appropriate configuration options.
Check the output file in the output files folder for any invalid tag names.

# Contributing
Contributions are welcome! Please feel free to submit a pull request or open an issue if you have any suggestions or improvements.
