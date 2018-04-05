using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace IntermediaryService
{
    public class Service1 : IService1
    {
        private string apiKey;
        private Cache cache;
        private JArray tempData;

        public const int DelayMilliseconds = 10000;

        public Service1()
        {
            apiKey = "5adfd2b9c75749261a8f2640290caad53559518a";

            string[] cities = LoadCitiesFromServer();

            tempData = null;

            if (cities == null)
            {
                cache = null;
            }

            else
            {
                cache = new Cache(cities);
            }
        }

        public string[] GetCities()
        {
            string[] cities;

            if (cache == null)
            {
                cities = LoadCitiesFromServer();

                if (cities == null)
                {
                    return null;
                }

                else
                {
                    cache = new Cache(cities);
                }
            }

            cities = cache.GetCities();
            return cities;
        }

        public string[] GetStations(string city)
        {
            string[] stations;

            try
            {
                if (cache.GetStations(city).Length == 0)
                {
                    stations = LoadStationsFromServer(city);

                    if (stations == null)
                    {
                        return null;
                    }

                    else
                    {
                        cache.UpdateStations(city, stations);
                    }
                }

                stations = cache.GetStations(city);
                return stations;
            }

            catch(Exception e)
            {
                return null;
            }
        }

        private string[] LoadCitiesFromServer()
        {
            string url = "https://api.jcdecaux.com/vls/v1/contracts?apiKey=" + apiKey;

            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();

                if (response == null)
                {
                    return null;
                }

                else
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);

                    string responseFromServer = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                    JArray data = JArray.Parse(responseFromServer);

                    string[] cities = new string[data.Count];

                    int i = 0;

                    foreach (JObject item in data)
                    {
                        string name = (string)item.SelectToken("name");
                        cities[i++] = name;
                    }

                    return cities;
                }
            }

            catch (Exception exception)
            {
                return null;
            }
        }

        private void LoadDataFromServer(string city)
        {
            string url = "https://api.jcdecaux.com/vls/v1/stations?contract=" + city + "&apiKey=" + apiKey;
            
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;

                WebResponse response = request.GetResponse();

                if (response == null)
                {
                    tempData = null;
                }

                else
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);

                    string responseFromServer = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                    /* PARSING JSON */

                    JArray data = JArray.Parse(responseFromServer);

                    tempData = data;
                }
            }

            catch (Exception exception)
            {
                tempData = null;
            }
        }

        private string[] LoadStationsFromServer(string city)
        {
            LoadDataFromServer(city);

            if (tempData == null)
            {
                return null;
            }

            string[] stations = new string[tempData.Count];

            int i = 0;

            foreach (JObject item in tempData)
            {
                string name = (string) item.SelectToken("name");
                stations[i++] = name;
            }

            return stations;
        }

        public List<string> GetDetails(string city, string station, bool refresh)
        {
            List<string> details;

            if (cache.GetStationDetails(city, station) == null)
            {
                details = LoadDetailsFromServer(city, station);

                if (details == null)
                {
                    return null;
                }

                else
                {
                    cache.UpdateStation(city, station, details);
                }
            }

            else
            {
                if (refresh)
                {
                    details = LoadDetailsFromServer(city, station);

                    if (details != null)
                    {
                        cache.UpdateStation(city, station, details);
                    }
                }
            }

            details = cache.GetStationDetails(city, station);
            return details;
        }

        public List<string> LoadDetailsFromServer(string city, string station)
        {
            if (tempData == null)
            {
                return null;
            }

            List<string> details = new List<string>();

            foreach (JObject item in tempData)
            {
                string name = (string)item.SelectToken("name");

                if (name.Contains(station))
                {
                    details.Add("Nombre de vélos disponibles : " + (item.SelectToken("available_bikes")));

                    // Optionnel :
                    /*
                    details.Add("");
                    details.Add("");
                    details.Add("Autres données facultatives non demandées dans l'énoncé :");
                    details.Add("");
                    details.Add("number : " + (item.SelectToken("number")));
                    details.Add("address : " + (item.SelectToken("address")));
                    details.Add("position : " + (item.SelectToken("position")));
                    details.Add("banking : " + (item.SelectToken("banking")));
                    details.Add("bonus : " + (item.SelectToken("bonus")));
                    details.Add("status : " + (item.SelectToken("status")));
                    details.Add("bike_stands : " + (item.SelectToken("bike_stands")));
                    details.Add("available_bike_stands : " + (item.SelectToken("available_bike_stands")));
                    details.Add("last_update : " + (item.SelectToken("last_update")));*/

                    return details;
                }
            }

            return null;
        }
        
        public IAsyncResult BeginGetCities(AsyncCallback callback, object state)
        {
            var asyncResult = new SimpleAsyncResult<string[]>(state);
            
            // mimic a long running operation
            var timer = new System.Timers.Timer(DelayMilliseconds);
            timer.Elapsed += (_, args) =>
            {
                asyncResult.Result = GetCities();
                asyncResult.IsCompleted = true;
                callback(asyncResult);
                timer.Enabled = false;
                timer.Close();
            };
            timer.Enabled = true;
            return asyncResult;
        }

        public string[] EndGetCities(IAsyncResult asyncResult)
        {
            return ((SimpleAsyncResult<string[]>)asyncResult).Result;
        }

        public IAsyncResult BeginGetStations(string city, AsyncCallback callback, object state)
        {
            var asyncResult = new SimpleAsyncResult<string[]>(state);

            // mimic a long running operation
            var timer = new System.Timers.Timer(DelayMilliseconds);
            timer.Elapsed += (_, args) =>
            {
                asyncResult.Result = GetStations(city);
                asyncResult.IsCompleted = true;
                callback(asyncResult);
                timer.Enabled = false;
                timer.Close();
            };
            timer.Enabled = true;
            return asyncResult;
        }

        public string[] EndGetStations(IAsyncResult asyncResult)
        {
            return ((SimpleAsyncResult<string[]>)asyncResult).Result;
        }

        public IAsyncResult BeginGetDetails(string city, string station, bool refresh, AsyncCallback callback, object state)
        {
            var asyncResult = new SimpleAsyncResult<List<string>>(state);

            // mimic a long running operation
            var timer = new System.Timers.Timer(DelayMilliseconds);
            timer.Elapsed += (_, args) =>
            {
                asyncResult.Result = GetDetails(city, station, refresh);
                asyncResult.IsCompleted = true;
                callback(asyncResult);
                timer.Enabled = false;
                timer.Close();
            };
            timer.Enabled = true;
            return asyncResult;
        }

        public List<string> EndGetDetails(IAsyncResult asyncResult)
        {
            return ((SimpleAsyncResult<List<string>>)asyncResult).Result;
        }
    } 

    public class SimpleAsyncResult<T> : IAsyncResult
    {
        private readonly object accessLock = new object();
        private bool isCompleted = false;
        private T result;

        public SimpleAsyncResult(object asyncState)
        {
            AsyncState = asyncState;
        }

        public T Result
        {
            get
            {
                lock (accessLock)
                {
                    return result;
                }
            }
            set
            {
                lock (accessLock)
                {
                    result = value;
                }
            }
        }

        public bool IsCompleted
        {
            get
            {
                lock (accessLock)
                {
                    return isCompleted;
                }
            }
            set
            {
                lock (accessLock)
                {
                    isCompleted = value;
                }
            }
        }
        
        public WaitHandle AsyncWaitHandle { get { return null; } }
        
        public bool CompletedSynchronously { get { return false; } }

        public object AsyncState { get; private set; }

        WaitHandle IAsyncResult.AsyncWaitHandle => throw new NotImplementedException();
    }
}