using AdventureOfCode.Utilities.Image;

namespace AdventOfCode.Utilities.Tests.Image
{
    [TestFixture]
    public class Image2Tests
    {
        [Test]
        public void SetAndGetValue()
        {
            var sut = new Image2<int>(5, 10);
            var p = (0,0);
            const int testValue = -123;

            sut[p] = testValue;

            Assert.That(sut[p], Is.EqualTo(testValue));
        }

        [Test]
        public void SetValueThrowsIfOutofBounds()
        {
            var sut = new Image2<int>(5, 10);
            var p = (5, 0);
            Assert.Throws<IndexOutOfRangeException>(() => sut[p] = 0);
        }

        [Test]
        public void GetValueThrowsIfOutofBounds()
        {
            var sut = new Image2<int>(5, 10);
            var p = (0, -1);
            Assert.Throws<IndexOutOfRangeException>(() => { var _ = sut[p]; });
        }

        [Test]
        public void CheckPointExists()
        {
            var sut = new Image2<int>(5, 10);
            var p = (2, 3);
            sut[p] = -123;
            Assert.That(sut.Exists(p));
            Assert.That(sut.Exists((0,0)), Is.False);
        }

        [Test]
        public void TryGetValue()
        {
            var sut = new Image2<int>(5, 10);
            var p = (2, 3);
            int testValue = -123;
            sut[p] = testValue;
            Assert.That(sut.TryGetValue(p, out int v));
            Assert.That(v, Is.EqualTo(testValue));
            Assert.That(sut.TryGetValue((0,0), out int v2), Is.False);
        }
    }
}
