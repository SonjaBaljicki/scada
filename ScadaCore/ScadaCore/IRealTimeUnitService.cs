using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    [ServiceContract]
    public interface IRealTimeUnitService
    {
        [OperationContract]
        bool CreateRealTimeUnit(int id, string address);

        [OperationContract]
        bool SetValue(string message, string id, byte[] signature);
    }
}
