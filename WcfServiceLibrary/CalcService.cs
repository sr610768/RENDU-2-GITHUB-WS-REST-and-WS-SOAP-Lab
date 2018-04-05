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
        static Action<string, string, string> m_Event1 = delegate { };
        static Action m_Event2 = delegate { };

        public void SubscribeCalculatedEvent()
        {
            ICalcServiceEvents subscriber =
            OperationContext.Current.GetCallbackChannel<ICalcServiceEvents>();
            m_Event1 += subscriber.LoadDetailsFromServer;
        }

        public void SubscribeCalculationFinishedEvent()
        {
            ICalcServiceEvents subscriber =
            OperationContext.Current.GetCallbackChannel<ICalcServiceEvents>();
            m_Event2 += subscriber.CalculationFinished;
        }

        public void LoadDetailsFromServer(string city, string station)
        {
            string apiKey = "5adfd2b9c75749261a8f2640290caad53559518a";

            string url = "https://api.jcdecaux.com/vls/v1/stations?contract=" + city + "&apiKey=" + apiKey;

            Console.WriteLine("\nRequête : " + url);
            Console.WriteLine("\nEn attente de réponse ...");

            /* REQUEST */

            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                WebResponse response = request.GetResponse();

                if (response == null)
                {
                    //Console.WriteLine("\nRéponse à la requête : Erreur votre ville ou clé n'existe pas.");
                }

                else
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);

                    string responseFromServer = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                    /* PARSING JSON */

                    JArray j = JArray.Parse(responseFromServer);

                    bool found = false;

                    foreach (JObject item in j)
                    {
                        string name = (string)item.SelectToken("name");

                        /* DISPLAY THE DETAILS OF THE SELECTED STATION */

                        if (name.Contains(station))
                        {
                            found = true;

                            m_Event1(city, station, (string) item.SelectToken("available_bikes"));
                            m_Event2();

                            break;
                        }
                    }

                    if (!found)
                    {
                        //Console.WriteLine("\nLe nom du point d'accès que vous avez entré est incorrect");
                    }
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine("\nErreur : \n" + exception.StackTrace);
            }

            Console.WriteLine("\n\nAppuyez sur n'importe quelle touche pour terminer le programme...");
            Console.ReadLine();
        }
    }
}
