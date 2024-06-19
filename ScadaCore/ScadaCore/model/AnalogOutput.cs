using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.model
{
    [DataContract]
    public class AnalogOutput:Tag
    {
        [DataMember]
        public int Value { get; set; }
        [DataMember]
        public int LowLimit { get; set; }
        [DataMember]
        public int HighLimit { get; set; }
        [DataMember]
        public string UnitsName { get; set; }

        public AnalogOutput() { }

        public AnalogOutput(string tagName, string description,string address,int initialValue, int lowLimit, int highLimit, string units)
        {
            TagName = tagName;
            Description = description;
            Address = address;
            Value = initialValue;
            LowLimit = lowLimit;
            HighLimit = highLimit;
            UnitsName = units;
        }

    }
}