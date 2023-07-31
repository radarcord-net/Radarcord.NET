using System;

namespace Radarcord.Logging;

static internal class Logger
{
    private static void SetColorAndLog(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void Info(string message)
    {
        SetColorAndLog(ConsoleColor.Cyan, $"[INFO] {message}");
    }

    public static void Error(string message)
    {
        SetColorAndLog(ConsoleColor.Red, $"[ERROR] {message}");
    }

    public static void Warn(string message)
    {
        SetColorAndLog(ConsoleColor.Yellow, $"[WARN] {message}");
    }

    public static void Debug(string message)
    {
        SetColorAndLog(ConsoleColor.Blue, $"[DEBUG] {message}");
    }
}
