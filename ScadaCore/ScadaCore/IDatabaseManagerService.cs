using System.Collections.Generic;
using System.ServiceModel;

namespace ScadaCore
{
    [ServiceContract]
    public interface IDatabaseManagerService
    {
        [OperationContract]
        string Registration(string username, string password);
        [OperationContract]
        string Login(string username, string password);
        [OperationContract]
        bool LogOut(string token);
        [OperationContract]
        bool DatabaseEmpty();
        [OperationContract]
        void StopTagThreads();
        [OperationContract]
        bool CheckTagName(string name);
        [OperationContract]
        bool AddDigitalInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn, string token);
        [OperationContract]
        bool AddAnalogInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn, int lowLimit, int hightLimit, string units, string token);
        [OperationContract]
        bool AddDigitalOutputTag(string name, string description, string address, int initialValue, string token);
        [OperationContract]
        bool AddAnalogOutputTag(string name, string description, string address, int initialValue, int lowLimit, int hightLimit, string units, string token);
        [OperationContract]
        bool AddAnalogAlarm(string tagName, int id, int type, int priority, double edgeValue, string units, string token);
        [OperationContract]
        bool CheckAlarmId(int id);
        [OperationContract]
        bool TurnOnScan(string name, string token);
        [OperationContract]
        bool TurnOffScan(string name, string token);
        [OperationContract]
        bool RemoveInputTag(string name, string token);
        [OperationContract]
        bool RemoveAlarm(int id, string token);
        [OperationContract]
        bool RemoveOutputTag(string name, string token);
        [OperationContract]
        Dictionary<string, int> GetDigitalOutputTags(string token);
        [OperationContract]
        Dictionary<string, int> GetAnalogOutputTags(string token);
        [OperationContract]
        bool ChangeValueDigitalOutputTag(string name, int newValue, string token);
        [OperationContract]
        bool ChangeValueAnalogOutputTag(string name, int newValue, string token);
        [OperationContract]
        bool ContainsAnalogInputTag(string name);
        [OperationContract]
        bool ContainsInputTag(string name);
        [OperationContract]
        bool ContainsOutputTag(string name);
        [OperationContract]
        void ReadFromConfig();
    }
}
