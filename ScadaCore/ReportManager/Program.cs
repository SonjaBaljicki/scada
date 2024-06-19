using System;
using System.Collections.Generic;
using System.Linq;
using ReportManager.ServiceReference1;

namespace ReportManager
{
    internal class Program
    {
        static IReportService client = new ReportServiceClient();

        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("\n\nSelect the report you want to generate:");
                Console.WriteLine("1. All alarms within a specific date range");
                Console.WriteLine("2. All alarms of a specific priority");
                Console.WriteLine("3. All tag values within a specific date range");
                Console.WriteLine("4. Latest value of all AI tags");
                Console.WriteLine("5. Latest value of all DI tags");
                Console.WriteLine("6. All values of a tag with a specific identifier");
                Console.WriteLine("7. Exit");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("\nEnter start date (yyyy-MM-dd):");
                            DateTime startDate = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("Enter end date (yyyy-MM-dd):");
                            DateTime endDate = DateTime.Parse(Console.ReadLine());
                            AlarmEntity[] alarms = client.GetAlarmsByDateRange(startDate, endDate);
                            PrintAlarms(alarms.ToList());
                            break;

                        case "2":
                            Console.WriteLine("\nEnter priority:");
                            int priority = int.Parse(Console.ReadLine());
                            AlarmEntity[] priorityAlarms = client.GetAlarmsByPriority(priority);
                            PrintAlarms(priorityAlarms.ToList());
                            break;

                        case "3":
                            Console.WriteLine("\nEnter start date (yyyy-MM-dd):");
                            startDate = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("Enter end date (yyyy-MM-dd):");
                            endDate = DateTime.Parse(Console.ReadLine());
                            TagEntity[] tags = client.GetTagsByDateRange(startDate, endDate);
                            PrintTags(tags.ToList());
                            break;

                        case "4":
                            Console.WriteLine();
                            TagEntity[] latestAITags = client.GetLatestAnalogInputTags();
                            PrintTags(latestAITags.ToList());
                            break;

                        case "5":
                            Console.WriteLine();
                            List<TagEntity> latestDITags = client.GetLatestDigitalInputTags().ToList();
                            PrintTags(latestDITags);
                            break;

                        case "6":
                            Console.WriteLine();
                            Console.WriteLine("Enter tag identifier:");
                            string tagName = Console.ReadLine();
                            TagEntity[] tagValues = client.GetTagValuesByIdentifier(tagName);
                            PrintTags(tagValues.ToList());
                            break;

                        case "7":
                            return;

                        default:
                            Console.WriteLine("Invalid input. Try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    client = new ReportServiceClient();
                }
            }
        }

        static void PrintAlarms(List<AlarmEntity> alarms)
        {
            if (alarms == null || alarms.Count == 0)
            {
                Console.WriteLine("\nNo alarms to display.");
            }
            else
            {
                Console.WriteLine("\nAlarms:");
                foreach (var alarm in alarms)
                {
                    Console.WriteLine($"Priority: {alarm.Priority} \n\tDate: {alarm.Date} \n\tTag: {alarm.TagName}");
                }
            }
        }

        static void PrintTags(List<TagEntity> tags)
        {
            if (tags == null || tags.Count == 0)
            {
                Console.WriteLine("\nNo tags to display.");
            }
            else
            {
                Console.WriteLine("\nTags:");
                foreach (var tag in tags)
                {
                    Console.WriteLine($"TagName: {tag.TagName} \n\tDate: {tag.Date} \n\tValue: {tag.Value}");
                }
            }

        }
    }
}
