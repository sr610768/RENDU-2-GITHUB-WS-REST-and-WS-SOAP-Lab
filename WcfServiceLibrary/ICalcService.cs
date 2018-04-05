using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    [ServiceContract(CallbackContract = typeof(ICalcServiceEvents))]
    public interface ICalcService
    {
        [OperationContract]
        void LoadDetailsFromServer(string city, string station);

        [OperationContract]
        void SubscribeCalculatedEvent();

        [OperationContract]
        void SubscribeCalculationFinishedEvent();
    }
}
