using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClientEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            CalcServiceCallbackSink objsink = new CalcServiceCallbackSink();
            InstanceContext iCntxt = new InstanceContext(objsink);

            ServiceEvent.CalcServiceClient objClient = new ServiceEvent.CalcServiceClient(iCntxt);
            objClient.SubscribeCalculatedEvent();
            objClient.SubscribeCalculationFinishedEvent();


            objClient.LoadDetailsFromServer("Valade", "Toulouse");

            Console.WriteLine("Press any key to close ...");
            Console.ReadKey();

            Console.WriteLine("Press any key to close ...");
            Console.ReadKey();
        }
    }
}
