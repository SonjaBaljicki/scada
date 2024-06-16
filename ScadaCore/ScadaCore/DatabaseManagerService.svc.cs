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

        public bool Registration(string username, string password)
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
            return TagProcessing.inputTags.ContainsKey(name);
        }

        public bool AddDigitalInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn)
        {
            return TagProcessing.AddDigitalInputTag(name, description, address, driver, scanTime, scanOn);
        }

        public bool AddAnalogInputTag(string name, string description, string address, int driver, int scanTime, bool scanOn, int lowLimit, int hightLimit, string units)
        {
            return TagProcessing.AddAnalogInputTag(name, description, address, driver, scanTime, scanOn, lowLimit, hightLimit, units);
        }

        public bool AddDigitalOutputTag(string name, string description, string address, int initialValue)
        {
            return TagProcessing.AddDigitalOutputTag(name, description, address, initialValue);
        }

        public bool AddAnalogOutputTag(string name, string description, string address, int initialValue, int lowLimit, int hightLimit, string units)
        {
            return TagProcessing.AddAnalogOutputTag(name, description, address, initialValue, lowLimit, hightLimit, units);
        }

        public bool TurnOnScan(string name)
        {
            return TagProcessing.TurnOnScan(name);    
        }

        public bool TurnOffScan(string name)
        {
            return TagProcessing.TurnOffScan(name);
        }

        public bool RemoveInputTag(string name)
        {
            return TagProcessing.RemoveInputTag(name);
        }

        public bool RemoveOutputTag(string name)
        {
            return TagProcessing.RemoveOutputTag(name);
        }

        public Dictionary<string, double> GetDigitalOutputTags()
        {
            return TagProcessing.GetDigitalOutputTags();
        }

        public Dictionary<string, double> GetAnalogOutputTags()
        {
            return TagProcessing.GetAnalogOutputTags();
        }

        public bool ChangeValueDigitalOutputTag(string name, int newValue)
        {
            return TagProcessing.ChangeValueDigitalOutputTag(name, newValue);
        }

        public bool ChangeValueAnalogOutputTag(string name, int newValue)
        {
            return TagProcessing.ChangeValueAnalogOutputTag(name, newValue);
        }

        public bool AddAnalogAlarm(string tagName, int id, int type, int priority, double edgeValue, string units)
        {
            if (!CheckAlarmId(id))
            {
                AlarmType alarmType = type == 1 ? AlarmType.HIGH : AlarmType.LOW;
                TagProcessing.AddAnalogAlarm(tagName, id, alarmType, priority, edgeValue, units);
                return true;
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
    }
}