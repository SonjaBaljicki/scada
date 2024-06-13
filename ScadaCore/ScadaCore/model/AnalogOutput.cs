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
        public int InitialValue { get; set; }
        [DataMember]
        public int LowLimit { get; set; }
        [DataMember]
        public int HighLimit { get; set; }
        [DataMember]
        public int Units { get; set; }

        public AnalogOutput() { }

        public AnalogOutput(string tagName, string description,string address,int initialValue, int lowLimit, int highLimit, int units)
        {
            TagName = tagName;
            Description = description;
            Address = address;
            InitialValue = initialValue;
            LowLimit = lowLimit;
            HighLimit = highLimit;
            Units = units;
        }

    }
}