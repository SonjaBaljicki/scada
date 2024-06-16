using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Windows.Markup;

namespace DatabaseManager
{
    class Program
    {
        static ServiceReference1.IDatabaseManagerService service = new ServiceReference1.DatabaseManagerServiceClient();
        static string token = "";
        static void Main(string[] args)
        {
            string input = "";
            while (input != "exit")
            { 
                Console.WriteLine("\n\n");
                Console.WriteLine("Choose an option by entering a number or 'exit' to exit:");
                Console.WriteLine("1. Log in");
                Console.WriteLine("2. Register");

                input = Console.ReadLine();
                int inputInt = 0;
                bool checkInput = int.TryParse(input, out inputInt);
                if (checkInput)
                {
                    if (inputInt > 0)
                    {
                        switch (inputInt)
                        {
                            case 1:
                                Login();
                                break;
                            case 2:
                                Register();
                                break;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            service.StopTagThreads();
        }

        private static void LoggedInMenu()
        {
            string input = "";
            while (true)
            {
                Console.WriteLine("\n\n");
                Console.WriteLine("Choose an option by entering a number:");
                Console.WriteLine("1. Add a new digital input tag");
                Console.WriteLine("2. Add a new digital output tag");
                Console.WriteLine("3. Add a new analog input tag");
                Console.WriteLine("4. Add a new analog output tag");
                Console.WriteLine("5. Disable input tag scan");
                Console.WriteLine("6. Enable input tag scan");
                Console.WriteLine("7. Add alarm to analog input");
                Console.WriteLine("8. Delete alarm from analog input");
                Console.WriteLine("9. Change value of digital output");
                Console.WriteLine("10. Change value of analog output");
                Console.WriteLine("11. Display all values of digital outputs");
                Console.WriteLine("12. Display all values of analog outputs");
                Console.WriteLine("13. Delete input tag");
                Console.WriteLine("14. Delete output tag");
                Console.WriteLine("15. Log out");

                input = Console.ReadLine();
                int inputInt = 0;
                bool checkInput = int.TryParse(input, out inputInt);
                if (checkInput)
                {
                    if (inputInt > 0)
                    {
                        switch (inputInt)
                        {
                            case 1:
                                Console.WriteLine();
                                AddDigitalInputTag();
                                break;
                            case 2:
                                Console.WriteLine();
                                AddDigitalOutputTag();
                                break;
                            case 3:
                                Console.WriteLine();
                                AddAnalogInputTag();
                                break;
                            case 4:
                                Console.WriteLine();
                                AddAnalogOutputTag();
                                break;
                            case 5:
                                Console.WriteLine();
                                TurnOffScan();
                                break;
                            case 6:
                                Console.WriteLine();
                                TurnOnScan();
                                break;
                            case 7:
                                Console.WriteLine();
                                AddAnalogAlarm();
                                break;
                            case 8:
                                Console.WriteLine();
                                DeleteAlarm();
                                break;
                            case 9:
                                Console.WriteLine();
                                ChangeValueDigitalOutput();
                                break;
                            case 10:
                                Console.WriteLine();
                                ChangeValueAnalogOutput();
                                break;
                            case 11:
                                Console.WriteLine();
                                ShowDigitalOutputs();
                                break;
                            case 12:
                                Console.WriteLine();
                                ShowAnalogOutputs();
                                break;
                            case 13:
                                Console.WriteLine();
                                DeleteInputTag();
                                break;
                            case 14:
                                Console.WriteLine();
                                DeleteOutputTag();
                                break;
                            case 15:
                                service.LogOut(token);
                                token = "";
                                Console.WriteLine("\n\n");
                                Console.WriteLine("Log out successful!");
                                return;
                            default:
                                continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
               
            }
        }

        private static void DeleteOutputTag()
        {
            throw new NotImplementedException();
        }

        private static void DeleteInputTag()
        {
            throw new NotImplementedException();
        }

        private static void ShowAnalogOutputs()
        {
            Dictionary<string, int> outputs = service.GetAnalogOutputTags();
            Console.WriteLine("Analog output tags:");
            foreach (var kv in outputs)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }
        }

        private static void ShowDigitalOutputs()
        {
            Dictionary<string, int> outputs = service.GetDigitalOutputTags();
            Console.WriteLine("Digital output tags:");
            foreach (var kv in outputs)
            {
                if (kv.Value == 0) Console.WriteLine($"{kv.Key}: off");
                else Console.WriteLine($"{kv.Key}: on");
            }
        }

        private static void ChangeValueDigitalOutput()
        {
            string tagName;
            int newValue;
            Console.Write("Enter tag name: ");
            tagName = Console.ReadLine();
            Console.Write("Enter new value (0 for off, 1 for on): ");
            while (!int.TryParse(Console.ReadLine(), out newValue) || (newValue != 0 && newValue != 1))
            {
                Console.Write("Invalid input. Please enter 0 for off or 1 for on: ");
            }
            bool check = service.ChangeValueDigitalOutputTag(tagName, newValue);
            if (check)
            {
                Console.WriteLine("Value changed successfully!");
            }
            else
            {
                Console.WriteLine("Invalid tag name!");
            }
        }

        private static void ChangeValueAnalogOutput()
        {
            string tagName;
            int newValue;
            Console.Write("Enter tag name: ");
            tagName = Console.ReadLine();
            Console.Write("Enter new value (integer value): ");
            while (!int.TryParse(Console.ReadLine(), out newValue))
            {
                Console.Write("Invalid input. Please enter a valid integer for new value: ");
            }
            bool check=service.ChangeValueAnalogOutputTag(tagName, newValue);
            if (check)
            {
                Console.WriteLine("Value changed successfully!");
            }
            else
            {
                Console.WriteLine("Invalid tag name or new value!");
            }
        }

        private static void DeleteAlarm()
        {
            throw new NotImplementedException();
        }

        private static void AddAnalogAlarm()
        {
            Console.Write("Enter tag name: ");
            string tagName = Console.ReadLine();
            while (!service.ContainsAnalogInputTag(tagName))
            {
                Console.Write("Invalid input. Please enter an existing tag name: ");
                tagName = Console.ReadLine();
            }
            Console.Write("Enter alarm id: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("Invalid input. Please enter a number: ");
            }
            Console.Write("Enter alarm type (0 for low, 1 for high): ");
            int type;
            while (!int.TryParse(Console.ReadLine(), out type) || (type != 0 && type != 1))
            {
                Console.Write("Invalid input. Please enter 0 for low or 1 for high: ");
            }
            Console.Write("Enter alarm priority (1, 2 or 3): ");
            int priority;
            while (!int.TryParse(Console.ReadLine(), out priority) || (priority != 1 && priority != 2 && priority != 3))
            {
                Console.Write("Invalid input. Please enter valid priority (1, 2 or 3): ");
            }
            Console.Write("Enter edge value: ");
            double edgeValue;
            while (!double.TryParse(Console.ReadLine(), out edgeValue))
            {
                Console.Write("Invalid input. Please enter a number: ");
            }
            Console.Write("Enter alarm units: ");
            string units = Console.ReadLine();
            bool check = service.AddAnalogAlarm(tagName, id, type, priority, edgeValue, units);
        }

        private static void TurnOnScan()
        {
            Console.Write("Enter tag name: ");
            string name = Console.ReadLine();
            bool check=service.TurnOnScan(name);
            if (check)
            {
                Console.WriteLine("Turning on tag successful");
            }
            else
            {
                Console.WriteLine("Invalid tag name");
            }
        }

        private static void TurnOffScan()
        {
            Console.Write("Enter tag name: ");
            string name = Console.ReadLine();
            bool check=service.TurnOffScan(name);
            if (check)
            {
                Console.WriteLine("Turning off tag successful");
            }
            else
            {
                Console.WriteLine("Invalid tag name");
            }
        }

        private static void AddDigitalInputTag()
        {
            string name;
            string description;
            string address;
            int driver;
            int scanTime;
            bool scanOn;

            name = EnterName();

            Console.Write("Enter description: ");
            description = Console.ReadLine();

            driver=EnterDriver();

            address = EnterAddress(driver);

            Console.Write("Enter scanTime (integer value): ");
            while (!int.TryParse(Console.ReadLine(), out scanTime))
            {
                Console.Write("Invalid input. Please enter a valid integer for scanTime: ");
            }

            Console.Write("Enter scanOn (0 for false, 1 for true): ");
            int scanOnInput;
            while (!int.TryParse(Console.ReadLine(), out scanOnInput) || (scanOnInput != 0 && scanOnInput != 1))
            {
                Console.Write("Invalid input. Please enter 0 for false or 1 for true: ");
            }
            scanOn = scanOnInput == 1;

            service.AddDigitalInputTag(name, description, address, driver, scanTime, scanOn);
        }

        private static void AddDigitalOutputTag()
        {
            string name;
            string description;
            string address;
            int value;

            name = EnterName();

            Console.Write("Enter description: ");
            description = Console.ReadLine();

            Console.Write("Enter address: ");
            address = Console.ReadLine();

            Console.Write("Enter value (0 for off, 1 for on): ");
            while (!int.TryParse(Console.ReadLine(), out value) || (value != 0 && value != 1))
            {
                Console.Write("Invalid input. Please enter 0 for off or 1 for on: ");
            }

            bool check = service.AddDigitalOutputTag(name, description, address, value);
            if (check)
            {
                Console.WriteLine("Added digital output tag successfully!");
            }
        }

        private static void AddAnalogInputTag()
        {
            string name;
            string description;
            string address;
            int driver;
            int scanTime;
            bool scanOn;
            string units;

            name = EnterName();

            Console.Write("Enter description: ");
            description = Console.ReadLine();

            driver = EnterDriver();

            address = EnterAddress(driver);

            Console.Write("Enter scanTime (integer value): ");
            while (!int.TryParse(Console.ReadLine(), out scanTime))
            {
                Console.Write("Invalid input. Please enter a valid integer for scanTime: ");
            }

            Console.Write("Enter scanOn (0 for false, 1 for true): ");
            int scanOnInput;
            while (!int.TryParse(Console.ReadLine(), out scanOnInput) || (scanOnInput != 0 && scanOnInput != 1))
            {
                Console.Write("Invalid input. Please enter 0 for false or 1 for true: ");
            }
            scanOn = scanOnInput == 1;

            List<int> limits = EnterLimits();
            int lowLimit = limits[0];
            int highLimit = limits[1];

            Console.Write("Enter tag units: ");
            units = Console.ReadLine();

            service.AddAnalogInputTag(name, description, address, driver, scanTime, scanOn, lowLimit, highLimit, units);
        }

        private static void AddAnalogOutputTag()
        {
            string name;
            string description;
            string address;
            int value;
            string units;

            name = EnterName();

            Console.Write("Enter description: ");
            description = Console.ReadLine();

            Console.Write("Enter address: ");
            address = Console.ReadLine();

            List<int> limits = EnterLimits();
            int lowLimit = limits[0];
            int highLimit = limits[1];

            Console.Write("Enter initial value: ");
            while (!int.TryParse(Console.ReadLine(), out value) || (value < lowLimit || value > highLimit))
            {
                Console.Write("Invalid input. Please enter a number between low limit and high limit: ");
            }

            Console.Write("Enter tag units: ");
            units = Console.ReadLine();

            bool check = service.AddAnalogOutputTag(name, description, address, value, lowLimit, highLimit, units);
            if (check)
            {
                Console.WriteLine("Added analog output tag successfully!");
            }
        }

        private static void Login()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("Login:");
            Console.WriteLine("Username:");
            string username = Console.ReadLine();
            Console.WriteLine("Password:");
            string password = Console.ReadLine();
            string success = service.Login(username, password);
            if (success == "Login failed")
            {
                Console.WriteLine("Bad credentials!.");
                return;
            }
            else
            {
                token = success;
                LoggedInMenu();
            }
        }

        private static void Register()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("Register:");
            Console.WriteLine("Username:");
            string username = Console.ReadLine();
            Console.WriteLine("Password:");
            string password = Console.ReadLine();
            service.Registration(username, password);
            LoggedInMenu();
        }

        private static string EnterAddress(int driver)
        {
            if (driver == 0)
            {
                string address;
                Console.Write("Enter address: ");
                address = Console.ReadLine();
                return address;
            }
            else
            {
                Console.Write("Enter value S for sine, C for cosine or R for ramp: ");
                string address = Console.ReadLine().ToUpper();
                while (address != "S" && address != "C" && address != "R")
                {
                    Console.WriteLine("Wrong input, enter again!");
                    address = Console.ReadLine().ToUpper();
                }
                return address;
            }
        }

        private static int EnterDriver()
        {
            int driver;
            Console.Write("Enter driver (integer value: 0-Real time driver, 1-Simulation driver): ");
            while (!int.TryParse(Console.ReadLine(), out driver) || (driver != 0 && driver != 1))
            {
                Console.Write("Invalid input. Please enter 0 for false or 1 for true: ");
            }
            return driver;
        }


        private static string EnterName()
        {
            string name = "";
            while (true)
            {
                Console.Write("Enter tag name: ");
                name = Console.ReadLine();
                if (service.CheckTagName(name))
                {
                    Console.WriteLine("Tag name already exists!");
                    continue;
                }
                else break;
            }

            return name;
        }

        private static List<int> EnterLimits()
        {
            List<int> limits = new List<int>();
            bool check = false;
            int lowLimit = 0;
            int highLimit = 0;
            while (!check)
            {
                Console.Write("Enter low limit: ");
                string low = Console.ReadLine();
                check = int.TryParse(low, out lowLimit);
            }

            check = false;
            while (!check)
            {
                Console.Write("Enter high limit: ");
                string hight = Console.ReadLine();
                check = int.TryParse(hight, out highLimit);

                if (lowLimit > highLimit) check = false;
            }
            limits.Add(lowLimit);
            limits.Add(highLimit);
            return limits;
        }
    }
}
