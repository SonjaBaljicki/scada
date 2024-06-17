using ScadaCore.model;
using ScadaCore.model.enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScadaCore
{
    public class ReportService : IReportService
    {
        private static object lockDatabase = new object();

        public ReportService() { }

        public List<AlarmEntity> GetAlarmsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<AlarmEntity> alarms = null;
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    alarms = dbContext.alarms.ToList();
                }
            }
            return alarms
                .Where(a => a.Date >= startDate && a.Date <= endDate)
                .OrderBy(a => a.Priority)
                .ThenBy(a => a.Date).ToList();
        }

        public List<AlarmEntity> GetAlarmsByPriority(int priority)
        {
            List<AlarmEntity> alarms = null;
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    alarms = dbContext.alarms.ToList();
                }
            }
            return alarms
                        .Where(a => a.Priority == priority)
                        .OrderBy(a => a.Date)
                        .ToList();
        }

        public List<TagEntity> GetTagsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<TagEntity> tags = null;
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    tags = dbContext.tags.ToList();
                }
            }
            return tags
                   .Where(t => t.Date >= startDate && t.Date <= endDate)
                   .OrderBy(t => t.Date)
                   .ToList();
        }

        public List<TagEntity> GetLatestAnalogInputTags()
        {
            List<TagEntity> tags = null;
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    tags = dbContext.tags.ToList();
                }
            }
            return tags
                        .Where(t => t.Type == TagType.ANALOG_INPUT)
                        .GroupBy(t => t.TagName)
                        .Select(g => g.OrderByDescending(t => t.Date).FirstOrDefault())
                        .OrderBy(t => t.Date)
                        .ToList();
        }

        public List<TagEntity> GetLatestDigitalInputTags()
        {
            List<TagEntity> tags = null;
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    tags = dbContext.tags.ToList();
                }
            }
            return tags
                        .Where(t => t.Type == TagType.DIGITAL_INPUT)
                        .GroupBy(t => t.TagName)
                        .Select(g => g.OrderByDescending(t => t.Date).FirstOrDefault())
                        .OrderBy(t => t.Date)
                        .ToList();
        }

        public List<TagEntity> GetTagValuesByIdentifier(string tagName)
        {
            List<TagEntity> tags = null;
            lock (lockDatabase)
            {
                using (DatabaseContext dbContext = new DatabaseContext())
                {
                    tags = dbContext.tags.ToList();
                }
            }
            return tags
                        .Where(t => t.TagName == tagName)
                        .OrderBy(t => t.Value)
                        .ToList();
        }
    }
}
