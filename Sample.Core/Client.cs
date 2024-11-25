using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Sample.Core;

public abstract class Client
{
    protected Handle<SDL.Renderer> renderer;
    protected bool running;
    protected float speed = 2f;
    protected float saturation = 1f;
    protected float value = 1f;

    public void RunApp()
    {
        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), ResolveDll);
        if (!CreateView()) return;
        running = true;
        float angle = 0;
        var time = Stopwatch.StartNew();
        var prevTime = time.Elapsed;
        while (true)
        {
            while (SDL.PollEvent(out var e) == 1) ProcessEvent(ref e);
            if (!running) break;

            angle = (angle + speed * (float)(time.Elapsed - prevTime).TotalSeconds) % MathF.Tau;
            prevTime = time.Elapsed;
            var color = Color.FromHSV(angle, saturation, value);
            if (SDL.SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A) != 0)
            {
                SDL.ReportError();
                break;
            }
            if (SDL.RenderClear(renderer) != 0)
            {
                SDL.ReportError();
                break;
            }
            SDL.RenderPresent(renderer);
        }
        DestroyView();
    }

    private nint ResolveDll(string name, Assembly _, DllImportSearchPath? __)
    {
        if (name == SDL.Name) return LoadSDLLibrary();
        return 0;
    }

    protected abstract void ProcessEvent(ref readonly SDL.Event @event);
    protected abstract bool CreateView();
    protected abstract void DestroyView();
    protected abstract nint LoadSDLLibrary();
}
