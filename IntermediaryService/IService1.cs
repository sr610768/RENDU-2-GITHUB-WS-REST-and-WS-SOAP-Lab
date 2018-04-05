using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace IntermediaryService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string[] GetCities();

        [OperationContract]
        string[] GetStations(string city);

        [OperationContract]
        List<string> GetDetails(string city, string station, bool refresh);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetCities(AsyncCallback callback, object state);
        string[] EndGetCities(IAsyncResult asyncResult);
        
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetStations(string city, AsyncCallback callback, object state);
        string[] EndGetStations(IAsyncResult asyncResult);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetDetails(string city, string station, bool refresh, AsyncCallback callback, object state);
        List<string> EndGetDetails(IAsyncResult asyncResult);
    }
}
