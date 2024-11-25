using Sample.Core;

namespace Sample.Tests;

[TestClass]
public sealed class ColorTest
{
    static void AssertNoThrow(Func<object?> func)
    {
        try
        {
            _ = func();
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected to not throw anything, thrown {}", ex);
        }
    }

    [TestMethod]
    public void Test_FromHSV_Bounds()
    {
        float dec0 = float.BitDecrement(0);
        float inc1 = float.BitIncrement(1);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Color.FromHSV(dec0, 0.5f, 0.5f));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Color.FromHSV(MathF.Tau, 0.5f, 0.5f));
        AssertNoThrow(() => Color.FromHSV(float.BitDecrement(MathF.Tau), 0.5f, 0.5f));
        AssertNoThrow(() => Color.FromHSV(0, 0.5f, 0.5f));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Color.FromHSV(1, dec0, 0.5f));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Color.FromHSV(1, inc1, 0.5f));
        AssertNoThrow(() => Color.FromHSV(1, 0, 0.5f));
        AssertNoThrow(() => Color.FromHSV(1, 1, 0.5f));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Color.FromHSV(1, 0.5f, dec0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Color.FromHSV(1, 0.5f, inc1));
        AssertNoThrow(() => Color.FromHSV(1, 0.5f, 0));
        AssertNoThrow(() => Color.FromHSV(1, 0.5f, 1));
    }

    [TestMethod]
    public void Test_FromHSV_ZeroValue()
    {
        for(float h = 0; h < 6; h += 0.2f)
            for (float s = 0; s <= 1; s += 0.1f)
                Assert.AreEqual(Color.FromHSV(h, s, 0), new(0, 0, 0));
    }

    [TestMethod]
    public void Test_FromHSV_ZeroSaturation()
    {
        for (float h = 0; h < 6; h += 0.2f)
        {
            Assert.AreEqual(Color.FromHSV(h, 0, 0.00f), new(0, 0, 0));
            Assert.AreEqual(Color.FromHSV(h, 0, 0.25f), new(63, 63, 63));
            Assert.AreEqual(Color.FromHSV(h, 0, 0.50f), new(127, 127, 127));
            Assert.AreEqual(Color.FromHSV(h, 0, 0.75f), new(191, 191, 191));
            Assert.AreEqual(Color.FromHSV(h, 0, 1.00f), new(255, 255, 255));
        }
    }

    [TestMethod]
    public void Test_FromHSV_Generic()
    {
        Assert.AreEqual(Color.FromHSV(0, 1, 1), new(255, 0, 0));
        Assert.AreEqual(Color.FromHSV(MathF.Tau * 5 / 12, 1, 0.5f), new(0, 127, 63));
        Assert.AreEqual(Color.FromHSV(MathF.Tau * 5 / 6, 0.25f, 0.75f), new(191, 143, 191));
        Assert.AreEqual(Color.FromHSV(MathF.Tau / 18, 0.75f, 0.5f), new(127, 63, 31));
    }
}
