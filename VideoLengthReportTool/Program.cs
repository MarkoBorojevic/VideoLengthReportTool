// See https://aka.ms/new-console-template for more information
Console.WriteLine("Input file names:");

string[] fileNames = Console.ReadLine().Split(" ");

Console.WriteLine("Write output path:");

string output = Console.ReadLine();

Console.WriteLine(await VideoLengthReportTool.LengthTool.ExportVideoDurations(fileNames, output));
