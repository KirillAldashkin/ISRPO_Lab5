using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sample.Core;

[DebuggerDisplay($"{{ToString(),nq}}")]
public readonly struct Color(byte r, byte g, byte b, byte a = 255)
{
    public readonly byte R = r;
    public readonly byte G = g;
    public readonly byte B = b;
    public readonly byte A = a;

    public static Color FromHSV(float h, float s, float v)
    {
        Range(h, 0, float.BitDecrement(MathF.Tau));
        Range(s, 0, 1);
        Range(v, 0, 1);

        var vmin = (1 - s) * v;
        var a = (v - vmin) * ((h * 6 / MathF.Tau) % 1);
        var vinc = vmin + a;
        var vdec = v - a;

        var x = (byte)(v * float.BitDecrement(256));
        var y = (byte)(vinc * float.BitDecrement(256));
        var z = (byte)(vmin * float.BitDecrement(256));
        var w = (byte)(vdec * float.BitDecrement(256));

        var code = (int)(h / MathF.Tau * 6);

        return code switch
        {
            0 => new(x, y, z),
            1 => new(w, x, z),
            2 => new(z, x, y),
            3 => new(z, w, x),
            4 => new(y, z, x),
            5 => new(x, z, w),
            _ => throw null!
        };

        static void Range(float v, float min, float max, 
                          [CallerArgumentExpression(nameof(v))] string name = "")
        {
            if (min <= v && v <= max) return;
            throw new ArgumentOutOfRangeException(name, $"Must be in range [{min:F7};{max:F7}], was {v:F7}");
        }
    }

    public static bool operator==(Color l, Color r) => (l.R == r.R) && (l.G == r.G) && (l.B == r.B) && (l.A == r.A);
    public static bool operator!=(Color l, Color r) => !(l == r);

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Color c && c == this;
    public override int GetHashCode() => HashCode.Combine(R, G, B, A);
    public override string ToString() => $"RGBA=#{R:X2}{G:X2}{B:X2}{A:X2}";
}
