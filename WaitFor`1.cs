using System;
using System.Threading;

namespace BiblioUpTik
{
  public sealed class WaitFor<TResult>
  {
    private readonly TimeSpan _timeout;

    public WaitFor(TimeSpan timeout)
    {
      this._timeout = timeout;
    }

    public TResult Run(Func<TResult> function)
    {
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      object sync = new object();
      bool isCompleted = false;
      WaitCallback callBack = (WaitCallback) (obj =>
      {
        Thread thread = obj as Thread;
        lock (sync)
        {
          if (!isCompleted)
            Monitor.Wait(sync, this._timeout);
        }
        if (isCompleted)
          return;
        thread.Abort();
      });
      try
      {
        ThreadPool.QueueUserWorkItem(callBack, (object) Thread.CurrentThread);
        return function();
      }
      catch (ThreadAbortException ex)
      {
        Thread.ResetAbort();
        throw new TimeoutException(string.Format("The operation has timed out after {0}.", (object) this._timeout));
      }
      finally
      {
        lock (sync)
        {
          isCompleted = true;
          Monitor.Pulse(sync);
        }
      }
    }

    public static TResult Run(TimeSpan timeout, Func<TResult> function)
    {
      return new WaitFor<TResult>(timeout).Run(function);
    }
  }
}
