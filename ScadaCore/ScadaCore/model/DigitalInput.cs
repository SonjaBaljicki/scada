using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.model
{
    [DataContract]
    public class DigitalInput:Tag
    {
        [DataMember]
        public int Driver { get; set; }
 
        [DataMember]
        public int ScanTime { get; set; }
        [DataMember]
        public bool ScanOn { get; set; }

        public DigitalInput(){}

        public DigitalInput(string tagName, string description, int driver, string address, int scanTime, bool scanOn)
        {
            TagName = tagName;
            Description = description;
            Driver = driver;
            Address = address;
            ScanTime = scanTime;
            ScanOn = scanOn;
        }
    }
}