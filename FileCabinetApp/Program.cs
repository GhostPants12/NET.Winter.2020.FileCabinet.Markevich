using System;
using System.Buffers;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using FileCabinetApp;
using FileCabinetApp.IRecordValidator;

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
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "create", "creates a record in the list", "The 'create' command leads to the screen where records can be created" },
            new string[] { "edit", "edits a record in the list", "The 'edit' command leads to the screen where you can recreate the record" },
            new string[] { "find", "finds a record with a specified property and its specified value", "The 'find' command leads to the screen where records can be found" },
            new string[] { "remove", "removes record with a specified index from the cabinet", "The 'remove' command leads to the screen where records can be deleted"},
            new string[] { "purge", "cleans up records' list by removing deleted records", "The 'purge' command leads to the screen where records are purged"},
            new string[] { "stat", "prints the records' statistics", "The 'stat' command prints the count of the list." },
            new string[] { "list", "gets the list of the records", "The 'list' command prints out all the records in list." },
            new string[] { "export", "exports the data to csv or xml format", "The 'export' command leads to the screen where records can be exported" },
            new string[] { "import", "imports the data to csv or xml format", "The 'import' command leads to the screen where records can be imported" },
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

                if (args[0].Equals("--storage=memory", StringComparison.InvariantCultureIgnoreCase) ||
                    (args[0] == "-s" && args[1].Equals("memory", StringComparison.CurrentCultureIgnoreCase)))
                {
                    fileCabinetService = new FileCabinetDefaultService();
                    Console.WriteLine("Using default validation rules.");
                }

                if (args[0].Equals("--storage=file", StringComparison.InvariantCultureIgnoreCase) ||
                    (args[0] == "-s" && args[1].Equals("file", StringComparison.CurrentCultureIgnoreCase)))
                {
                    fileCabinetService = new FileCabinetFilesystemService(new FileStream("cabinet-records.db", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None), new DefaultValidator());
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
                firstName = ReadInput<string>(ConvertStringToString, ValidateName);
                Console.Write("Last Name: ");
                lastName = ReadInput<string>(ConvertStringToString, ValidateName);
                Console.Write("Code: ");
                code = ReadInput<short>(ConvertStringToShort, ValidateCode);
                Console.Write("Letter: ");
                letter = ReadInput<char>(ConvertStringToChar, ValidateLetter);
                Console.Write("Balance: ");
                balance = ReadInput<decimal>(ConvertStringToDecimal, ValidateBalance);
                Console.Write("Date of birth: ");
                dateOfBirth = ReadInput<DateTime>(ConvertStringToDate, ValidateDate);
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
                firstName = ReadInput<string>(ConvertStringToString, ValidateName);
                Console.Write("Last Name: ");
                lastName = ReadInput<string>(ConvertStringToString, ValidateName);
                Console.Write("Code: ");
                code = ReadInput<short>(ConvertStringToShort, ValidateCode);
                Console.Write("Letter: ");
                letter = ReadInput<char>(ConvertStringToChar, ValidateLetter);
                Console.Write("Balance: ");
                balance = ReadInput<decimal>(ConvertStringToDecimal, ValidateBalance);
                Console.Write("Date of birth: ");
                dateOfBirth = ReadInput<DateTime>(ConvertStringToDate, ValidateDate);
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
            string propertyName = string.Empty;
            string valueToFind = string.Empty;
            try
            {
                string[] parametersArray = parameters.Split('"');
                propertyName = parametersArray[0];
                valueToFind = parametersArray[1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command parameters are incorrect.");
            }

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

        private static void Remove(string parameters)
        {
            int id = int.Parse(parameters, CultureInfo.InvariantCulture);
            try
            {
                fileCabinetService.DeleteRecord(id);
                Console.WriteLine($"Record #{id} was successfully deleted.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Purge(string parameters)
        {
            try
            {
                int purged = fileCabinetService.Purge();
                Console.WriteLine($"{purged} elements from {fileCabinetService.GetStat() + purged} were purged.");
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Export(string parameters)
        {
            string[] parametersArray = parameters.Split(' ');
            string formatName = parametersArray[0];
            string path = parametersArray[1];
            try
            {
                if (File.Exists(path))
                {
                    while (true)
                    {
                        Console.Write($"File is exist - rewrite {path} [Y/n] ");
                        string answer = Console.ReadKey().KeyChar.ToString(CultureInfo.InvariantCulture);
                        Console.WriteLine();
                        if (answer.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                        {
                            break;
                        }

                        if (answer.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return;
                        }
                    }
                }

                if (formatName.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (StreamWriter sr = new StreamWriter(new FileStream(path, FileMode.Create)))
                    {
                        fileCabinetService.MakeSnapshot().SaveToCsv(sr);
                        Console.WriteLine($"All records are exported to {path}");
                        sr.Close();
                        return;
                    }
                }

                if (formatName.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (StreamWriter sr = new StreamWriter(new FileStream(path, FileMode.Create)))
                    {
                        fileCabinetService.MakeSnapshot().SaveToXml(sr);
                        Console.WriteLine($"All records are exported to {path}");
                        sr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Export failed: " + ex.Message);
            }
        }

        private static void Import(string parameters)
        {
            string[] parametersArray = parameters.Split(' ');
            string formatName = parametersArray[0];
            string path = parametersArray[1];
            try
            {
                if (formatName.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (StreamReader sr = new StreamReader(new FileStream(path, FileMode.Open)))
                    {
                        FileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();
                        snapshot.LoadFromCsv(sr);
                        fileCabinetService.Restore(snapshot);
                        sr.Close();
                    }

                    Console.WriteLine("All records were imported.");
                }

                if (formatName.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        FileCabinetServiceSnapshot snapshot = fileCabinetService.MakeSnapshot();
                        snapshot.LoadFromXml(fs);
                        fileCabinetService.Restore(snapshot);
                        fs.Close();
                    }

                    Console.WriteLine("All records were imported.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Import failed: " + ex.Message);
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
            var recordsToRemoveCount = fileCabinetService.GetRemovedStat();
            Console.WriteLine($"{recordsCount} record(s).");
            if (recordsToRemoveCount == 0)
            {
                Console.WriteLine("There are no records to remove.");
                return;
            }

            Console.WriteLine($"There are {recordsToRemoveCount} records to remove.");
        }

        /// <summary>Exits from the application.</summary>
        /// <param name="parameters">The parameters.</param>
        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static Tuple<bool, string, string> ConvertStringToString(string value)
        {
            return new Tuple<bool, string, string>(true, value, value);
        }

        private static Tuple<bool, string, char> ConvertStringToChar(string value)
        {
            if (value.Length == 1)
            {
                return new Tuple<bool, string, char>(true, value, value[0]);
            }

            return new Tuple<bool, string, char>(false, value, '0');
        }

        private static Tuple<bool, string, short> ConvertStringToShort(string value)
        {
            short result;
            if (short.TryParse(value, out result))
            {
                return new Tuple<bool, string, short>(true, value, result);
            }

            return new Tuple<bool, string, short>(false, value, result);
        }

        private static Tuple<bool, string, decimal> ConvertStringToDecimal(string value)
        {
            decimal result;
            if (decimal.TryParse(value, out result))
            {
                return new Tuple<bool, string, decimal>(true, value, result);
            }

            return new Tuple<bool, string, decimal>(false, value, result);
        }

        private static Tuple<bool, string, DateTime> ConvertStringToDate(string value)
        {
            DateTime result;
            if (DateTime.TryParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return new Tuple<bool, string, DateTime>(true, value, result);
            }

            return new Tuple<bool, string, DateTime>(false, value, result);
        }

        private static Tuple<bool, string> ValidateDate(DateTime date)
        {
            try
            {
                fileCabinetService.GetValidator().ValidateParameters("1234", "1234", 123, 'a', 1, date);
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> ValidateName(string firstname)
        {
            try
            {
                fileCabinetService.GetValidator()
                    .ValidateParameters(firstname, "1234", 123, 'a', 1, new DateTime(1990, 1, 1));
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> ValidateCode(short code)
        {
            try
            {
                fileCabinetService.GetValidator().ValidateParameters("1234", "1234", code, 'a', 1, new DateTime(1990, 1, 1));
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> ValidateBalance(decimal balance)
        {
            try
            {
                fileCabinetService.GetValidator().ValidateParameters("1234", "1234", 123, 'a', balance, new DateTime(1990, 1, 1));
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string> ValidateLetter(char letter)
        {
            try
            {
                fileCabinetService.GetValidator().ValidateParameters("1234", "1234", 123, letter, 1, new DateTime(1990, 1, 1));
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}