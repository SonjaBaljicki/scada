using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    [ServiceContract(CallbackContract = typeof(IAlarmCallback))]
    public interface IAlarmService
    {
        [OperationContract(IsOneWay = true)]
        void Init();
    }

    public interface IAlarmCallback
    {
        [OperationContract(IsOneWay = true)]
        void MessageArrived(string message);
    }
}
