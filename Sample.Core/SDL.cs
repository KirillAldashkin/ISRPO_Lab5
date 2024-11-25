namespace Sample.Core;

public static partial class SDL
{
    public const string Name = "SDL";
    public const KeyCode ScancodeMask = (KeyCode)(1 << 30);

    public static void ReportError()
    {
        var error = GetError();
        if (ShowSimpleMessageBox(MessageBoxFlags.Error, "SDL_GetError()", error, default) == 0) return;
        Console.Error.WriteLine($"SDL_GetError(): {error}");
    }

    public static int WindowPosCentered(int index = 0) => 0x2FFF0000 | index;
}
