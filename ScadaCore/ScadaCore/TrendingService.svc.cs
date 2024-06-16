using ScadaCore.processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrendingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TrendingService.svc or TrendingService.svc.cs at the Solution Explorer and start debugging.
    public class TrendingService : ITrendingService
    {
        static ICallback callback = null;
        public void Init()
        {
            callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            TagProcessing.OnMessageArrived += callback.MessageArrived;
        }
    }
}
