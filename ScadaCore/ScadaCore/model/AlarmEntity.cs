using ScadaCore.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScadaCore.model
{
    public class AlarmEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public AlarmType Type { get; set; }
        public int Priority { get; set; }
        public string TagName { get; set; }
        public DateTime Date { get; set; }

        public AlarmEntity() { }

        public AlarmEntity(AlarmType type, int priority, string tagName,DateTime date)
        {
            Type = type;
            Priority = priority;
            TagName = tagName;
            Date = date;
        }
    }
}