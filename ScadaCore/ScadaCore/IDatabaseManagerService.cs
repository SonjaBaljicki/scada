using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaCore
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDatabaseManagerService" in both code and config file together.
    [ServiceContract]
    public interface IDatabaseManagerService
    {
        [OperationContract]
        bool Registration(string username, string password);
        [OperationContract]
        string Login(string username, string password);
        [OperationContract]
        bool LogOut(string token);
        [OperationContract]
        bool DatabaseEmpty();
    }
}
