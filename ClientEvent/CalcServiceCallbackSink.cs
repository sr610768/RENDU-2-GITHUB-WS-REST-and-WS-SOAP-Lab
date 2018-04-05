using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientEvent
{
    class CalcServiceCallbackSink : ServiceEvent.ICalcServiceCallback
    {
        public void Calculated(int numberOfBikes, string city, string station)
        {
            if (numberOfBikes == -1)
            {
                Console.WriteLine("Vérifiez votre connexion internet ou que les noms que vous avez entrés sont correctes (le programme est sensible à la casse)");
                return;
            }

            Console.WriteLine(numberOfBikes + " vélos sont disponibles dans la ville " + city + " dans la station " + station);
        }

        public void CalculationFinished()
        {
            Console.WriteLine("Calculation completed");
        }
    }
}
