using BCServerlessDemo.DataAndFunctions.Core.Service;
using BCServerlessDemo.DataAndFunctions.Functions.Startup;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.TimerTriggers
{
    public static class DailyTimerTrigger
    {
        /*
         * A CRON expression for the Azure Functions timer trigger includes six fields:
         * {second} {minute} {hour} {day} {month} {day-of-week}
         * https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer#cron-expressions
         * */
        [FunctionName("DailyTimerTrigger")]
        public static async Task Run([TimerTrigger("0 0 1 * * *")] TimerInfo myTimer, TraceWriter log, 
            [Inject(typeof(IDriverService))]IDriverService driverService)
        {
            try
            {
                // TODO: something daily
            }
            catch (Exception ex)
            {
                log.Error("Error - DailyTimerTrigger", ex);
                throw;
            }
        }
    }
}
