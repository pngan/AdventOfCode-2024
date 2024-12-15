using AdventureOfCode.Utilities.Image;

namespace AdventOfCode.Utilities.Tests.Image
{
    [TestFixture]
    public class Point2Tests
    {
        [Test]
        public void TestAddOperator()
        {
            var result = Step2.N + Step2.NW;
            Assert.That(result, Is.EqualTo(new Step2(-2, -1)));
        }

        [Test]
        public void TestSubtractOperator()
        {
            var result = Step2.N - Step2.SE;
            Assert.That(result, Is.EqualTo(new Step2(-2, -1)));
        }

        [Test]
        public void TestNegateOperator()
        {
            Assert.That(Step2.NW, Is.EqualTo(-Step2.SE));
        }

        [Test]
        public void TestEqualityOperator()
        {
            Assert.That(Step2.NW, Is.EqualTo(Step2.NW));
        }

        [Test]
        public void TestNonEqualityOperator()
        {
            Assert.That(Step2.NW, Is.Not.EqualTo(Step2.W));
        }
    }
}
