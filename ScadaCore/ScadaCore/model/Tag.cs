using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ScadaCore.model
{
    [DataContract]
    public class Tag
    {
        [DataMember]
        public string TagName { get; set; }
        [DataMember]
        public string Description { get; set; }
 
        [DataMember]
        public string Address { get; set; }
    }
}