﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using FileCabinetApp;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.IRecordValidator;

namespace FileCabinetApp
{
    /// <summary>The Program Class.</summary>
    public static class Program
    {
        private const string DeveloperName = "Ivan Markevich";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        public static bool isRunning = true;

        private static IFileCabinetService fileCabinetService = new FileCabinetCustomService();

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

                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                CreateCommandHandler().Handle(new AppCommandRequest(command, parameters));
            }
            while (isRunning);
        }

        private static CommandHandlerBase CreateCommandHandler()
        {
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var editHandler = new EditCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService);
            var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var statHandler = new StatCommandHandler();
            var listHandler = new ListCommandHandler(fileCabinetService);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var exitHandler = new ExitCommandHandler();
            var missedHandler = new MissedCommandHandler();
            exitHandler.SetNext(missedHandler);
            importHandler.SetNext(exitHandler);
            exportHandler.SetNext(importHandler);
            listHandler.SetNext(exportHandler);
            statHandler.SetNext(listHandler);
            purgeHandler.SetNext(statHandler);
            removeHandler.SetNext(purgeHandler);
            findHandler.SetNext(removeHandler);
            editHandler.SetNext(findHandler);
            createHandler.SetNext(editHandler);
            return createHandler;
        }

        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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

        public static Tuple<bool, string, string> ConvertStringToString(string value)
        {
            return new Tuple<bool, string, string>(true, value, value);
        }

        public static Tuple<bool, string, char> ConvertStringToChar(string value)
        {
            if (value.Length == 1)
            {
                return new Tuple<bool, string, char>(true, value, value[0]);
            }

            return new Tuple<bool, string, char>(false, value, '0');
        }

        public static Tuple<bool, string, short> ConvertStringToShort(string value)
        {
            short result;
            if (short.TryParse(value, out result))
            {
                return new Tuple<bool, string, short>(true, value, result);
            }

            return new Tuple<bool, string, short>(false, value, result);
        }

        public static Tuple<bool, string, decimal> ConvertStringToDecimal(string value)
        {
            decimal result;
            if (decimal.TryParse(value, out result))
            {
                return new Tuple<bool, string, decimal>(true, value, result);
            }

            return new Tuple<bool, string, decimal>(false, value, result);
        }

        public static Tuple<bool, string, DateTime> ConvertStringToDate(string value)
        {
            DateTime result;
            if (DateTime.TryParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return new Tuple<bool, string, DateTime>(true, value, result);
            }

            return new Tuple<bool, string, DateTime>(false, value, result);
        }

        public static Tuple<bool, string> ValidateDate(DateTime date)
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

        public static Tuple<bool, string> ValidateName(string firstname)
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

        public static Tuple<bool, string> ValidateCode(short code)
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

        public static Tuple<bool, string> ValidateBalance(decimal balance)
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

        public static Tuple<bool, string> ValidateLetter(char letter)
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