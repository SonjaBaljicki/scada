using ScadaCore.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    [ServiceContract]
    public interface IReportService
    {
        [OperationContract]
        List<AlarmEntity> GetAlarmsByDateRange(DateTime startDate, DateTime endDate);

        [OperationContract]
        List<AlarmEntity> GetAlarmsByPriority(int priority);

        [OperationContract]
        List<TagEntity> GetTagsByDateRange(DateTime startDate, DateTime endDate);

        [OperationContract]
        List<TagEntity> GetLatestAnalogInputTags();

        [OperationContract]
        List<TagEntity> GetLatestDigitalInputTags();

        [OperationContract]
        List<TagEntity> GetTagValuesByIdentifier(string tagName);
    }
}
