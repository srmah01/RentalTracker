using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RentalTracker.Domain.Tests
{
    [TestClass]
    public class PayeeTest
    {
        [TestMethod]
        public void CanCreatePayee()
        {
            var expected = new Payee();

            Assert.IsNotNull(expected);
        }
    }
}
