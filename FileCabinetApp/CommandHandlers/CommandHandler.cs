using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class CommandHandler : CommandHandlerBase
    {
        public new void Handle(AppCommandRequest request)
        {

        }

        /// <summary>Prints the missed command information.</summary>
        /// <param name="command">The command.</param>
        public static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        /// <summary>Prints the help.</summary>
        /// <param name="parameters">The parameters.</param>
        public static void PrintHelp(string parameters)
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
        public static void Create(string parameters)
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
                id = Program.fileCabinetService.CreateRecord(recordDataToCreate);
                Console.WriteLine($"Record #{id} has been created.");
            }
            catch (Exception)
            {
                Create(parameters);
            }
        }

        /// <summary>Edits the record.</summary>
        /// <param name="parameters">The parameters.</param>
        public static void Edit(string parameters)
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
            try
            {
                int id = int.Parse(parameters, CultureInfo.InvariantCulture);
                fileCabinetService.DeleteRecord(id);
                Console.WriteLine($"Record #{id} was successfully deleted.");
            }
            catch (Exception ex)
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
