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
    [TestCase(typeof(Day09), "6384282079460", "6408966547049")]
    [TestCase(typeof(Day10), "733", "1514")]
    [TestCase(typeof(Day11), "203228", "240884656550923")]
    [TestCase(typeof(Day12), "1363484", "838988")]
    [TestCase(typeof(Day13), "29023", "96787395375634")]
    [TestCase(typeof(Day15), "1398947", "1397393")]
    [TestCase(typeof(Day16), "135536", "583")]
    [TestCase(typeof(Day17), "1,5,7,4,1,6,0,3,0", "108107574778365")]
    [TestCase(typeof(Day18), "314", "(15, 20)")]
    [TestCase(typeof(Day19), "350", "769668867512623")]
    [TestCase(typeof(Day20), "1399", "994807")]
    [TestCase(typeof(Day21), "94284", "116821732384052")]
    [TestCase(typeof(Day22), "18261820068", "2044")]
    [TestCase(typeof(Day23), "1400", "am,bc,cz,dc,gy,hk,li,qf,th,tj,wf,xk,xo")]
    [TestCase(typeof(Day24), "50411513338638", "gfv,hcm,kfs,tqm,vwr,z06,z11,z16")]
    [TestCase(typeof(Day25), "3317", "")]
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
