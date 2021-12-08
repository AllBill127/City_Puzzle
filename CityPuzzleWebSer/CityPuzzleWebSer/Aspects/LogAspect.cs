using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components;
using Castle.DynamicProxy;
using Serilog;

namespace CityPuzzleWebSer.Aspects
{
    public class LogAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
                //After the successful invocation
                Log.Logger.Information($"Method {invocation.Method.Name} " +
                    $"called with these parameters: {Newtonsoft.Json.JsonConvert.SerializeObject(invocation.Arguments)}" +
                    $"returned this response: {Newtonsoft.Json.JsonConvert.SerializeObject(invocation.ReturnValue)}");
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Error happened in method: {invocation.Method}. Error: {Newtonsoft.Json.JsonConvert.SerializeObject(ex)}");
                throw; //If it's needed to process somewhere else
            }
        }
    }
}
