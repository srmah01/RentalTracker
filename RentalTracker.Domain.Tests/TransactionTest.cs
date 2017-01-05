using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RentalTracker.Domain.Tests
{
    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void CanCreateTransaction()
        {
            var expected = new Transaction();

            Assert.IsNotNull(expected);
        }
    }
}
