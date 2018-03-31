using System;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IntermediaryServiceClient.Service1Client client = new IntermediaryServiceClient.Service1Client();

            Console.WriteLine("Bienvenue dans l'application client version console");

            string request = "";
            bool gotStations = false;
            string[] requests = { "exit", "select cities", "select stations -", "select details -", "help" };

            while (true)
            {
                Console.WriteLine("\n\nTapez help pour obtenir la liste des commandes");
                Console.WriteLine("Votre commande (sensible à la casse) :");
                request = Console.ReadLine();
                
                Console.WriteLine("");

                if (IsWellFormed(request, requests))
                {
                    if (request.Equals("exit"))
                    {
                        break;
                    }

                    if (request.Equals("help"))
                    {
                        Console.WriteLine("exit : Ferme l'application");
                        Console.WriteLine("select cities (pour lister l'ensemble des villes");
                        Console.WriteLine("select stations -city : (exemple : select stations -Besancon)");
                        Console.WriteLine("select details -\"station\" -city : (exemple : select details -\"11 JEAN - CORNET\" -Besancon)");
                    }

                    char[] split = { ' ' };
                    string[] param = request.Split(split);

                    if(request.StartsWith("select cities"))
                    {
                        PrintCities(client);
                    }

                    else if (request.StartsWith("select stations -"))
                    {
                        PrintStations(client, param[2].Replace("-", ""));
                        gotStations = true;
                    }

                    else if(request.StartsWith("select details -"))
                    {
                        char[] split2 = { '"' };

                        string station = "";

                        if (request.Contains("\""))
                        {
                            string[] stationParams = request.Split(split2);
                            station = stationParams[1];

                            string requestChanged = "";
                            requestChanged = request.Replace(station, "");
                            param = requestChanged.Split(split);
                        }

                        string city = param[3].Replace("-", "");

                        if (!gotStations)
                        {
                            client.GetStations(city);
                        }

                        PrintDetails(client, city, station);
                    }
                }

                else
                {
                    Console.WriteLine("Commande incorrecte. Veuillez réessayer");
                }
            }
        }

        private static void PrintCities(IntermediaryServiceClient.Service1Client client)
        {
            string[] cities = client.GetCities();

            if (cities != null)
            {
                foreach (string city in cities)
                {
                    Console.WriteLine(city);
                }
            }

            else
            {
                Console.WriteLine("Vérifiez votre connexion internet ou rafraichissez la page");
            }
        }

        private static void PrintStations(IntermediaryServiceClient.Service1Client client, string city)
        {
            try
            {
                string[] stations = client.GetStations(city);

                if (stations != null)
                {
                    foreach (string station in stations)
                    {
                        Console.WriteLine(station);
                    }
                }

                else
                {
                    Console.WriteLine("Vérifiez votre connexion internet ou rafraichissez la page");
                }
            }

            catch(Exception e)
            {
                Console.WriteLine("Vérifiez que vous avez bien entré le nom de la ville ou vérifiez votre connexion internet");
            }
        }

        private static void PrintDetails(IntermediaryServiceClient.Service1Client client, string city, string station)
        {
            try
            {
                string[] data = client.GetDetails(city, station, false);

                if (data != null)
                {
                    foreach (string d in data)
                    {
                        Console.WriteLine(d);
                    }
                }

                else
                {
                    Console.WriteLine("Vérifiez votre connexion internet ou rafraichissez la page");
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Vérifiez que vous avez bien entré le nom de la ville ou vérifiez votre connexion internet");
            }
        }

        private static bool IsWellFormed(string request, string[] requests)
        {
            bool wellFormed = false;

            foreach(string rqst in requests)
            {
                if(request.StartsWith(rqst))
                {
                    wellFormed = true;
                    break;
                }
            }

            if(!wellFormed)
            {
                return false;
            }
            
            char[] split = { ' ' };
            char[] split2 = { '"' };

            string[] param = request.Split(split);
            string station = null;
            string requestChanged = null;

            if(request.Contains("\""))
            {
                string[] stationParams = request.Split(split2);
                station = stationParams[1];
                requestChanged = request.Replace(station, "");
                param = requestChanged.Split(split);
            }

            if (request.Equals(requests[0]) || request.Equals(requests[1]) || request.Equals(requests[4]))
            {
                return true;
            }

            if(request.StartsWith(requests[2]) && param.Length == 3)
            {
                return true;
            }

            if (requestChanged != null && requestChanged.StartsWith(requests[3]) && param.Length == 4)
            {
                if(param[3].StartsWith("-") && station.Contains(" - ") && !station.StartsWith(" ") && !station.EndsWith(" "))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
