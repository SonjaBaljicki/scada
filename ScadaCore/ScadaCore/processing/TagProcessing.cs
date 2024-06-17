using ScadaCore.model;
using ScadaCore.model.enums;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace ScadaCore.processing
{
    public static class TagProcessing
    {
        public static Dictionary<string, Tag> inputTags = new Dictionary<string, Tag>();
        public static Dictionary<string, Tag> outputTags = new Dictionary<string, Tag>();
        public static Dictionary<int, Alarm> alarms = new Dictionary<int, Alarm>();
        public static Dictionary<string,Thread> tagThreads=new Dictionary<string, Thread>();

        public delegate void MessageArrivedDelegate(string message);
        public static event MessageArrivedDelegate OnMessageArrived;
        public static event MessageArrivedDelegate OnAlarmMessageArrived;


        public static bool CheckAlarmId(int id)
        {
            if (alarms.ContainsKey(id))
            {
                return true;
            }
            return false;
        }

        public static bool ContainsAnalogInputTag(string name)
        {
            if (inputTags.ContainsKey(name) && inputTags[name] is AnalogInput)
            {
                return true;
            }
            return false;
        }

        public static bool ContainsDigitalInputTag(string name)
        {
            if (inputTags.ContainsKey(name) && inputTags[name] is DigitalInput)
            {
                return true;
            }
            return false;
        }

        public static bool ContainsAnalogOutputTag(string name)
        {
            if (outputTags.ContainsKey(name) && outputTags[name] is AnalogOutput)
            {
                return true;
            }
            return false;
        }

        public static bool ContainsDigitalOutputTag(string name)
        {
            if (outputTags.ContainsKey(name) && outputTags[name] is DigitalOutput)
            {
                return true;
            }
            return false;
        }

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
            t.IsBackground= true;
            tagThreads[tag.TagName] = t;
            t.Start(tag);

            return true;
        }
        public static bool AddAnalogInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn, int lowLimit, int hightLimit, string units)
        {
            if (ContainsTag(name))
            {
                return false;
            }
            Thread t = null;
            AnalogInput tag = new AnalogInput(name, description, address, driver, scanTime, scanOn, new List<Alarm>(), lowLimit, hightLimit, units);
            inputTags[name] = tag;
            if (tag.Driver == 0)   //real time driver
            {
                if (!RealTimeDriver.DriverValues.ContainsKey(address))
                {
                    RealTimeDriver.SetValue(address, 0);
                }
            }
            t = new Thread(SimualteValuesAnalogInput);
            t.IsBackground = true;
            tagThreads[tag.TagName] = t;
            t.Start(tag);
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
            return true;
        }

        public static void AddAnalogAlarm(string tagName, int id, AlarmType type, int priority, double edgeValue, string units)
        {
            Alarm alarm = new Alarm(id, type, priority, edgeValue, units);
            AnalogInput analogInputTag = (AnalogInput)inputTags[tagName];
            analogInputTag.Alarms.Add(alarm);
            alarms.Add(id, alarm);
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
            if (!ContainsAnalogInputTag(name) && !ContainsDigitalInputTag(name))
            {
                return false;
            }
            inputTags.Remove(name);
            return true;
        }

        public static bool RemoveOutputTag(string name)
        {
            if (!ContainsAnalogOutputTag(name) && !ContainsDigitalOutputTag(name))
            {
                return false;
            }
            outputTags.Remove(name);
            return true;
        }

        public static bool RemoveAlarm(int id)
        {
            bool check = false;
            foreach (Tag tag in inputTags.Values)
            {
                if (tag is AnalogInput) {
                    AnalogInput analogInput = (AnalogInput)tag;

                    if (analogInput.Alarms.Contains(alarms[id]))
                    {
                        check = true;
                        analogInput.Alarms.Remove(alarms[id]);
                        alarms.Remove(id);
                        break;
                    }
                }
            }
            return check;
        }

        public static Dictionary<string, int> GetDigitalOutputTags()
        {
            Dictionary<string, int> digitalTags = new Dictionary<string, int>();
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

        public static Dictionary<string, int> GetAnalogOutputTags()
        {
            Dictionary<string, int> analoglTags = new Dictionary<string, int>();
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
            if (!ContainsDigitalOutputTag(name))
            {
                return false;
            }

            Tag tag = outputTags[name];
            DigitalOutput digitalOutput = (DigitalOutput)tag;
            digitalOutput.Value = newValue;
            //AddDigitalOutputTagDB(digitalOutput, newValue);
            return true;
        }

        public static bool ChangeValueAnalogOutputTag(string name, int newValue)
        {
            if (!ContainsAnalogOutputTag(name))
            {
                return false;
            }

            Tag tag = outputTags[name];
            AnalogOutput analogOutput = (AnalogOutput)tag;
            if (analogOutput.LowLimit > newValue || analogOutput.HighLimit < newValue)
            {
                return false;
            }
            analogOutput.Value = newValue;
            //AddAnalogOutputTagDB(analogOutput, newValue);
            return true;
        }



        public static void AddDigitalInputTagDB(DigitalInput tag,int value)
       {
            using(DatabaseContext dbContext = new DatabaseContext())
            {
                TagEntity tagEntity = new TagEntity(tag.TagName, TagType.DIGITAL_INPUT, new DateTime(), value);
                dbContext.tags.Add(tagEntity);
                dbContext.SaveChanges();
            }
        }
        public static void AddAnalogInputTagDB(AnalogInput tag, int value)
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

                        if (value < 0.5) value = 0;
                        else value = 1;
                    }
                    else
                    {
                        value = SimulationDriver.ReturnValue(digitalTag.Address);

                        if (value <= 0) value = 0;
                        else value = 1;

                    }
                    OnMessageArrived?.Invoke($"Value of {digitalTag.TagName} tag is: {value}");

                    Thread.Sleep(digitalTag.ScanTime * 1000);
                }
                if (!inputTags.ContainsKey(digitalTag.TagName))
                {
                    break;
                }
            }
        }
        private static void SimualteValuesAnalogInput(object tag)
        {
            AnalogInput analogTag = (AnalogInput)tag;
            double value = 0;

            while (true)
            {
                if (analogTag.ScanOn)
                {
                    if (analogTag.Driver == 0)
                    {
                        value = RealTimeDriver.DriverValues[analogTag.Address];
                    }
                    else
                    {
                        value = SimulationDriver.ReturnValue(analogTag.Address);
                    }

                    if (value < analogTag.LowLimit) value = analogTag.LowLimit;
                    else if (value > analogTag.HighLimit) value = analogTag.HighLimit;

                    string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "processing/alarms/alarmsLog.txt";

                    foreach (Alarm alarm in analogTag.Alarms)
                    {
                        if (alarm.Type == AlarmType.LOW && alarm.EdgeValue > value)
                        {
                            DateTime now = DateTime.Now;
                            for (int i = 0; i < alarm.Priority; i++)
                            {
                                OnAlarmMessageArrived?.Invoke($"LOW VALUE of {analogTag.TagName}: {value}{alarm.UnitsName}\n at {now}");
                            }
                            using (StreamWriter streamWriter = File.AppendText(path))
                            {
                                streamWriter.WriteLine($"LOW VALUE ALARM with priority {alarm.Priority} of {analogTag.TagName}: {value}{alarm.UnitsName} at {now}");
                            }
                        } else if (alarm.Type == AlarmType.HIGH && alarm.EdgeValue < value)
                        {
                            DateTime now = DateTime.Now;
                            for (int i = 0; i < alarm.Priority; i++)
                            {
                                OnAlarmMessageArrived?.Invoke($"HIGH VALUE of {analogTag.TagName}: {value}{alarm.UnitsName}\n at {now}");
                            }
                            using (StreamWriter streamWriter = File.AppendText(path))
                            {
                                streamWriter.WriteLine($"HIGH VALUE ALARM with priority {alarm.Priority} of {analogTag.TagName}: {value}{alarm.UnitsName} at {now}");
                            }
                        }
                    }

                    OnMessageArrived?.Invoke($"Value of {analogTag.TagName} tag is: {value}");

                    Thread.Sleep(analogTag.ScanTime * 1000);
                }
                if (!inputTags.ContainsKey(analogTag.TagName))
                {
                    break;
                }
            }
        }
        public static void StopTagThreads()
        {
            foreach (var value in tagThreads)
            {
                value.Value.Join();
            }
        }
    }
}