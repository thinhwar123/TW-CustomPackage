using System;
using System.IO;

public static class GeneratorLog
{
    private const string LogPath = "C:\\Users\\nguye\\RiderProjects\\TW.SourcesGenerator.AEnumQuickConvert\\TW.SourcesGenerator.AEnumQuickConvert";
    
    public static void ClearLog()
    {
        File.WriteAllText($"{LogPath}/GeneratorLog.txt", "");
    }
    
    public static void Log(string message)
    {
        File.AppendAllText($"{LogPath}/GeneratorLog.txt", message + Environment.NewLine);
    }
}