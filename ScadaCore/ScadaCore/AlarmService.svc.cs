using ScadaCore.processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    public class AlarmService : IAlarmService
    {
        static IAlarmCallback callback = null;
        public void Init()
        {
            callback = OperationContext.Current.GetCallbackChannel<IAlarmCallback>();
            TagProcessing.OnAlarmMessageArrived += callback.MessageArrived;
        }
    }
}
