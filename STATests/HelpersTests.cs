namespace STATests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using STA;

    /// <summary>
    /// Tests for <see cref="Helpers"/>.
    /// </summary>
    [TestFixture]
    public class HelpersTests
    {
        /// <summary>
        /// Test for swapping.
        /// </summary>
        [Test]
        public void SwapTest()
        {
            // Setup.
            var list1 = new List<int> { 1, 2, 3 };
            var list2 = new List<int> { 4, 5, 6 };

            // Execute.
            Helpers.Swap(list1, list2, 1, 1);
            Helpers.Swap(list1, list1, 1, 2);

            // Verify.
            Assert.AreEqual(1, list1[0]);
            Assert.AreEqual(3, list1[1]);
            Assert.AreEqual(5, list1[2]);
            Assert.AreEqual(4, list2[0]);
            Assert.AreEqual(2, list2[1]);
            Assert.AreEqual(6, list2[2]);
        }
    }
}
