using System;
using System.Collections.ObjectModel;
using System.Globalization;
using FileCabinetApp;

namespace FileCabinetApp
{
    /// <summary>The Program Class.</summary>
    public static class Program
    {
        private const string DeveloperName = "Ivan Markevich";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static IFileCabinetService fileCabinetService = new FileCabinetCustomService();

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "create", "creates a record in the list", "The 'create' command leads to the screen where records can be created" },
            new string[] { "edit", "edits a record in the list", "The 'edit' command leads to the screen where you can recreate the record" },
            new string[] { "find", "finds a record with a specified property and its specified value", "The 'find' command leads to the screen where you can find a record" },
            new string[] { "stat", "prints the records' statistics", "The 'stat' command prints the count of the list." },
            new string[] { "list", "gets the list of the records", "The 'list' command prints out all the records in list." },
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                if (args[0].Equals("--validation-rules=default", StringComparison.InvariantCultureIgnoreCase) ||
                    (args[0] == "-v" && args[1].Equals("default", StringComparison.CurrentCultureIgnoreCase)))
                {
                    fileCabinetService = new FileCabinetDefaultService();
                    Console.WriteLine("Using default validation rules.");
                }

                if (args[0].Equals("--validation-rules=custom", StringComparison.InvariantCultureIgnoreCase) ||
                    (args[0] == "-v" && args[1].Equals("custom", StringComparison.CurrentCultureIgnoreCase)))
                {
                    fileCabinetService = new FileCabinetCustomService();
                    Console.WriteLine("Using custom validation rules.");
                }
            }
            else
            {
                Console.WriteLine("Using default validation rules.");
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        /// <summary>Prints the missed command information.</summary>
        /// <param name="command">The command.</param>
        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        /// <summary>Prints the help.</summary>
        /// <param name="parameters">The parameters.</param>
        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        /// <summary>Creates the record.</summary>
        /// <param name="parameters">The parameters.</param>
        private static void Create(string parameters)
        {
            int id;
            string firstName;
            string lastName;
            short code;
            char letter;
            decimal balance;
            DateTime dateOfBirth;
            try
            {
                Console.Write("First Name: ");
                firstName = Console.ReadLine();
                Console.Write("Last Name: ");
                lastName = Console.ReadLine();
                Console.Write("Code: ");
                code = short.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                Console.Write("Letter: ");
                letter = Console.ReadKey().KeyChar;
                Console.WriteLine();
                Console.Write("Balance: ");
                balance = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                Console.Write("Date of birth: ");
                dateOfBirth = DateTime.ParseExact(Console.ReadLine(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                RecordData recordDataToCreate = new RecordData(firstName, lastName, code, letter, balance, dateOfBirth);
                id = fileCabinetService.CreateRecord(recordDataToCreate);
                Console.WriteLine($"Record #{id} has been created.");
            }
            catch (Exception)
            {
                Create(parameters);
            }
        }

        /// <summary>Edits the record.</summary>
        /// <param name="parameters">The parameters.</param>
        private static void Edit(string parameters)
        {
            int id = 0;
            string firstName;
            string lastName;
            short code;
            char letter;
            decimal balance;
            DateTime dateOfBirth;
            try
            {
                Console.WriteLine("Id: ");
                id = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                Console.Write("First Name: ");
                firstName = Console.ReadLine();
                Console.Write("Last Name: ");
                lastName = Console.ReadLine();
                Console.Write("Code: ");
                code = short.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                Console.Write("Letter: ");
                letter = Console.ReadKey().KeyChar;
                Console.WriteLine();
                Console.Write("Balance: ");
                balance = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                Console.Write("Date of birth: ");
                dateOfBirth = DateTime.ParseExact(Console.ReadLine(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                RecordData recordDataToEdit = new RecordData(firstName, lastName, code, letter, balance, dateOfBirth);
                recordDataToEdit.Id = id;
                fileCabinetService.EditRecord(recordDataToEdit);
                Console.WriteLine($"Record #{id} is updated.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"#{id} record is not found.");
            }
        }

        /// <summary>Finds the record.</summary>
        /// <param name="parameters">The parameters.</param>
        private static void Find(string parameters)
        {
            string[] parametersArray = parameters.Split('"');
            string propertyName = parametersArray[0];
            string valueToFind = parametersArray[1];
            if (propertyName.Equals("firstname ", StringComparison.InvariantCultureIgnoreCase))
            {
                ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = fileCabinetService.FindByFirstName(valueToFind);
                foreach (FileCabinetRecord record in arrayOfRecords)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Code}, {record.Letter}, {record.Balance.ToString(CultureInfo.InvariantCulture)}, {record.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}");
                }
            }

            if (propertyName.Equals("lastname ", StringComparison.InvariantCultureIgnoreCase))
            {
                ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = fileCabinetService.FindByLastName(valueToFind);
                foreach (FileCabinetRecord record in arrayOfRecords)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Code}, {record.Letter}, {record.Balance.ToString(CultureInfo.InvariantCulture)}, {record.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}");
                }
            }

            if (propertyName.Equals("dateofbirth ", StringComparison.InvariantCultureIgnoreCase))
            {
                DateTime parameterDateTime = DateTime.ParseExact(valueToFind, "yyyy-MMM-dd", CultureInfo.InvariantCulture);
                ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = fileCabinetService.FindByDateOfBirth(parameterDateTime);
                foreach (FileCabinetRecord record in arrayOfRecords)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Code}, {record.Letter}, {record.Balance.ToString(CultureInfo.InvariantCulture)}, {record.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}");
                }
            }
        }

        /// <summary>  Gets the list of the records.</summary>
        /// <param name="parameters">The parameters.</param>
        private static void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> arrayOfRecords = fileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in arrayOfRecords)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.Code}, {record.Letter}, {record.Balance.ToString(CultureInfo.InvariantCulture)}, {record.DateOfBirth.ToString("yyyy-MMM-dd", System.Globalization.CultureInfo.InvariantCulture)}");
            }
        }

        /// <summary>  Shows the stat.</summary>
        /// <param name="parameters">The parameters.</param>
        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        /// <summary>Exits from the application.</summary>
        /// <param name="parameters">The parameters.</param>
        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}