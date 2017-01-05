using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RentalTracker.Domain.Tests
{
    [TestClass]
    public class CategoryTest
    {
        [TestMethod]
        public void CanCreateCategory()
        {
            var expected = new Category();

            Assert.IsNotNull(expected);
        }
    }
}
