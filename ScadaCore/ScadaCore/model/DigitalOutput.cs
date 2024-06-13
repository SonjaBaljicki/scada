using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.model
{
    [DataContract]
    public class DigitalOutput:Tag
    {
        [DataMember]
        public int Value { get; set; }   //0 is off and 1 is on

        public DigitalOutput() { }

        public DigitalOutput(string tagName, string description,string address, int value)
        {
            TagName = tagName;
            Description = description;
            Address = address;
            Value = value;
            
        }
        public bool IsTurnedOn()
        {
            return Value == 1;
        }

    }
}