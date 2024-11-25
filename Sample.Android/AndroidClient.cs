using Sample.Core;
using System.Runtime.InteropServices;

namespace Sample.Android;

public class AndroidClient : Client
{
    private Handle<SDL.Window> window;

    protected override bool CreateView()
    {
        if (SDL.Init(SDL.InitFlags.Everything) != 0)
        {
            SDL.ReportError();
            return false;
        }

        window = SDL.CreateWindow(
            "Launcher [Android]", SDL.WindowPosCentered(), SDL.WindowPosCentered(), 640, 480,
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

    protected override nint LoadSDLLibrary() => NativeLibrary.Load("libSDL2.so");

    private const float Accel = 1.5f;

    private int fingerCount = 0;
    private SDL.FingerID firstFinger;

    protected override void ProcessEvent(ref readonly SDL.Event @event)
    {
        if (@event.Type == SDL.EventType.Quit) running = false;

        if (@event.Type == SDL.EventType.FingerDown)
        {
            ++fingerCount;
            if (fingerCount == 1) firstFinger = @event.TFinger.FingerID;
        }
        if (@event.Type == SDL.EventType.FingerUp) --fingerCount;

        if (@event.Type == SDL.EventType.FingerMotion && @event.TFinger.FingerID == firstFinger)
        {
            if (fingerCount == 1)
            {
                speed += @event.TFinger.DY * Accel;
            }
            else
            {
                value = Math.Min(1, Math.Max(0, value + @event.TFinger.DX * Accel));
                saturation = Math.Min(1, Math.Max(0, saturation + @event.TFinger.DY * Accel));
            }
        }
    }
}
