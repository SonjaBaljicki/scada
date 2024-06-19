using ScadaCore.model;
using ScadaCore.model.enums;
using ScadaCore.processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ScadaCore
{
    public class DatabaseManagerService : IDatabaseManagerService
    {

        public string Registration(string username, string password)
        {
            return UserProcessing.Registration(username, password);
        }

        public string Login(string username, string password)
        {
            return UserProcessing.LogIn(username, password);
        }

        public bool LogOut(string token)
        {
            return UserProcessing.LogOut(token);
        }

        public bool DatabaseEmpty()
        {
            using (var db = new DatabaseContext())
            {
                if (db.users.Count() == 0)
                {
                    return true;
                }
                return false;
            }
        }

        public void StopTagThreads()
        {
            TagProcessing.StopTagThreads();
        }
        public bool CheckTagName(string name) 
        {
            return TagProcessing.ContainsTag(name);
        }

        public bool AddDigitalInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn, string token)
        {
            if (UserProcessing.CheckToken(token))
            {
                return TagProcessing.AddDigitalInputTag(name, description, address, driver, scanTime, scanOn);
            }
            return false;
        }

        public bool AddAnalogInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn, int lowLimit, int hightLimit, string units, string token)
        {
            if (UserProcessing.CheckToken(token))
            {
                return TagProcessing.AddAnalogInputTag(name, description, address, driver, scanTime, scanOn, lowLimit, hightLimit, units);
            }
            return false;
        }

        public bool AddDigitalOutputTag(string name, string description, string address, int initialValue, string token)
        {
                if (UserProcessing.CheckToken(token))
                {
                    return TagProcessing.AddDigitalOutputTag(name, description, address, initialValue);
            }
            return false;
        }

        public bool AddAnalogOutputTag(string name, string description, string address, int initialValue, int lowLimit, int hightLimit, string units, string token)
        {
                    if (UserProcessing.CheckToken(token))
                    {
                        return TagProcessing.AddAnalogOutputTag(name, description, address, initialValue, lowLimit, hightLimit, units);
            }
            return false;
        }

        public bool TurnOnScan(string name, string token)
        {
                        if (UserProcessing.CheckToken(token))
                        {
                            return TagProcessing.TurnOnScan(name);
            }
            return false;
        }

        public bool TurnOffScan(string name, string token)
        {
                            if (UserProcessing.CheckToken(token))
                            {
                                return TagProcessing.TurnOffScan(name);
            }
            return false;
        }

        public bool RemoveInputTag(string name, string token)
        {
                                if (UserProcessing.CheckToken(token))
                                {
                                    return TagProcessing.RemoveInputTag(name);
            }
            return false;
        }

        public bool RemoveOutputTag(string name, string token)
        {
                                    if (UserProcessing.CheckToken(token))
                                    {
                                        return TagProcessing.RemoveOutputTag(name);
            }
            return false;
        }

        public bool RemoveAlarm(int id, string token)
        {
                                        if (UserProcessing.CheckToken(token))
                                        {
                                            return TagProcessing.RemoveAlarm(id);
            }
            return false;
        }

        public Dictionary<string, int> GetDigitalOutputTags(string token)
        {
                                            if (UserProcessing.CheckToken(token))
                                            {
                                                return TagProcessing.GetDigitalOutputTags();
            }
            return null;
        }

        public Dictionary<string, int> GetAnalogOutputTags(string token)
        {
                                                if (UserProcessing.CheckToken(token))
                                                {
                                                    return TagProcessing.GetAnalogOutputTags();
            }
            return null;
        }

        public bool ChangeValueDigitalOutputTag(string name, int newValue, string token)
        {
                                                    if (UserProcessing.CheckToken(token))
                                                    {
                                                        return TagProcessing.ChangeValueDigitalOutputTag(name, newValue);
            }
            return false;
        }

        public bool ChangeValueAnalogOutputTag(string name, int newValue, string token)
        {
                                                        if (UserProcessing.CheckToken(token))
                                                        {
                                                            return TagProcessing.ChangeValueAnalogOutputTag(name, newValue);
            }
            return false;
        }

        public bool AddAnalogAlarm(string tagName, int id, int type, int priority, double edgeValue, string units, string token)
        {
                                                            if (UserProcessing.CheckToken(token))
                                                            {
                                                                if (!CheckAlarmId(id))
            {
                AlarmType alarmType = type == 1 ? AlarmType.HIGH : AlarmType.LOW;
                TagProcessing.AddAnalogAlarm(tagName, id, alarmType, priority, edgeValue, units);
                return true;
            }
            return false;
            }
            return false;
        }

        public bool CheckAlarmId(int id)
        {
            return TagProcessing.CheckAlarmId(id);
        }

        public bool ContainsAnalogInputTag(string name)
        {
            return TagProcessing.ContainsAnalogInputTag(name);
        }

        public bool ContainsInputTag(string name)
        {
            return TagProcessing.ContainsAnalogInputTag(name) || TagProcessing.ContainsDigitalInputTag(name);
        }

        public bool ContainsOutputTag(string name)
        {
            return TagProcessing.ContainsAnalogOutputTag(name) || TagProcessing.ContainsDigitalOutputTag(name);
        }

        public void ReadFromConfig()
        {
            TagProcessing.ReadFromConfig();
        }
    }
}