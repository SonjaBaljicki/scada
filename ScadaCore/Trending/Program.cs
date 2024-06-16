using System;
using System.ServiceModel;


namespace Trending
{
    internal class Program
    {
        public class Callback:ServiceReference1.ITrendingServiceCallback
        {
            public void MessageArrived(string message)
            {
                Console.WriteLine(message);
            }
        }
        static void Main(string[] args)
        {
            InstanceContext ic = new InstanceContext(new Callback());
            ServiceReference1.TrendingServiceClient client = new ServiceReference1.TrendingServiceClient(ic);
            client.Init();
            Console.ReadKey();
        }


    }
}
