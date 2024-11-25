using Sample.Core;

namespace Test;

public class ColorTest
{
    [Test]
    public void Test_FromHSV_Bounds() => Assert.Multiple(() =>
    {
        float dec0 = float.BitDecrement(0);
        float inc1 = float.BitIncrement(1);

        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHSV(dec0, 0.5f, 0.5f));
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHSV(MathF.Tau, 0.5f, 0.5f));
        Assert.DoesNotThrow(() => Color.FromHSV(float.BitDecrement(MathF.Tau), 0.5f, 0.5f));
        Assert.DoesNotThrow(() => Color.FromHSV(0, 0.5f, 0.5f));

        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHSV(1, dec0, 0.5f));
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHSV(1, inc1, 0.5f));
        Assert.DoesNotThrow(() => Color.FromHSV(1, 0, 0.5f));
        Assert.DoesNotThrow(() => Color.FromHSV(1, 1, 0.5f));

        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHSV(1, 0.5f, dec0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHSV(1, 0.5f, inc1));
        Assert.DoesNotThrow(() => Color.FromHSV(1, 0.5f, 0));
        Assert.DoesNotThrow(() => Color.FromHSV(1, 0.5f, 1));
    });

    [Test]
    public void Test_FromHSV_ZeroValue() => Assert.Multiple(() =>
    {
        for (float h = 0; h < 6; h += 0.2f)
            for (float s = 0; s <= 1; s += 0.1f)
                Assert.That(Color.FromHSV(h, s, 0), Is.EqualTo(new Color(0, 0, 0)));
    });

    [Test]
    public void Test_FromHSV_ZeroSaturation() => Assert.Multiple(() =>
    {
        for (float h = 0; h < 6; h += 0.2f)
        {
            Assert.That(Color.FromHSV(h, 0, 0.00f), Is.EqualTo(new Color(0, 0, 0)));
            Assert.That(Color.FromHSV(h, 0, 0.25f), Is.EqualTo(new Color(63, 63, 63)));
            Assert.That(Color.FromHSV(h, 0, 0.50f), Is.EqualTo(new Color(127, 127, 127)));
            Assert.That(Color.FromHSV(h, 0, 0.75f), Is.EqualTo(new Color(191, 191, 191)));
            Assert.That(Color.FromHSV(h, 0, 1.00f), Is.EqualTo(new Color(255, 255, 255)));
        }
    });

    [Test]
    public void Test_FromHSV_Generic() => Assert.Multiple(() =>
    {
        Assert.That(Color.FromHSV(0, 1, 1), Is.EqualTo(new Color(255, 0, 0)));
        Assert.That(Color.FromHSV(MathF.Tau * 5 / 12, 1, 0.5f), Is.EqualTo(new Color(0, 127, 63)));
        Assert.That(Color.FromHSV(MathF.Tau * 5 / 6, 0.25f, 0.75f), Is.EqualTo(new Color(191, 143, 191)));
        Assert.That(Color.FromHSV(MathF.Tau / 18, 0.75f, 0.5f), Is.EqualTo(new Color(127, 63, 31)));
    });
}
