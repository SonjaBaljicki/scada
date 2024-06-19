using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    internal class Program
    {
        public class Callback : ServiceReference1.IAlarmServiceCallback
        {
            public void MessageArrived(string message)
            {
                Console.WriteLine(message);
            }
        }
        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new Callback());
            ServiceReference1.AlarmServiceClient client = new ServiceReference1.AlarmServiceClient(ic);
            client.Init();
            Console.ReadKey();
        }
    }
}
