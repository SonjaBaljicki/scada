using ScadaCore.model;
using ScadaCore.model.enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

        private static object lockDatabase = new object();
        private static object lockConfig = new object();

        public static bool WriteToConfig()
        {
            XElement tags = new XElement("Tags");
            XElement inputs = new XElement("InputTags");
            XElement outputs = new XElement("OutputTags");
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "processing/config/ScadaConfig.xml";
            
            foreach (Tag tag in inputTags.Values)
            {
                if (tag is AnalogInput)
                {
                    AnalogInput analogInput = (AnalogInput)tag;
                    List<XElement> alarmElements = new List<XElement>();
                    foreach(Alarm a in analogInput.Alarms)
                    {
                        XElement alarm = new XElement("Alarm",
                            new XAttribute("Id", a.Id),
                            new XAttribute("Type", a.Type),
                            new XAttribute("Priority", a.Priority),
                            new XAttribute("EdgeValue", a.EdgeValue),
                            new XAttribute("UnitsName", a.UnitsName));
                        alarmElements.Add(alarm);
                    }
                    XElement analogInputElement = new XElement("AnalogInput", 
                        new XAttribute("TagName", analogInput.TagName), 
                        new XAttribute("Description", analogInput.Description),
                        new XAttribute("Address", analogInput.Address), 
                        new XAttribute("Driver", analogInput.Driver), 
                        new XAttribute("ScanTime", analogInput.ScanTime),
                        new XAttribute("ScanOn", analogInput.ScanOn),
                        new XAttribute("LowLimit", analogInput.LowLimit),
                        new XAttribute("HighLimit", analogInput.HighLimit));
                    analogInputElement.Add(alarmElements);
                    inputs.Add(analogInputElement);
                }
                else { 
                    DigitalInput digitalInput = (DigitalInput)tag;
                    XElement digitalInputElement = new XElement("AnalogInput",
                        new XAttribute("TagName", digitalInput.TagName),
                        new XAttribute("Description", digitalInput.Description),
                        new XAttribute("Address", digitalInput.Address),
                        new XAttribute("Driver", digitalInput.Driver),
                        new XAttribute("ScanTime", digitalInput.ScanTime),
                        new XAttribute("ScanOn", digitalInput.ScanOn));
                    inputs.Add(digitalInputElement);
                }
            }
            tags.Add(inputs);
            
            foreach (Tag tag in outputTags.Values)
            {
                if (tag is AnalogOutput)
                {
                    AnalogOutput analogOutput = (AnalogOutput)tag;
                    XElement analogOutputElement = new XElement("AnalogInput",
                        new XAttribute("TagName", analogOutput.TagName),
                        new XAttribute("Description", analogOutput.Description),
                        new XAttribute("Address", analogOutput.Address),
                        new XAttribute("Value", analogOutput.Value),
                        new XAttribute("UnitsName", analogOutput.UnitsName),
                        new XAttribute("LowLimit", analogOutput.LowLimit),
                        new XAttribute("HighLimit", analogOutput.HighLimit));
                    outputs.Add(analogOutputElement);
                }
                else
                {
                    DigitalOutput digitalOutput = (DigitalOutput)tag;
                    XElement digitalOutputElement = new XElement("AnalogInput",
                        new XAttribute("TagName", digitalOutput.TagName),
                        new XAttribute("Description", digitalOutput.Description),
                        new XAttribute("Address", digitalOutput.Address),
                        new XAttribute("Value", digitalOutput.Value));
                    outputs.Add(digitalOutputElement);
                }
            }
            tags.Add(outputs);
            
            lock(lockConfig)
                tags.Save(path);
            return true;
        }

        public static bool ReadFromConfig()
        {
            //procitamo input tagove
            //procitamo output tagove
            //napravimo dodatno i listu alarma
            //threads za svaki tag
            return true;
        }


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
            AddDigitalInputTagDB(tag, 0);
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

            WriteToConfig();
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
            AddAnalogInputTagDB(tag, 0);
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

            WriteToConfig();
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
            AddDigitalOutputTagDB(tag, initialValue);

            WriteToConfig();
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
            AddAnalogOutputTagDB(tag, initialValue);

            WriteToConfig();
            return true;
        }

        public static void AddAnalogAlarm(string tagName, int id, AlarmType type, int priority, double edgeValue, string units)
        {
            Alarm alarm = new Alarm(id, type, priority, edgeValue, units);
            AnalogInput analogInputTag = (AnalogInput)inputTags[tagName];
            analogInputTag.Alarms.Add(alarm);
            alarms.Add(id, alarm);

            WriteToConfig();
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

            WriteToConfig();
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

            WriteToConfig();
            return true;
        }

        public static bool RemoveInputTag(string name)
        {
            if (!ContainsAnalogInputTag(name) && !ContainsDigitalInputTag(name))
            {
                return false;
            }
            inputTags.Remove(name);

            WriteToConfig();
            return true;
        }

        public static bool RemoveOutputTag(string name)
        {
            if (!ContainsAnalogOutputTag(name) && !ContainsDigitalOutputTag(name))
            {
                return false;
            }
            outputTags.Remove(name);

            WriteToConfig();
            return true;
        }

        public static bool RemoveAlarm(int id)
        {
            bool check = false;
            foreach (Tag tag in inputTags.Values)
            {
                if (tag is AnalogInput) {
                    AnalogInput analogInput = (AnalogInput)tag;
                    if (alarms.ContainsKey(id))
                    {
                        if (analogInput.Alarms.Contains(alarms[id]))
                        {
                            check = true;
                            analogInput.Alarms.Remove(alarms[id]);
                            alarms.Remove(id);
                            break;
                        }
                    }
                }
            }

            WriteToConfig();
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
            AddDigitalOutputTagDB(digitalOutput, newValue);

            WriteToConfig();
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
            AddAnalogOutputTagDB(analogOutput, newValue);

            WriteToConfig();
            return true;
        }

        public static void AddAlarmDB(Alarm alarm, string tagName)
        {
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    AlarmEntity alarmEntity = new AlarmEntity(alarm.Type, alarm.Priority, tagName, DateTime.Now);
                    dbContext.alarms.Add(alarmEntity);
                    dbContext.SaveChanges();
                }
            }
        }

        public static void AddDigitalInputTagDB(DigitalInput tag,int value)
       {
            lock (lockDatabase)
            {
                using(DatabaseContext dbContext = new DatabaseContext())
                {
                    TagEntity tagEntity = new TagEntity(tag.TagName, TagType.DIGITAL_INPUT, DateTime.Now, value);
                    dbContext.tags.Add(tagEntity);
                    dbContext.SaveChanges();
                }
            }
            
        }
        public static void AddAnalogInputTagDB(AnalogInput tag, int value)
        {
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    TagEntity tagEntity = new TagEntity(tag.TagName, TagType.ANALOG_INPUT, DateTime.Now, value);
                    dbContext.tags.Add(tagEntity);
                    dbContext.SaveChanges();
                }
            }
        }
        public static void AddDigitalOutputTagDB(DigitalOutput tag, int value)
        {
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    TagEntity tagEntity = new TagEntity(tag.TagName, TagType.DIGITAL_OUTPUT, DateTime.Now, value);
                    dbContext.tags.Add(tagEntity);
                    dbContext.SaveChanges();
                }
            }
        }
        public static void AddAnalogOutputTagDB(AnalogOutput tag, int value)
        {
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    TagEntity tagEntity = new TagEntity(tag.TagName, TagType.ANALOG_OUTPUT, DateTime.Now, value);
                    dbContext.tags.Add(tagEntity);
                    dbContext.SaveChanges();
                }
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
                    AddDigitalInputTagDB(digitalTag, (int)value);

                    WriteToConfig();

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
                            AddAlarmDB(alarm, analogTag.TagName);
                        }
                        else if (alarm.Type == AlarmType.HIGH && alarm.EdgeValue < value)
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
                            AddAlarmDB(alarm, analogTag.TagName);
                        }
                    }

                    OnMessageArrived?.Invoke($"Value of {analogTag.TagName} tag is: {value}");
                    AddAnalogInputTagDB(analogTag, (int)value);

                    WriteToConfig();

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