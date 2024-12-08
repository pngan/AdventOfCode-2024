using AdventOfCode.Solutions.Common;
using AdventOfCode.Solutions.Days;

namespace AdventOfCode.Tests;

public class DayTests
{
    [TestCase(typeof(Day01), "1110981", "24869388")]
    [TestCase(typeof(Day02), "220", "296")]
    [TestCase(typeof(Day03), "181345830", "98729041")]
    [TestCase(typeof(Day04), "2401", "1822")]
    [TestCase(typeof(Day05), "5166", "4679")]
    [TestCase(typeof(Day06), "5129", "1888")]
    [TestCase(typeof(Day07), "303766880536", "337041851384440")]
    [TestCase(typeof(Day08), "247", "861")]
    public void Testing(Type dayType, string expectedPart1, string expectedPart2)
    {
        BaseDay? dayObj = Activator.CreateInstance(dayType) as BaseDay;

        if (!string.IsNullOrEmpty(expectedPart1))
        {
            Assert.That(dayObj!.Solve1(), Is.EqualTo(expectedPart1));
        }

        if (!string.IsNullOrEmpty(expectedPart2))
        {
            Assert.That(dayObj!.Solve2(), Is.EqualTo(expectedPart2));
        }
    }
}
