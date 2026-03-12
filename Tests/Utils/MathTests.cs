using System;
using NUnit.Framework;
using Utils;

namespace Tests.Utils
{
    [TestFixture]
    public class MathTests
    {
        [Test]
        public void CalculateTorusSegmentLength_ShouldReturnZeroWhenRadiusIsZero()
        {
            double result = MathExtensions.CalculateTorusSegmentLength(0, Math.PI / 4);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CalculateTorusSegmentLength_ShouldReturnZeroWhenAngleIsZero()
        {
            double result = MathExtensions.CalculateTorusSegmentLength(5, 0);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void CalculateTorusSegmentLength_ShouldReturnCorrectValueForPositiveRadiusAndAngle()
        {
            double result = MathExtensions.CalculateTorusSegmentLength(5, Math.PI / 4);
            Assert.AreEqual(5 * Math.Tan(Math.PI / 8), result, 1e-6);
        }

        [Test]
        public void CalculateTorusSegmentLength_ShouldHandleNegativeRadius()
        {
            double result = MathExtensions.CalculateTorusSegmentLength(-5, Math.PI / 4);
            Assert.AreEqual(-5 * Math.Tan(Math.PI / 8), result, 1e-6);
        }

        [Test]
        public void CalculateTorusSegmentLength_ShouldHandleNegativeAngle()
        {
            double result = MathExtensions.CalculateTorusSegmentLength(5, -Math.PI / 4);
            Assert.AreEqual(5 * Math.Tan(-Math.PI / 8), result, 1e-6);
        }
    }
}