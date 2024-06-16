using ScadaCore.model;
using ScadaCore.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ScadaCore.processing
{
    public static class TagProcessing
    {
        public static Dictionary<string, Tag> inputTags = new Dictionary<string, Tag>();
        public static Dictionary<string, Tag> outputTags = new Dictionary<string, Tag>();
        public static Dictionary<string,Thread> tagThreads=new Dictionary<string, Thread>();

        public delegate void MessageArrivedDelegate(string message);
        public static event MessageArrivedDelegate OnMessageArrived;

        public static bool ContainsTag(string name)
        {
            if (inputTags.ContainsKey(name) || outputTags.ContainsKey(name))
            {
                return true;
            }
            return false;
        }

        public static bool AddDigitalInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn)
        {
            if (ContainsTag(name))
            {
                return false;
            }
            Thread t=null;
            DigitalInput tag = new DigitalInput(name, description, driver, address, scanTime, scanOn);
            inputTags[name] = tag;
            if (tag.Driver == 0)   //real time driver
            {
                if (!RealTimeDriver.DriverValues.ContainsKey(address))
                {
                    RealTimeDriver.SetValue(address, 0);
                }
            }
            t = new Thread(SimualteValuesDigitalInput);
            tagThreads[tag.TagName] = t;
            t.Start(tag);

            return true;
        }

        private static void SimualteValuesDigitalInput(object tag)
        {
            DigitalInput digitalTag = (DigitalInput)tag;
            double value = 0;

            while (true)
            {
                if (digitalTag.ScanOn)
                {
                    if (digitalTag.Driver == 0)
                    {
                        value = RealTimeDriver.DriverValues[digitalTag.Address];
                    }
                    else
                    {
                        value = SimulationDriver.ReturnValue(digitalTag.Address);

                    }
                    OnMessageArrived?.Invoke($"Value of {digitalTag.TagName} tag is: {value}");

                    Thread.Sleep(digitalTag.ScanTime * 1000);
                }
            }  
        }

        public static bool AddAnalogInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn, int lowLimit, int hightLimit, string units)
        {
            if (ContainsTag(name))
            {
                return false;
            }
            AnalogInput tag = new AnalogInput(name, description, address, driver, scanTime, scanOn, new List<Alarm>(), lowLimit, hightLimit, units);
            inputTags[name] = tag;
            return true;
        }

        public static bool AddDigitalOutputTag(string name, string description, string address, int initialValue)
        {
            if (ContainsTag(name))
            {
                return false;
            }
            DigitalOutput tag = new DigitalOutput(name, description, address, initialValue);
            outputTags[name] = tag;
            TagProcessing.AddDigitalOutputTag(tag, initialValue);
            return true;
        }

        public static bool AddAnalogOutputTag(string name, string description, string address, int initialValue, int lowLimit, int hightLimit, string units)
        {
            if (ContainsTag(name))
            {
                return false;
            }
            AnalogOutput tag = new AnalogOutput(name, description, address, initialValue, lowLimit, hightLimit, units);
            outputTags[name] = tag;
            TagProcessing.AddAnalogOutputTag(tag, initialValue);
            return true;
        }

        public static bool TurnOnScan(string name)
        {
            if (!ContainsTag(name))
            {
                return false;
            }
            Tag tag = inputTags[name];
            if (tag is DigitalInput)
            {
                DigitalInput digitalInput = (DigitalInput)tag;
                digitalInput.ScanOn = true;
            }
            else
            {
                AnalogInput analogInput = (AnalogInput)tag;
                analogInput.ScanOn = true;
            }
            return true;

        }

        public static bool TurnOffScan(string name)
        {
            if (!ContainsTag(name))
            {
                return false;
            }
            Tag tag = inputTags[name];
            if (tag is DigitalInput)
            {
                DigitalInput digitalInput = (DigitalInput)tag;
                digitalInput.ScanOn = false;
            }
            else
            {
                AnalogInput analogInput = (AnalogInput)tag;
                analogInput.ScanOn = false;
            }
            return true;
        }

        public static bool RemoveInputTag(string name)
        {
            if (!ContainsTag(name))
            {
                return false;
            }
            inputTags.Remove(name);
            return true;
        }

        public static bool RemoveOutputTag(string name)
        {
            if (!ContainsTag(name))
            {
                return false;
            }
            outputTags.Remove(name);
            return true;
        }

        public static Dictionary<string, double> GetDigitalOutputTags()
        {
            Dictionary<string, double> digitalTags = new Dictionary<string, double>();
            foreach (Tag tag in outputTags.Values)
            {
                if (tag is DigitalOutput)
                {
                    DigitalOutput digitalOutput = (DigitalOutput)tag;
                    digitalTags[digitalOutput.TagName] = digitalOutput.Value;
                }
            }
            return digitalTags;
        }

        public static Dictionary<string, double> GetAnalogOutputTags()
        {
            Dictionary<string, double> analoglTags = new Dictionary<string, double>();
            foreach (Tag tag in outputTags.Values)
            {
                if (tag is AnalogOutput)
                {
                    AnalogOutput analogOutput = (AnalogOutput)tag;
                    analoglTags[analogOutput.TagName] = analogOutput.Value;
                }
            }
            return analoglTags;
        }

        public static bool ChangeValueDigitalOutputTag(string name, int newValue)
        {
            if (!ContainsTag(name))
            {
                return false;
            }

            Tag tag = outputTags[name];
            DigitalOutput digitalOutput = (DigitalOutput)tag;
            digitalOutput.Value = newValue;
            TagProcessing.AddDigitalOutputTag(digitalOutput, newValue);
            return true;
        }

        public static bool ChangeValueAnalogOutputTag(string name, int newValue)
        {
            if (!ContainsTag(name))
            {
                return false;
            }

            Tag tag = outputTags[name];
            AnalogOutput analogOutput = (AnalogOutput)tag;
            analogOutput.Value = newValue;
            TagProcessing.AddAnalogOutputTag(analogOutput, newValue);
            return true;
        }



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