using NUnit.Framework;
// ReSharper disable PossibleNullReferenceException

namespace SparseCodingString.Tests
{
    [TestFixture]
    public class IndexingTests {
        [Test]
        public void can_get_first_character () {
            var source = "Hello, world";
            var subject = SparseString.FromString(source);

            var result = subject.CharAt(0);

            Assert.That(result, Is.EqualTo(source[0]));
        }

        [Test]
        public void can_get_last_character () {
            var source = "Hello, world";
            var subject = SparseString.FromString(source);

            var result = subject.CharAt(11);

            Assert.That(result, Is.EqualTo(source[11]));
        }

        [Test]
        public void can_get_any_index () {
            var source = "Hello, world";
            var subject = SparseString.FromString(source);

            Assert.That(subject.CharAt(4), Is.EqualTo(source[4]));
            Assert.That(subject.CharAt(7), Is.EqualTo(source[7]));
            Assert.That(subject.CharAt(9), Is.EqualTo(source[9]));
        }
    }
}