using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class CalcService : ICalcService
    {
        private static readonly string API_KEY = "5adfd2b9c75749261a8f2640290caad53559518a";
        public const int DelayMilliseconds = 10000;

        static Action<int, string, string> m_Event1 = delegate { };//

        static Action m_Event2 = delegate { };

        public void Calculate(string city, string station, int time)
        {
            int availableBikes = getAvailableBikesService(city, station);
            m_Event1(availableBikes, city, station);
            m_Event2();
        }

        public void SubscribeCalculatedEvent()
        {
            ICalcServiceEvents subscriber = OperationContext.Current.GetCallbackChannel<ICalcServiceEvents>();
            m_Event1 += subscriber.Calculated;
        }

        public void SubscribeCalculationFinishedEvent()
        {
            ICalcServiceEvents subscriber = OperationContext.Current.GetCallbackChannel<ICalcServiceEvents>();
            m_Event2 += subscriber.CalculationFinished;
        }
        private int getAvailableBikesService(string city, string station)
        {
            try
            {
                WebRequest requete = WebRequest.Create("https://api.jcdecaux.com/vls/v1/stations?contract=" + city + "&apiKey=" + API_KEY);
                WebResponse reponse = requete.GetResponse();
                Stream stream = reponse.GetResponseStream();

                StreamReader reader = new StreamReader(stream);
                string json = reader.ReadToEnd();

                if (reponse == null || station == null)
                {
                    return -1;
                }

                reponse.Close();
                reader.Close();

                JArray jsonArray = JArray.Parse(json);

                foreach (JObject item in jsonArray)
                {
                    string name = (string)item.SelectToken("name");

                    if (name.Contains(station))
                    {
                        return (int)item.SelectToken("available_bikes");
                    }
                }
            }
            
            catch(Exception e)
            {

            }

            return -1;
        }
    }
}
