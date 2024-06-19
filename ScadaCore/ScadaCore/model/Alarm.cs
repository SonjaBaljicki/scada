using ScadaCore.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.model
{
    [DataContract]
    public class Alarm
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public AlarmType Type { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public double EdgeValue { get; set; }
        [DataMember]
        public string UnitsName { get; set; }
        public Alarm() { }

        public Alarm(int id, AlarmType type, int priority, double value, string units)
        {
            Id = id;
            Type = type;
            Priority = priority;
            EdgeValue = value;
            UnitsName = units;
        }
    }
}