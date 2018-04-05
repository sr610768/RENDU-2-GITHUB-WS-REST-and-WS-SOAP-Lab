using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientEvent
{
    class CalcServiceCallbackSink : ServiceEvent.ICalcServiceCallback
    {
        public void LoadDetailsFromServer(string city, string station, string result)
        {
            Console.WriteLine("Ville: {0}", city);
            Console.WriteLine("Station: {0}", station);
            Console.WriteLine("Velos: {0}", result);
        }

        public void CalculationFinished()
        {
            throw new NotImplementedException();
        }
    }
}
