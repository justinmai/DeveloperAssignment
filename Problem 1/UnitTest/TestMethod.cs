using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evision_Recruit;
using Rhino.Mocks;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest
{
  /// <summary>
  /// Use mocking technique. Assumption is IAccountService.
  /// GetAccountAmount does not throw exception over interface (usually the case, stated otherwise).
  /// </summary>
  [TestClass]
  public class TestMethod
  {
    [TestMethod]
    public void TestRefreshAmount()
    {
      var mock = new MockRepository();
      var accountServiceMock = mock.StrictMock<IAccountService>();

      var expectedAccountId = new Random().Next(100);
      var accountInfoToTest = new AccountInfo(expectedAccountId, accountServiceMock);

      var expectedReturnAmount = new Random().NextDouble();
      Expect.Call(accountServiceMock.GetAccountAmount(expectedAccountId)).Return(expectedReturnAmount);
      mock.ReplayAll();

      accountInfoToTest.RefreshAmount();
      Assert.AreEqual(expectedReturnAmount, accountInfoToTest.Amount);

      mock.VerifyAll();
    }

  }
}
