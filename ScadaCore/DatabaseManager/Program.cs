using System;

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
        }

        private static void LoggedInMenu()
        {
            string input = "";
            while (true)
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Choose an option by entering a number:");
                Console.WriteLine("Choose an option by entering a number:");
                Console.WriteLine("1. Add a new digital input tag");
                Console.WriteLine("2. Add a new digital output tag");
                Console.WriteLine("3. Add a new analog input tag");
                Console.WriteLine("4. Add a new analog output tag");
                Console.WriteLine("5. Disable input tag scan");
                Console.WriteLine("6. Enable input tag scan");
                Console.WriteLine("7. Add alarm to analog input");
                Console.WriteLine("8. Add alarm to digital input");
                Console.WriteLine("9. Delete alarm from analog input");
                Console.WriteLine("10. Delete alarm from digital input");
                Console.WriteLine("11. Change value of digital output");
                Console.WriteLine("12. Change value of analog output");
                Console.WriteLine("13. Display all values of digital outputs");
                Console.WriteLine("14. Display all values of analog outputs");
                Console.WriteLine("15. Delete input tag");
                Console.WriteLine("16. Delete output tag");
                Console.WriteLine("17. Log out");

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
                                AddDigitalInputTag();
                                break;
                            case 2:
                                AddDigitalOutputTag();
                                break;
                            case 3:
                                AddAnalogInputTag();
                                break;
                            case 4:
                                AddAnalogOutputTag();
                                break;
                            case 5:
                                TurnOffScan();
                                break;
                            case 6:
                                TurnOnScan();
                                break;
                            case 7:
                                AddAnalogAlarm();
                                break;
                            case 8:
                                AddDigitalAlarm();
                                break;
                            case 9:
                                DeleteAlarm();
                                break;
                            case 10:
                                DeleteAlarm();
                                break;
                            case 11:
                                ChangeValueDigitalOutput("Enter digital output name: ");
                                break;
                            case 12:
                                ChangeValueAnalogOutput("Enter analog output name: ");
                                break;
                            case 13:
                                ShowDigitalOutputs();
                                break;
                            case 14:
                                ShowAnalogOutputs();
                                break;
                            case 15:
                                DeleteInputTag();
                                break;
                            case 16:
                                DeleteOutputTag();
                                break;
                            case 17:
                                service.LogOut(token);
                                Console.WriteLine("\n\n");
                                Console.WriteLine("Log out successful!");
                                break;
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
            throw new NotImplementedException();
        }

        private static void ShowDigitalOutputs()
        {
            throw new NotImplementedException();
        }

        private static void ChangeValueDigitalOutput(string v1)
        {
            throw new NotImplementedException();
        }

        private static void ChangeValueAnalogOutput(string v1)
        {
            throw new NotImplementedException();
        }

        private static void DeleteAlarm()
        {
            throw new NotImplementedException();
        }

        private static void AddDigitalAlarm()
        {
            throw new NotImplementedException();
        }

        private static void AddAnalogAlarm()
        {
            throw new NotImplementedException();
        }

        private static void TurnOnScan()
        {
            throw new NotImplementedException();
        }

        private static void TurnOffScan()
        {
            throw new NotImplementedException();
        }

        private static void AddDigitalInputTag()
        {
            throw new NotImplementedException();
        }

        private static void AddDigitalOutputTag()
        {
            throw new NotImplementedException();
        }

        private static void AddAnalogInputTag()
        {
            throw new NotImplementedException();
        }

        private static void AddAnalogOutputTag()
        {
            throw new NotImplementedException();
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
    }
}
