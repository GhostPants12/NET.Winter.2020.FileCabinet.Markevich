using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        public static bool isRunning = true;

        public static IFileCabinetService fileCabinetService = new FileCabinetCustomService();

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

                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                CreateCommandHandler().Handle(new AppCommandRequest(command, parameters));
            }
            while (isRunning);
        }

        private static CommandHandler CreateCommandHandler()
        {
            var commandHandler = new CommandHandler();
            return commandHandler;
        }
    }
}