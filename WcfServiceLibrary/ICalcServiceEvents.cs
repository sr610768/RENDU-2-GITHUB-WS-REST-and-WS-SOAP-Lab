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
        void Calculated(int numberOfBikes, string city, string station);

        [OperationContract(IsOneWay = true)]
        void CalculationFinished();
    }
}
