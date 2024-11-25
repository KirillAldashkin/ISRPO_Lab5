using System;
using System.Runtime.InteropServices;

namespace Sample.Core;

#pragma warning disable IDE0079 // They are NOT unnecessary
#pragma warning disable CA1401 // Nope, they will be used in the launcher projects

public static partial class SDL
{
    [LibraryImport(Name, EntryPoint = "SDL_Init")]
    public static partial int Init(InitFlags flags);

    [LibraryImport(Name, EntryPoint = "SDL_Quit")]
    public static partial void Quit();

    [LibraryImport(Name, EntryPoint = "SDL_GetError")]
    [return: MarshalAs(UnmanagedType.LPUTF8Str)]
    public static partial string GetError();

    [LibraryImport(Name, EntryPoint = "SDL_ShowSimpleMessageBox")]
    public static partial int ShowSimpleMessageBox(
        MessageBoxFlags flags, 
        [MarshalAs(UnmanagedType.LPUTF8Str)] string title, 
        [MarshalAs(UnmanagedType.LPUTF8Str)] string message, 
        Handle<Window> window);

    [LibraryImport(Name, EntryPoint = "SDL_CreateWindow")]
    public static partial Handle<Window> CreateWindow(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string title,
        int x, int y, int w, int h, 
        WindowFlags flags);

    [LibraryImport(Name, EntryPoint = "SDL_DestroyWindow")]
    public static partial void DestroyWindow(Handle<Window> window);

    [LibraryImport(Name, EntryPoint = "SDL_CreateRenderer")]
    public static partial Handle<Renderer> CreateRenderer(Handle<Window> window, int index, RendererFlags flags);

    [LibraryImport(Name, EntryPoint = "SDL_DestroyRenderer")]
    public static partial void DestroyRenderer(Handle<Renderer> renderer);

    [LibraryImport(Name, EntryPoint = "SDL_RenderClear")]
    public static partial int RenderClear(Handle<Renderer> renderer);

    [LibraryImport(Name, EntryPoint = "SDL_SetRenderDrawColor")]
    public static partial int SetRenderDrawColor(Handle<Renderer> renderer, byte r, byte g, byte b, byte a);

    [LibraryImport(Name, EntryPoint = "SDL_RenderPresent")]
    public static partial void RenderPresent(Handle<Renderer> renderer);

#pragma warning disable SYSLIB1051 // Uhh... it seems to be confused by a custom struct layout and 'out'
    [LibraryImport(Name, EntryPoint = "SDL_PollEvent")]
    public static partial int PollEvent(out Event @event);
#pragma warning restore SYSLIB1051
}
