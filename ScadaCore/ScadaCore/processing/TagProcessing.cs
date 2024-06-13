using ScadaCore.model;
using ScadaCore.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.processing
{
    public static class TagProcessing
    {
       public static void AddDigitalInputTag(DigitalInput tag,int value)
       {
            using(DatabaseContext dbContext = new DatabaseContext())
            {
                TagEntity tagEntity = new TagEntity(tag.TagName, TagType.DIGITAL_INPUT, new DateTime(), value);
                dbContext.tags.Add(tagEntity);
                dbContext.SaveChanges();
            }
        }
        public static void AddAnalogInputTag(AnalogInput tag, int value)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                TagEntity tagEntity = new TagEntity(tag.TagName, TagType.ANALOG_INPUT, new DateTime(), value);
                dbContext.tags.Add(tagEntity);
                dbContext.SaveChanges();
            }
        }
        public static void AddDigitalOutputTag(DigitalOutput tag, int value)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                TagEntity tagEntity = new TagEntity(tag.TagName, TagType.DIGITAL_OUTPUT, new DateTime(), value);
                dbContext.tags.Add(tagEntity);
                dbContext.SaveChanges();
            }
        }
        public static void AddAnalogOutputTag(AnalogOutput tag, int value)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                TagEntity tagEntity = new TagEntity(tag.TagName, TagType.ANALOG_OUTPUT, new DateTime(), value);
                dbContext.tags.Add(tagEntity);
                dbContext.SaveChanges();
            }
        }

    }
}