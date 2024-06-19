using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface ITrendingService
    {
        [OperationContract(IsOneWay = true)]
        void Init();

    }
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void MessageArrived(string message);
    }
}
