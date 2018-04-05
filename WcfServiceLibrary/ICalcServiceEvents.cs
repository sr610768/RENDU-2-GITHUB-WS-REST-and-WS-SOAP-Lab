using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    interface ICalcServiceEvents
    {
        [OperationContract(IsOneWay = true)]
        void LoadDetailsFromServer(string city, string station, string result);

        [OperationContract(IsOneWay = true)]
        void CalculationFinished();
    }
}
