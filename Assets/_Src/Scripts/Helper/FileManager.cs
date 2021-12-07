using System;
using System.IO;
using UnityEngine;

public static class FileManager
{
    /// <summary>
    /// Write texts to file
    /// </summary>
    /// <param name="fileName">name of the file, will be created if not exist</param>
    /// <param name="contents">contents appened to the file</param>
    public static void TryWriteFile(string fileName, string contents)
    {
        string fullPath = Path.Combine(Application.dataPath, fileName);
        try
        {
            File.WriteAllText(fullPath, contents);
        }
        catch (Exception e)
        {
            Debug.LogError($"Uh, oh, Failed to write to {fileName} with exception {e}");
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="result"></param>
    /// <returns>true if file is read successfully, else false.</returns>
    public static bool TryReadFile(string fileName, out string result)
    {
        string fullPath = Path.Combine(Application.dataPath, fileName);
        if (!File.Exists(fullPath))
        {
            result = "";
            return false;
        }

        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Uh oh, Failed to read from {fileName} with exception {e}");
            result = "";
            return false;
        }

    }
}
