using Sample.Core;
using System.Runtime.InteropServices;

class Program : Client
{
    static void Main() => new Program().RunApp();

    private Handle<SDL.Window> window;

    protected override bool CreateView()
    {
        if (SDL.Init(SDL.InitFlags.Everything) != 0)
        {
            SDL.ReportError();
            return false;
        }

        window = SDL.CreateWindow(
            "Launcher [Desktop]", SDL.WindowPosCentered(), SDL.WindowPosCentered(), 640, 480,
            SDL.WindowFlags.Shown | SDL.WindowFlags.Resizable);
        if (window.IsNull)
        {
            SDL.ReportError();
            SDL.Quit();
            return false;
        }

        renderer = SDL.CreateRenderer(window, -1, SDL.RendererFlags.Accelerated);
        if (renderer.IsNull) renderer = SDL.CreateRenderer(window, -1, SDL.RendererFlags.Software);
        if (renderer.IsNull)
        {
            SDL.ReportError();
            SDL.DestroyWindow(window);
            SDL.Quit();
            return false;
        }

        return true;
    }

    protected override void DestroyView()
    {
        SDL.DestroyRenderer(renderer);
        SDL.DestroyWindow(window);
        SDL.Quit();
    }

    protected override nint LoadSDLLibrary()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var handle = NativeLibrary.Load("SDL2.dll");
                unsafe
                {
                    // SDL successfully loaded, can hide console window
                    var kernel32 = NativeLibrary.Load("kernel32.dll");
                    var freeConsole = (delegate* unmanaged<int>)NativeLibrary.GetExport(kernel32, "FreeConsole");
                    freeConsole();
                    NativeLibrary.Free(kernel32);
                }
                return handle;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return NativeLibrary.Load("libSDL2-2.0.so.0");
        }
        catch (DllNotFoundException)
        {
            Console.Error.WriteLine("<!!! FATAL: Could not find SDL2 shared library !!!>");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Console.Error.WriteLine("""
                    Original download provides a .zip archive with SDL2 library bundled.
                    If you do not have it for some reason, download and place it near to
                    this executable, named 'SDL2.dll'
                    """);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Console.Error.WriteLine("""
                    Install SDL2 on your system. The easiest way to do this is using
                    package manager:
                      `sudo apt install libsdl2-2.0-0` - for Debian-based systems
                      `sudo dnf install SDL2` - for Red Hat-based systems
                    """);
            Console.Error.WriteLine("[See https://wiki.libsdl.org/SDL2/Installation for more info]");
            Thread.Sleep(2500);
            Environment.Exit(-1);
        }
        throw new PlatformNotSupportedException();
    }

    private const float Accel = 0.05f;

    protected override void ProcessEvent(ref readonly SDL.Event @event)
    {
        if (@event.Type == SDL.EventType.Quit) running = false;

        if (@event.Type == SDL.EventType.KeyDown)
        {
            if (@event.Key.KeySym.Scancode == SDL.Scancode.Escape) running = false;
            if (@event.Key.KeySym.Scancode == SDL.Scancode.Up) speed += Accel;
            if (@event.Key.KeySym.Scancode == SDL.Scancode.Down) speed -= Accel;

            if (@event.Key.KeySym.Scancode == SDL.Scancode.D) saturation = MathF.Min(1, saturation + Accel);
            if (@event.Key.KeySym.Scancode == SDL.Scancode.A) saturation = MathF.Max(0, saturation - Accel);
            if (@event.Key.KeySym.Scancode == SDL.Scancode.W) value = MathF.Min(1, value + Accel);
            if (@event.Key.KeySym.Scancode == SDL.Scancode.S) value = MathF.Max(0, value - Accel);
        }

        if (@event.Type == SDL.EventType.MouseWheel)
        {
            var dy = @event.Wheel.PreciseY;
            if (@event.Wheel.Direction == SDL.MouseWheelDirection.Flipped) dy *= -1;
            speed += dy * Accel;
        }
    }
}
