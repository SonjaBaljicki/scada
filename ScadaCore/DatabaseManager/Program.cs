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
                Console.WriteLine("1. Log out");

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
                                service.LogOut(token);
                                token = "";
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
