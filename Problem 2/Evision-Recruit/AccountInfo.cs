using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Evision_Recruit
{

  public interface IAccountService
  {
    double GetAccountAmount(int _accountId);
  }

  public class AccountInfo
  {
    private readonly int _accountId;
    private readonly IAccountService _accountService;
    private readonly AutoResetEvent getAccountAmountResetEvent;

    public AccountInfo(int accountId, IAccountService accountService, AutoResetEvent accountAmountResetEvent)
    {
      _accountId = accountId;
      _accountService = accountService;
      getAccountAmountResetEvent = accountAmountResetEvent;
    }
    public double Amount { get; private set; }

    private delegate double GetAccountAmountDelegate(int accountId);

    public void RefreshAmount()
    {
      if (getAccountAmountResetEvent.WaitOne(0))
      {
        var getAccountAmountdlg = new GetAccountAmountDelegate(GetAccountAmount);
        getAccountAmountdlg.BeginInvoke(this._accountId, CallBack, getAccountAmountdlg);
      }
    }

    private void CallBack(IAsyncResult ar)
    {
      var handler = (GetAccountAmountDelegate)ar.AsyncState;
      Amount = handler.EndInvoke(ar);

      getAccountAmountResetEvent.Set();
    }

    private double GetAccountAmount(int accountid)
    {
      return _accountService.GetAccountAmount(accountid);
    }


  }
}
