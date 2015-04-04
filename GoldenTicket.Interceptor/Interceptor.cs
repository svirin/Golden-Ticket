using System;
using Castle.DynamicProxy;
using GoldenTicket.Logger.Log4Net;

namespace GoldenTicket.Interceptor
{
    public class Interceptor : IInterceptor
    {
        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            LogFactory.Log.InfoFormat("Start of {0} method of {1} type", invocation.Method.Name, invocation.TargetType.Name);

            try
            {
                invocation.Proceed();
  
            }
            catch (Exception exp)
            {
                LogFactory.Log.InfoFormat("Error of {0} method of {1} type. Exception message : {2}", invocation.Method.Name, invocation.TargetType.Name, exp);
                throw;
            }

            LogFactory.Log.InfoFormat("End of {0} method of {1} type", invocation.Method.Name, invocation.TargetType.Name);
        }

        #endregion
    }
}
