using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.model
{
    [DataContract]
    [KnownType(typeof(List<Alarm>))]
    [KnownType(typeof(Alarm))]
    public class AnalogInput:Tag
    {
        [DataMember]
        public int Driver { get; set; }

        [DataMember]
        public int ScanTime { get; set; }
        [DataMember]
        public bool ScanOn { get; set; }
        [DataMember]
        public List<Alarm> Alarms { get; set; }
        [DataMember]
        public int LowLimit { get; set; }
        [DataMember]
        public int HighLimit { get; set; }
        [DataMember]
        public string UnitsName { get; set; }

        public AnalogInput() { }

        public AnalogInput(string tagName, string description, string address,int driver, int scanTime, bool scanOn, List<Alarm> alarms, int lowLimit, int highLimit, string units)
        {
            TagName = tagName;
            Description = description;
            Address = address;
            Driver = driver;
            ScanTime = scanTime;
            ScanOn = scanOn;
            Alarms = alarms;
            LowLimit = lowLimit;
            HighLimit = highLimit;
            UnitsName = units;
        }
    }
}