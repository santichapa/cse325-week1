using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = FindDirectory(currentDirectory, "stores")
    ?? FindDirectory(AppContext.BaseDirectory, "stores")
    ?? throw new DirectoryNotFoundException(
        $"Could not find a 'stores' directory from '{currentDirectory}' or '{AppContext.BaseDirectory}'.");

var projectDirectory = Directory.GetParent(storesDirectory)?.FullName
    ?? throw new DirectoryNotFoundException($"Could not determine the parent directory of '{storesDirectory}'.");
var salesTotalDir = Path.Combine(projectDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);
WriteSalesSummary(salesFiles, salesTotal);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

static IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = [];

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        // The file name will contain the full path, so only check the end of it
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

static string? FindDirectory(string startDirectory, string directoryName)
{
    DirectoryInfo? directory = new(startDirectory);

    while (directory is not null)
    {
        var candidate = Path.Combine(directory.FullName, directoryName);
        if (Directory.Exists(candidate))
        {
            return candidate;
        }

        directory = directory.Parent;
    }

    return null;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

void WriteSalesSummary(IEnumerable<string> salesFiles, double salesTotal)
{
    File.WriteAllText(Path.Combine(salesTotalDir, "Sales Summary.txt"), $"Sales Summary{Environment.NewLine}-----------------------------{Environment.NewLine}Total Sales: {salesTotal:C}{Environment.NewLine}Details:{Environment.NewLine}");

    foreach (var file in salesFiles)
    {
        string fileData = File.ReadAllText(file);

        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(fileData);

        string fileSummaryLine = $"{Path.GetFileName(file)}: {data?.Total ?? 0:C}";

        File.AppendAllText(Path.Combine(salesTotalDir, "Sales Summary.txt"), $"{fileSummaryLine}{Environment.NewLine}");
    }
}

record SalesData (double Total);