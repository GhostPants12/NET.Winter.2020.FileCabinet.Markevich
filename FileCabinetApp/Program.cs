using System;
using System.Globalization;
using System.IO;
using System.Linq;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.FileCabinetService;
using FileCabinetApp.RecordValidator;

namespace FileCabinetApp
{
    /// <summary>The Program Class.</summary>
    public static class Program
    {
        #pragma warning disable CA1303 // Do not pass literals as localized parameters

        private const string DeveloperName = "Ivan Markevich";
        private const string HintMessage = "Enter your command, or enter 'help' To get help.";

        private static bool isRunning = true;

        private static IFileCabinetService fileCabinetService = new FileCabinetCustomService();

        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            if (args?.Length >= 1)
            {
                for (int i = 0; i < args.Length - 1; i++)
                {
                    if (args[i].Equals("--validation-rules=default", StringComparison.InvariantCultureIgnoreCase) ||
                        (args[i] == "-v" && args[i + 1].Equals("default", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        fileCabinetService = new FileCabinetDefaultService();
                        Console.WriteLine("Using default validation rules.");
                        break;
                    }

                    if (args[i].Equals("--validation-rules=custom", StringComparison.InvariantCultureIgnoreCase) ||
                        (args[i] == "-v" && args[i + 1].Equals("custom", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        fileCabinetService = new FileCabinetCustomService();
                        Console.WriteLine("Using custom validation rules.");
                        break;
                    }

                    if (args[i].Equals("--storage=memory", StringComparison.InvariantCultureIgnoreCase) ||
                        (args[i] == "-s" && args[i + 1].Equals("memory", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        fileCabinetService = new FileCabinetDefaultService();
                        Console.WriteLine("Using default validation rules.");
                        break;
                    }

                    if (args[i].Equals("--storage=file", StringComparison.InvariantCultureIgnoreCase) ||
                        (args[i] == "-s" && args[i + 1].Equals("file", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        fileCabinetService = new FileCabinetFilesystemService(
                            new FileStream("cabinet-records.db", FileMode.Create, FileAccess.ReadWrite, FileShare.None),
                            new ValidatorBuilder().CreateDefault());
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Using default validation rules.");
            }

            if (args.Contains("use-stopwatch"))
            {
                fileCabinetService = new RecordMeter(fileCabinetService);
            }

            if (args.Contains("use-logger"))
            {
                fileCabinetService = new ServiceLogger(fileCabinetService);
            }

            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            do
            {
                try
                {
                    Console.Write("> ");
                    var inputs = Console.ReadLine()?.Split(' ', 2);
                    const int commandIndex = 0;
                    var command = inputs[commandIndex];

                    if (string.IsNullOrEmpty(command))
                    {
                        Console.WriteLine(HintMessage);
                        continue;
                    }

                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    CreateCommandHandler().Handle(new AppCommandRequest(command, parameters));
                }
                #pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                #pragma warning restore CA1031 // Do not catch general exception types
                {
                    Console.WriteLine(ex.Message);
                }
            }
            while (isRunning);
        }

        /// <summary>Converts the string to string.</summary>
        /// <param name="value">The value.</param>
        /// <returns>Tuple, containing three values: first - is conversion completed, second - value to convert, third - result of conversion.</returns>
        public static Tuple<bool, string, string> ConvertStringToString(string value)
        {
            return new Tuple<bool, string, string>(true, value, value);
        }

        /// <summary>Converts the string to char.</summary>
        /// <param name="value">The value.</param>
        /// <returns>Tuple, containing three values: first - is conversion completed, second - value to convert, third - result of conversion.</returns>
        public static Tuple<bool, string, char> ConvertStringToChar(string value)
        {
            if (value?.Length == 1)
            {
                return new Tuple<bool, string, char>(true, value, value[0]);
            }

            return new Tuple<bool, string, char>(false, value, '0');
        }

        /// <summary>Converts the string to short.</summary>
        /// <param name="value">The value.</param>
        /// <returns>Tuple, containing three values: first - is conversion completed, second - value to convert, third - result of conversion.</returns>
        public static Tuple<bool, string, short> ConvertStringToShort(string value)
        {
            if (short.TryParse(value, out var result))
            {
                return new Tuple<bool, string, short>(true, value, result);
            }

            return new Tuple<bool, string, short>(false, value, result);
        }

        /// <summary>Converts the string to decimal.</summary>
        /// <param name="value">The value.</param>
        /// <returns>Tuple, containing three values: first - is conversion completed, second - value to convert, third - result of conversion.</returns>
        public static Tuple<bool, string, decimal> ConvertStringToDecimal(string value)
        {
            if (decimal.TryParse(value, out var result))
            {
                return new Tuple<bool, string, decimal>(true, value, result);
            }

            return new Tuple<bool, string, decimal>(false, value, result);
        }

        /// <summary>Converts the string to date.</summary>
        /// <param name="value">The value.</param>
        /// <returns>Tuple, containing three values: first - is conversion completed, second - value to convert, third - result of conversion.</returns>
        public static Tuple<bool, string, DateTime> ConvertStringToDate(string value)
        {
            if (DateTime.TryParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return new Tuple<bool, string, DateTime>(true, value, result);
            }

            return new Tuple<bool, string, DateTime>(false, value, result);
        }

        /// <summary>Validates the date.</summary>
        /// <param name="date">The date.</param>
        /// <returns>Tuple with two elements: first - is date correct, second - message if it is not correct, empty string if it is not.</returns>
        public static Tuple<bool, string> ValidateDate(DateTime date)
        {
            try
            {
                fileCabinetService.GetValidator().Validate("1234", "1234", 123, 'a', 1, date);
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>Validates the name.</summary>
        /// <param name="firstname">The date.</param>
        /// <returns>Tuple with two elements: first - is name correct, second - message if it is not correct, empty string if it is not.</returns>
        public static Tuple<bool, string> ValidateName(string firstname)
        {
            try
            {
                fileCabinetService.GetValidator()
                    .Validate(firstname, "1234", 123, 'a', 1, new DateTime(1990, 1, 1));
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>Validates the code.</summary>
        /// <param name="code">The code.</param>
        /// <returns>Tuple with two elements: first - is code correct, second - message if it is not correct, empty string if it is not.</returns>
        public static Tuple<bool, string> ValidateCode(short code)
        {
            try
            {
                fileCabinetService.GetValidator().Validate("1234", "1234", code, 'a', 1, new DateTime(1990, 1, 1));
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>Validates the balance.</summary>
        /// <param name="balance">The balance.</param>
        /// <returns>Tuple with two elements: first - is balance correct, second - message if it is not correct, empty string if it is not.</returns>
        public static Tuple<bool, string> ValidateBalance(decimal balance)
        {
            try
            {
                fileCabinetService.GetValidator().Validate("1234", "1234", 123, 'a', balance, new DateTime(1990, 1, 1));
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>Validates the letter.</summary>
        /// <param name="letter">The letter.</param>
        /// <returns>Tuple with two elements: first - is letter correct, second - message if it is not correct, empty string if it is not.</returns>
        public static Tuple<bool, string> ValidateLetter(char letter)
        {
            try
            {
                fileCabinetService.GetValidator().Validate("1234", "1234", 123, letter, 1, new DateTime(1990, 1, 1));
            }
            catch (ArgumentException e)
            {
                return new Tuple<bool, string>(false, e.Message);
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        /// <summary>Reads the input, validates it and converts to the specified type.</summary>
        /// <typeparam name="T">Type to validate and convert.</typeparam>
        /// <param name="converter">The converter.</param>
        /// <param name="validator">The validator.</param>
        /// <returns>Returns the value of specified type.</returns>
        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                var value = conversionResult.Item3;

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

        /// <summary>Creates the command handler.</summary>
        /// <returns>CommandHandler that refers to all the commands via chain of responsibility.</returns>
        private static CommandHandlerBase CreateCommandHandler()
        {
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler(b => isRunning = b);
            var missedHandler = new MissedCommandHandler();
            var insertHandler = new InsertCommandHandler(fileCabinetService);
            var deleteHandler = new DeleteCommandHandler(fileCabinetService);
            var updateHandler = new UpdateCommandHandler(fileCabinetService);
            var selectHandler = new SelectCommandHandler(fileCabinetService);
            exitHandler.SetNext(missedHandler);
            importHandler.SetNext(exitHandler);
            exportHandler.SetNext(importHandler);
            statHandler.SetNext(exportHandler);
            purgeHandler.SetNext(statHandler);
            createHandler.SetNext(purgeHandler);
            updateHandler.SetNext(createHandler);
            deleteHandler.SetNext(updateHandler);
            selectHandler.SetNext(deleteHandler);
            insertHandler.SetNext(selectHandler);
            return insertHandler;
        }
    }
}