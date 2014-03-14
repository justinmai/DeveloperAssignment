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
  /// Use mocking and autoresetEvent technique. Reset event is used for getting access to RefreshAmount function. 
  /// </summary>
  [TestClass]
  public class TestMethod
  {
    [TestMethod]
    public void TestRefreshAmount()
    {
      var mock = new MockRepository();
      var getAmountEventReceived = new AutoResetEvent(false);

      var accountServiceMock = mock.StrictMock<IAccountService>();
      var getAmountResetEventMock = mock.StrictMock<AutoResetEvent>();

      var expectedAccountId = new Random().Next(100);
      var accountInfoToTest = new AccountInfo(expectedAccountId, accountServiceMock, getAmountResetEventMock);

      var expectedReturnAmount = new Random().NextDouble();
      Expect.Call(getAmountResetEventMock.WaitOne(0)).Return(true);
      Expect.Call(getAmountResetEventMock.WaitOne(0)).Return(false);
      Expect.Call(accountServiceMock.GetAccountAmount(expectedAccountId)).Return(expectedReturnAmount);
      Expect.Call(getAmountResetEventMock.Set()).Return(true).WhenCalled(MethodInvocation => getAmountEventReceived.Set());
      mock.ReplayAll();

      getAmountEventReceived.Reset();

      accountInfoToTest.RefreshAmount();
      accountInfoToTest.RefreshAmount();

      Assert.IsTrue(getAmountEventReceived.WaitOne(1000));
      Assert.AreEqual(expectedReturnAmount, accountInfoToTest.Amount);

      mock.VerifyAll();

    }

  }
}
