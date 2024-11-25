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
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return NativeLibrary.Load("SDL2.dll");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return NativeLibrary.Load("libSDL2-2.0.so.0");
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
