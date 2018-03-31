using System;
using System.Collections.Generic;

namespace IntermediaryService
{
    class Cache
    {
        private string[] cities;
        private Dictionary<string, Dictionary<string, List<string>>> dictionary;

        public Cache(string[] cities)
        {
            dictionary = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (string city in cities)
            {
                AddInDictionnary(dictionary, city, new Dictionary<string, List<string>>());
            }

            this.cities = cities;
        }

        public string[] GetCities()
        {
            return cities;
        }

        public string[] GetStations(string city)
        {
            try
            {
                List<string> keyList = new List<string>(dictionary[city].Keys);
                return keyList.ToArray();
            }

            catch(Exception e)
            {
                return null;
            }
        }

        public void UpdateStations(string city, string[] stations)
        {
            foreach (string station in stations)
            {
                AddInDictionnary(dictionary[city], station, null);
            }
        }

        public void UpdateStation(string city, string station, List<string> details)
        {
            try
            {
                dictionary[city][station] = details;
            }

            catch(Exception e)
            {

            }
        }

        private void AddInDictionnary<Key, Element>(Dictionary<Key, Element> dictionary, Key key, Element element)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, element);
            }

            else
            {
                dictionary[key] = element;
            }
        }
        
        public bool ContainsStation(string city, string station)
        {
            if (dictionary.ContainsKey(city))
            {
                Dictionary<string, List<string>> stations = dictionary[city];

                if (stations.ContainsKey(station))
                {
                    return true;
                }
            }

            return false;
        }

        public List<string> GetStationDetails(string city, string station)
        {
            try
            {
                Dictionary<string, List<string>> stations = dictionary[city];
                return stations[station];
            }

            catch(Exception e)
            {
                return null;
            }
        }
    }
}
