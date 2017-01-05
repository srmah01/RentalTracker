using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RentalTracker.Domain.Tests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void CanCreateAccount()
        {
            var expected = new Account();

            Assert.IsNotNull(expected);
        }
    }
}
