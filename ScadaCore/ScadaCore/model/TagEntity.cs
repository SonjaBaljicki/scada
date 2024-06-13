using ScadaCore.model.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScadaCore.model
{
    public class TagEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TagName { get; set; }
        public TagType Type { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }

        public TagEntity() { }

        public TagEntity(string tagName, TagType type, DateTime date, int value)
        {
            TagName = tagName;
            Type = type;
            Date = date;
            Value = value;
        }

    }
}