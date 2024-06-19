using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore
{
    public class RealTimeDriver
    {
        private static object lockDictAddress = new object();
        private static object lockDictValue = new object();

        public static Dictionary<int, string> RTUAddress = new Dictionary<int, string>();
        public static Dictionary<string, double> DriverValues = new Dictionary<string, double>();

        public static bool AddAddress(int id, string address)
        {
            if (RTUAddress.ContainsKey(id))
                return false;
            lock (lockDictAddress)
                RTUAddress[id] = address;
            return true;
        }

        public static bool SetValue(string address, double value)
        {
            lock (lockDictValue) 
                DriverValues[address] = value;
            return true;
        }
    }
}