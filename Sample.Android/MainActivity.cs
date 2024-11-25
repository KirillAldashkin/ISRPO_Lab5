using Org.Libsdl.App;

namespace Sample.Android;

[Activity(MainLauncher = true)]
public class MainActivity : SDLActivity
{
    private AndroidClient _client = null!;

    public override void SDLMain(string[]? _) => _client.RunApp();

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        _client = new();
        base.OnCreate(savedInstanceState);
    }
}
